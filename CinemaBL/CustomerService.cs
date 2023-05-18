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
        IEnumerable<MovieScheduleForCustomerDTO> GetMoviesScheduled(MovieFilterForCustomerDTO ff);
        CrudCinemaEnum Insert(CustomerForInsertDTO cus);
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

    }
}
