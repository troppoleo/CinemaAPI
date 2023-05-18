using CinemaBL.Enums;
using CinemaBL.Extension;
using CinemaBL.Repository;
using CinemaBL.Utility;
using CinemaDAL.Models;
using CinemaDTO;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBL
{
    public interface ICustomerService
    {
        BuyedTicketsCustomerDTO BuyTickets(BuyTicketCustomerDTO btc, int idCustomer);
        MessageForUserEnum DeleteTicket(int ticketId, int idCustomer);
        IEnumerable<MovieDetailForCustomerDTO> GetMovieDetail(int idMovieSchedule);
        IEnumerable<MovieScheduleForCustomerDTO> GetMoviesScheduled(MovieFilterForCustomerDTO ff);
        public IEnumerable<MovieWatchedDTO> GetWatchedMovies(int idCustomer);
        CrudCinemaEnum Insert(CustomerForInsertDTO cus);
        MessageForUserEnum InsertMovieReview(MovieReviewDTO mr, int idCustomer);
    }

    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWorkGeneric _uow;
        private readonly IUserUtility _userUtility;

        public CustomerService(IUnitOfWorkGeneric uow, IUserUtility userUtility)
        {
            _uow = uow;
            _userUtility = userUtility;
        }

        public BuyedTicketsCustomerDTO BuyTickets(BuyTicketCustomerDTO btc, int idCustomer)
        {
            /// controllo che 
            ///     il film sia in Waiting
            ///     abbia l'età per vederlo

            var ms = _uow.GetMovieScheduleRep.Get(x => x.Id == btc.MovieScheduleId && x.Status == Enums.MovieScheduleEnum.WAITING.ToString(), includeProperties: nameof(Movie)).FirstOrDefault();
            if (ms is not null)
            {
                var customer = _uow.GetCustomerRep.GetByID(idCustomer);

                if (_userUtility.GetAge(customer.Birthdate) < ms.Movie.LimitAge.ToDefault())
                {
                    return new BuyedTicketsCustomerDTO()
                    {
                        MessageBackForCustomer = MessageForUserEnum.LIMIT_TICKET.ToString()
                    };
                }
                if (btc.ReservedStdSeat + btc.ReservedVipSeat > 4)
                {
                    return new BuyedTicketsCustomerDTO()
                    {
                        MessageBackForCustomer = MessageForUserEnum.LIMIT_TICKET.ToString()
                    };
                }

                // decremento i posti disponibili
                ms.StdSeat -= btc.ReservedStdSeat;
                ms.VipSeat -= btc.ReservedVipSeat;
                _uow.GetMovieScheduleRep.Update(ms);

                var stdDefaultPrice = _uow.GetPriceTicketDefaultRep.GetByID(Enums.PriceTicketDefaultEnum.STD_SEAT.ToString()).Price.Value;
                var vipDefaultPercentual = _uow.GetPriceTicketDefaultRep.GetByID(Enums.PriceTicketDefaultEnum.VIP_SEAT_PERCENTUAL.ToString()).Price.Value;
                var vipDefaultPrice = stdDefaultPrice + (stdDefaultPrice * vipDefaultPercentual / 100);


                var tk = new Ticket()
                {
                    CustomerId = customer.Id,
                    MovieScheduleId = ms.Id,
                    ReservedStdSeats = btc.ReservedStdSeat,
                    ReservedVipSeat = btc.ReservedVipSeat,
                    PriceStd = stdDefaultPrice,
                    PriceVipPercent = vipDefaultPercentual,
                    DateTicket = DateTime.Now
                };

                _uow.GetTicketRep.Insert(tk);

                _uow.Save();


                //  controllo che ci siano abbastanza posti disponibili
                if (ms.StdSeat < btc.ReservedStdSeat || ms.VipSeat < btc.ReservedVipSeat)
                {
                    return new BuyedTicketsCustomerDTO()
                    {
                        MessageBackForCustomer = MessageForUserEnum.NO_SEAT.ToString()
                    };
                }
                // TODO: DECREMENTARE I POSTI DISPONIBILI IN MOVIESCHEDULE!!
                return new BuyedTicketsCustomerDTO()
                {
                    MessageBackForCustomer = MessageForUserEnum.DONE.ToString(),
                    MovieScheduleId = ms.Id,
                    IdTicket = tk.Id,
                    ReservedStdSeat = btc.ReservedStdSeat,
                    ReservedVipSeat = btc.ReservedVipSeat,
                    TotalPrice = (stdDefaultPrice * btc.ReservedStdSeat) + (vipDefaultPrice * btc.ReservedVipSeat)
                };
            }

            return new BuyedTicketsCustomerDTO()
            {
                MessageBackForCustomer = MessageForUserEnum.SCHEDULE_NOT_FOUND.ToString()
            };
        }

        public MessageForUserEnum DeleteTicket(int ticketId, int idCustomer)
        {
            Ticket? ticket = GetTicket(ticketId, idCustomer);

            if (ticket != null)
            {
                if (ticket.MovieSchedule.Status.ToEnum<MovieScheduleEnum>() != MovieScheduleEnum.WAITING)
                {
                    return MessageForUserEnum.TOO_LATE_TO_DELETE_TICKET;
                }
                var ms = _uow.GetMovieScheduleRep.GetByID(ticket.MovieScheduleId.Value);
                // incremento i posti disponibili:
                ms.StdSeat += ticket.ReservedStdSeats;
                ms.VipSeat += ticket.ReservedVipSeat;

                _uow.GetMovieScheduleRep.Update(ms);

                _uow.GetTicketRep.Delete(ticket);

                return MessageForUserEnum.DONE;
            }

            return MessageForUserEnum.USER_NOT_AUTHORIZED;
        }

        private Ticket? GetTicket(int ticketId, int idCustomer)
        {
            // nelle'estrazione controllo anche il customer sia quello che che ha fatto la prenotazione
            return _uow.GetTicketRep.Get(x => x.Id == ticketId && x.CustomerId == idCustomer,
                includeProperties: nameof(MovieSchedule)).FirstOrDefault();
        }


        public IEnumerable<MovieDetailForCustomerDTO> GetMovieDetail(int idMovieSchedule)
        {
            var ms = _uow.GetMovieScheduleRep.Get(x => x.Id == idMovieSchedule, includeProperties: $"{nameof(Movie)},{nameof(CinemaRoom)}");
            return ms.Select(x => new MovieDetailForCustomerDTO()
            {
                Actors = x.Movie.Actors,
                Duration = x.Movie.Duration,
                FilmName = x.Movie.FilmName,
                FreeSeatStd = x.CinemaRoom.MaxStdSeat - x.StdSeat.ToDefault(),
                FreeSeatVip = x.CinemaRoom.MaxVipSeat - x.VipSeat.ToDefault(),
                Genere = x.Movie.Genere,
                LimitAge = x.Movie.LimitAge.ToDefault(),
                RoomName = x.CinemaRoom.RoomName,
                StartDate = x.StartDate,
                Trama = x.Movie.Trama
            });
        }

        public IEnumerable<MovieScheduleForCustomerDTO> GetMoviesScheduled(MovieFilterForCustomerDTO ff)
        {

            IEnumerable<MovieSchedule> msFiltered = _uow.GetMovieScheduleRep
                .Get(x => x.IsApproved == 1 && x.Status == MovieScheduleEnum.WAITING.ToString(),
                includeProperties: nameof(Movie));

            if (ff.StartDate != DateTime.MinValue)
            {
                msFiltered = msFiltered.Where(x => x.StartDate.Date == ff.StartDate.Date);
            }
            if (ff.Genere.ToEnum<MovieGenereEnum>() != Enums.MovieGenereEnum.NONE)
            {
                msFiltered = msFiltered.Where(x => x.Movie.Genere == ff.Genere);
            }

            return msFiltered.Select(x => new MovieScheduleForCustomerDTO()
            {
                MovieScheduleId = x.Id,
                FilmName = x.Movie.FilmName,
                Actors = x.Movie.Actors,
                Duration = x.Movie.Duration,
                Genere = x.Movie.Genere,
                LimitAge = x.Movie.LimitAge.ToDefault(),
                StartDate = x.StartDate,
                Trama = x.Movie.Trama
            });
        }

        public Enums.CrudCinemaEnum Insert(CustomerForInsertDTO cus)
        {
            if (_userUtility.IsUsernameAlreadyUsed(cus.UserName))
            {
                return CrudCinemaEnum.ALREADY_EXISTS;
            }

            _uow.GetCustomerRep.Insert(new Customer()
            {
                Name = cus.Name,
                Surname = cus.Surname,
                UserName = cus.UserName,
                Password = cus.Password,
                Birthdate = cus.Birthdate,
                Email = cus.Email
            });

            return CrudCinemaEnum.CREATED;

            //var emp = _uow.GetCustomerRep.Get(x => x.UserName.Trim().ToLower() == cus.UserName.Trim().ToLower());
            //if (!emp.Any())
            //{
            //    _uow.GetCustomerRep.Insert(new Customer()
            //    {
            //        Name = cus.Name,
            //        Surname = cus.Surname,
            //        UserName = cus.UserName,
            //        Password = cus.Password,
            //        Birthdate = cus.Birthdate,
            //        Email = cus.Email
            //    });

            //    return CrudCinemaEnum.CREATED;
            //}


        }

        public MessageForUserEnum InsertMovieReview(MovieReviewDTO mr, int idCustomer)
        {
            Ticket? ticket = GetTicket(mr.IdTicket, idCustomer);
            if (ticket != null)
            {
                _uow.GetMovieRateRep.Insert(new MovieRate()
                {
                    ActorRate = mr.actorRate,
                    AmbientRate = mr.ambientRate,
                    CommentNote = mr.commentNote,
                    MovieId = ticket.MovieSchedule.MovieId,
                    TramaRate = mr.tramaRate,
                    CustomerId = idCustomer
                });
                return MessageForUserEnum.YOUR_RATE_HAS_BEEN_INSERTED;
            }
            return MessageForUserEnum.USER_NOT_AUTHORIZED;
        }

        public IEnumerable<MovieWatchedDTO> GetWatchedMovies(int idCustomer)
        {
            return _uow.GetViewCustomerMovieWatchedRep.Get(x => x.CustomerId == idCustomer).Select(x => new MovieWatchedDTO()
            {
                Actors = x.Actors,
                DateTicket = x.DateTicket.Value,
                director = x.Director,
                duration = x.Duration,
                FilmName = x.FilmName,
                genere = x.Genere,
                trama = x.Trama
            });

        }
    }
}
