using CinemaDAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBL.Repository
{
    public interface IRepository<T> where T : class, new()
    {
        Task<IEnumerable<T>> GetAll();
    }

    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private readonly CinemaContext _ctx;

        public Repository(CinemaContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<T>> GetAll() => await _ctx.Set<T>().ToListAsync();
    }

}
