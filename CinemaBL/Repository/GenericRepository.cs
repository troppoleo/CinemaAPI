using CinemaBL.Extension;
using CinemaBL.Paging;
using CinemaDAL.Models;
using CinemaDTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBL.Repository
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal CinemaContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(CinemaContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        #region "metodi CRUD"
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity? GetByID(object id)
        {
            return dbSet.Find(id);
        }


        /// <summary>
        /// Aggiunto per gestire i null
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool GetByID(object id, out TEntity entity)
        {
            entity = GetByID(id);
            if (entity == null)
                return false;
            return true;
        }


        public virtual bool Exists(Expression<Func<TEntity, bool>> filter = null)
        {
            return Get(filter).Any();
        }

        public virtual bool NotExists(Expression<Func<TEntity, bool>> filter = null)
        {
            return !Exists(filter);
        }


        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        #endregion
    }

    public class CinemaRoomRep : GenericRepository<CinemaRoom>
    {
        public CinemaRoomRep(CinemaContext context) : base(context) { }



        public bool GetByIdAndFillMovieSchedule(int id, out CinemaRoom cr)
        {
            //            var x = context.UserEmployees.Include(x => x.JobQualification).FirstOrDefault(x => x.Id == userEmployeeId);

            cr = context.CinemaRooms.Include(x => x.MovieSchedules).FirstOrDefault(x => x.Id == id);
            if (cr == null)
            {
                return false;
            }

            return true;

        }
    }

    public class CinemaRoomCrossUserEmployeeRep : GenericRepository<CinemaRoomCrossUserEmployee>
    {
        public CinemaRoomCrossUserEmployeeRep(CinemaContext context) : base(context) { }
    }

    public class CustomerRep : GenericRepository<Customer>
    {
        public CustomerRep(CinemaContext context) : base(context) { }
    }
    public class CustomerCrossMovieScheduleRep : GenericRepository<CustomerCrossMovieSchedule>
    {
        public CustomerCrossMovieScheduleRep(CinemaContext context) : base(context) { }
    }

    public class JobEmployeeQualificationRep : GenericRepository<JobEmployeeQualification>
    {
        public JobEmployeeQualificationRep(CinemaContext context) : base(context) { }
    }
    public class MovieRep : GenericRepository<Movie>
    {
        public MovieRep(CinemaContext context) : base(context) { }


    }
    public class MovieScheduleRep : GenericRepository<MovieSchedule>
    {
        public MovieScheduleRep(CinemaContext context) : base(context) { }
    }

    public class UsersAdminRep : GenericRepository<UsersAdmin>
    {
        public UsersAdminRep(CinemaContext context) : base(context) { }
    }
    public class UserTypeRep : GenericRepository<UserType>
    {
        public UserTypeRep(CinemaContext context) : base(context) { }
    }


    public class TicketRep : GenericRepository<Ticket>
    {
        public TicketRep(CinemaContext context) : base(context) { }

        //internal IEnumerable<RateReviewDTO> GetPage(PaginationFilter pf)
        //{
        //    //context.Tickets.Include(x=> x.MovieSchedule)
        //    return null;
        //}
    }

    public class ViewReviewRep : GenericRepository<ViewReview>
    {
        public ViewReviewRep(CinemaContext context) : base(context) { }

        internal IEnumerable<ViewReview> GetPage(PaginationFilter pf)
        {
            if (string.IsNullOrEmpty(pf.FilmName))
            {
                return Get().Skip((pf.PageNumber - 1) * pf.PageSize).Take(pf.PageSize);
            }

            return Get(x => x.FilmName.ToLower() == pf.FilmName)
                .Skip((pf.PageNumber - 1) * pf.PageSize).Take(pf.PageSize);
        }
    }


    public class WeekCalendarRep : GenericRepository<WeekCalendar>
    {
        public WeekCalendarRep(CinemaContext context) : base(context) { }

        public DateTime OpeningInTheDateChoised(DateTime dayChoised)
        {
            var x = context.WeekCalendars.Find((int)dayChoised.DayOfWeek);
            DateTime dtOperturaDelGiornoScelto = new DateTime(dayChoised.Year, dayChoised.Month, dayChoised.Day).Add(x.StartTime);

            return dtOperturaDelGiornoScelto;
        }

    }

    public class UserEmployeeRep : GenericRepository<UserEmployee>
    {
        public UserEmployeeRep(CinemaContext context) : base(context) { }

        public void AddMinimal(UserEmployeeMinimalDTO emp)
        {
            UserEmployee ue = new UserEmployee()
            {
                UserName = emp.UserName,
                Password = emp.Password,
                JobQualificationId = emp.JobQualificationId
            };

            dbSet.Add(ue);
        }



        /// <summary>
        /// verifica se si tratta di un resposnsabile di sala
        /// </summary>
        /// <param name="userEmployeeId"></param>
        /// <returns></returns>
        public bool IsOwnSala(int userEmployeeId)
        {
            // CARICAMENTO della relazione:
            var x = context.UserEmployees.Include(x => x.JobQualification).FirstOrDefault(x => x.Id == userEmployeeId);
            if (x is null)
            {
                return false;
            }

            //switch (x.JobQualification.ShortDescr.ToEnum<Enums.JobEmployeeQualificationEnum>())
            //{
            //    case Enums.JobEmployeeQualificationEnum.OWN_SALA:
            //        return true;
            //    default:
            //        return false;
            //}

            // soluzione proposta da VS ma incomprensibile:
            return x.JobQualification.ShortDescr.ToEnum<Enums.JobEmployeeQualificationEnum>() switch
            {
                Enums.JobEmployeeQualificationEnum.OWN_SALA => true,
                _ => false,
            };

            //var result = x.JobQualification.ShortDescr.ToEnum<Enums.JobEmployeeQualificationEnum>() == Enums.JobEmployeeQualificationEnum.OWN_SALA;
            //return result;
        }


    }

}
