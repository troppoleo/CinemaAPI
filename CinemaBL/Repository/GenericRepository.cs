using CinemaDAL.Models;
using CinemaDTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
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

    public class CinemaRoomRep: GenericRepository<CinemaRoom>
    {
        public CinemaRoomRep(CinemaContext context) : base(context) { }
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
    public class WeekCalendarRep : GenericRepository<WeekCalendar>
    {
        public WeekCalendarRep(CinemaContext context) : base(context) { }
    }

    public class UserEmployeeRep : GenericRepository<UserEmployee>
    {
        public UserEmployeeRep(CinemaContext context) : base(context) { }

        public void AddMinimal(UsersEmployeeMinimalDTO emp)
        {
            UserEmployee ue = new UserEmployee()
            {
                UserName = emp.UserName,
                Password = emp.Password,
                JobQualificationId = emp.JobQualificationId
            };

            dbSet.Add(ue);
        }

    }

}
