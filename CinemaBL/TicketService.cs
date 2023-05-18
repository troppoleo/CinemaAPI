using CinemaBL.Enums;
using CinemaBL.Extension;
using CinemaBL.Repository;
using CinemaDAL.Models;
using CinemaDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBL
{
    public interface ITicketService
    {
        object TicketGenerate(TicketGenerateDTO tg);
    }

    public class TicketService : ITicketService
    {
        private readonly IUnitOfWorkGeneric _uow;

        public TicketService(IUnitOfWorkGeneric uow)
        {
            _uow = uow;
        }


        public object TicketGenerate(TicketGenerateDTO tg)
        {
            var ms = _uow.GetMovieScheduleRep.GetByID(tg.MovieScheduleId);
            if (ms == null)
            {
                // schedulazione non trovata
                return Enums.CrudCinemaEnum.VIOLATION_MINIMUM_REQUIRED.ToString();
            }
            
            if (ms.Status.ToEnum<MovieScheduleEnum>() != MovieScheduleEnum.WAITING)
            {
                // il film è già iniziato
                return Enums.CrudCinemaEnum.VIOLATION_MINIMUM_REQUIRED.ToString();
            }

            if (ms.StdSeat < tg.reservedStdSeats || ms.VipSeat < tg.reservedVipSeat)
            {
                // posti richiesti non disponibili
                return Enums.CrudCinemaEnum.VIOLATION_MINIMUM_REQUIRED.ToString();
            }


            Ticket tk = new Ticket()
            {
                MovieScheduleId = tg.MovieScheduleId,
                PriceStd = (decimal)tg.PriceStd,
                PriceVipPercent = (decimal)tg.PriceVipPercent,
                ReservedStdSeats = tg.reservedStdSeats,
                ReservedVipSeat = tg.reservedVipSeat
            };

            _uow.GetTicketRep.Insert(tk);
            
            ms.VipSeat -= tg.reservedVipSeat;
            ms.StdSeat -= tg.reservedStdSeats;
            _uow.GetMovieScheduleRep.Update(ms);

            _uow.Save();

            // prezzo posti Vip
            double vipPrice = tg.PriceStd + (tg.PriceStd * tg.PriceVipPercent / 100);
            double totalPrice = tg.reservedStdSeats * tg.PriceStd + (tg.reservedVipSeat  * vipPrice);
            TicketGenerateResultDTO tickGen = new TicketGenerateResultDTO()
            {
                IdTicket = tk.Id,
                TotalPrice = totalPrice
            };

            return new
            {
                Status = CrudCinemaEnum.CREATED.ToString(),
                TicketGenerated = tickGen
            };
        }


    }
}
