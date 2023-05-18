using CinemaDAL.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBL.Repository
{
    public interface IUnitOfWorkGeneric
    {
        GenericRepository<UserEmployee> UserEmployeeRepository { get; }
        UserEmployeeRep GetUserEmployeeRep { get; }
        CinemaRoomRep GetCinemaRoomRep { get; }
        CinemaRoomCrossUserEmployeeRep GetCinemaRoomCrossUserEmployeeRep { get; }
        CustomerRep GetCustomerRep { get; }
        CustomerCrossMovieScheduleRep GetCustomerCrossMovieScheduleRep { get; }
        JobEmployeeQualificationRep GetJobEmployeeQualificationRep { get; }
        MovieRep GetMovieRep { get; }
        MovieScheduleRep GetMovieScheduleRep { get; }
        UsersAdminRep GetUsersAdminRep { get; }
        UserTypeRep GetUserTypeRep { get; }
        WeekCalendarRep GetWeekCalendarRep { get; }
        TicketRep GetTicketRep { get; }
        ViewReviewRep GetViewReviewRep { get; }
        PriceTicketDefaultRep GetPriceTicketDefaultRep { get; }
        MovieRateRep GetMovieRateRep { get; }
        ViewCustomerMovieWatchedRep GetViewCustomerMovieWatchedRep { get; }

        void Dispose();
        void Save();
    }

    public class UnitOfWorkGeneric : IDisposable, IUnitOfWorkGeneric
    {
        private CinemaContext context = new CinemaContext();
        private GenericRepository<UserEmployee> userEmployeeRepository;
        private UserEmployeeRep userEmployeeExt;
        private CinemaRoomRep cinemaRoom;
        private CinemaRoomCrossUserEmployeeRep cinemaRoomCrossUserEmployee;
        private CustomerRep customerRep;
        private CustomerCrossMovieScheduleRep customerCrossMovieScheduleRep;
        private JobEmployeeQualificationRep jobEmployeeQualificationRep;
        private MovieRep movieRep;
        private MovieScheduleRep movieScheduleRep;
        private UsersAdminRep usersAdminRep;
        private UserTypeRep userTypeRep;
        private WeekCalendarRep weekCalendarRep;
        private TicketRep ticketRep;
        private ViewReviewRep viewReviewRep;
        private PriceTicketDefaultRep priceTicketDefaultRep;
        private MovieRateRep movieRateRep;
        private ViewCustomerMovieWatchedRep viewCustomerMovieWatchedRep;

        public CinemaRoomRep GetCinemaRoomRep => cinemaRoom ??= new CinemaRoomRep(context);
        public CinemaRoomCrossUserEmployeeRep GetCinemaRoomCrossUserEmployeeRep => cinemaRoomCrossUserEmployee ??= new CinemaRoomCrossUserEmployeeRep(context);
        public CustomerRep GetCustomerRep => customerRep ??= new CustomerRep(context);
        public CustomerCrossMovieScheduleRep GetCustomerCrossMovieScheduleRep => customerCrossMovieScheduleRep ??= new CustomerCrossMovieScheduleRep(context);
        public JobEmployeeQualificationRep GetJobEmployeeQualificationRep => jobEmployeeQualificationRep ??= new JobEmployeeQualificationRep(context);
        public MovieRep GetMovieRep => movieRep ??= new MovieRep(context);
        public MovieScheduleRep GetMovieScheduleRep => movieScheduleRep ??= new MovieScheduleRep(context);
        public UsersAdminRep GetUsersAdminRep => usersAdminRep ??= new UsersAdminRep(context);
        public UserTypeRep GetUserTypeRep => userTypeRep ??= new UserTypeRep(context);
        public WeekCalendarRep GetWeekCalendarRep => weekCalendarRep ??= new WeekCalendarRep(context);
        public TicketRep GetTicketRep => ticketRep ??= new TicketRep(context);
        public ViewReviewRep GetViewReviewRep => viewReviewRep ??= new ViewReviewRep(context);
        public PriceTicketDefaultRep GetPriceTicketDefaultRep => priceTicketDefaultRep??= new PriceTicketDefaultRep(context);
        public MovieRateRep GetMovieRateRep => movieRateRep ??= new MovieRateRep(context);
        public ViewCustomerMovieWatchedRep GetViewCustomerMovieWatchedRep => viewCustomerMovieWatchedRep ??= new ViewCustomerMovieWatchedRep(context);

        public UserEmployeeRep GetUserEmployeeRep
        {
            get
            {
                if (this.userEmployeeExt == null)
                {
                    this.userEmployeeExt = new UserEmployeeRep(context);
                }
                return userEmployeeExt;
            }
        }

        public GenericRepository<UserEmployee> UserEmployeeRepository
        {
            get
            {
                if (this.userEmployeeRepository == null)
                {
                    this.userEmployeeRepository = new GenericRepository<UserEmployee>(context);
                }
                return userEmployeeRepository;
            }
        }



        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;


        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
