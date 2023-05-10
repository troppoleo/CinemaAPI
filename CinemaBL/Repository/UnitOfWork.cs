using CinemaDAL.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBL.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Movie> MovieRepository { get; }
        IRepository<CinemaRoom> CinemaRoomRep { get; }

        //IRepository<UsersEmployee> UsersEmployeeRep { get; }
        IUsersEmployeeRepository<UserEmployee> UsersEmployeeRep { get; }

        IDbContextTransaction BeginTransaction();
        Task<bool> SaveChangesAsync();
        bool HasChanges();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly CinemaContext _ctx;


        public UnitOfWork(CinemaContext ctx)
        {
            _ctx = ctx;
        }

        private MovieRepository movieRepository;
        public IRepository<Movie> MovieRepository => movieRepository ??= new MovieRepository(_ctx);
        // questo è equivalente:
        //public IRepository<Movie> MovieRepository
        //{
        //    get
        //    {
        //        movieRepository = new MovieRepository(_ctx);
        //        return movieRepository;
        //    }
        //}

        private CinemaRoomRepository roomRepository;
        public IRepository<CinemaRoom> CinemaRoomRep => roomRepository ??= new CinemaRoomRepository(_ctx);


        private IUsersEmployeeRepository<UserEmployee> usersEmployeeRepository;        
        public IUsersEmployeeRepository<UserEmployee> UsersEmployeeRep
        {
            get
            {
                if (usersEmployeeRepository == null)
                {
                    usersEmployeeRepository = new UsersEmployeeRepository(_ctx);
                }
                return usersEmployeeRepository;
            }
        }

        //private IUsersEmployeeRepository userEmployeeRepository;
        //public IUsersEmployeeRepository<UsersEmployee> UsersEmployeeRep => userEmployeeRepository ??= new UsersEmployeeRepository(_ctx);



        public async Task<bool> SaveChangesAsync()
        {
            return await _ctx.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            _ctx.Dispose();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _ctx.Database.BeginTransaction();
        }

        public bool HasChanges()
        {
            return _ctx.ChangeTracker.HasChanges();
        }
    }
}
