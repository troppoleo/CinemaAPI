using CinemaDAL.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBL.Repository
{
    public interface IRepository<T> where T : class, new()
    {
        void Add(T item);
        void AddRange(IEnumerable<T> items);
        void Delete(T item);
        void DeleteRange(IEnumerable<T> items);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        T Find(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAllWhere(Expression<Func<T, bool>> expr);
        void Update(T item);
        void UpdateRange(IEnumerable<T> items);
    }

    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private readonly CinemaContext _ctx;

        public Repository(CinemaContext ctx)
        {
            _ctx = ctx;
        }

        //public async Task<T> Find(Expression<Func<T, bool>> predicate) => await _ctx.Set<T>().FindAsync(predicate);
        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            //var x = _ctx.Set<T>().AsQueryable();
            //return await x.FirstOrDefaultAsync(predicate);
            var x = await _ctx.Set<T>().FirstOrDefaultAsync(predicate);
            return x;
        }

        public T Find(Expression<Func<T, bool>> predicate)
        {            
            var x = _ctx.Set<T>().FirstOrDefault(predicate);
            return x;
        }

        public async Task<IEnumerable<T>> GetAll() => await _ctx.Set<T>().ToListAsync();

        public async Task<IEnumerable<T>> GetAllWhere(Expression<Func<T, bool>> expr) => await _ctx.Set<T>().Where(expr).ToListAsync();

        public void Add(T item)
        {
            _ctx.Set<T>().Add(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            _ctx.Set<T>().AddRange(items);
        }

        public void Update(T item)
        {
            _ctx.Set<T>().Update(item);
        }

        public void UpdateRange(IEnumerable<T> items)
        {
            _ctx.Set<T>().UpdateRange(items);
        }

        public void Delete(T item)
        {
            _ctx.Set<T>().Remove(item);
        }
        public void DeleteRange(IEnumerable<T> items)
        {
            _ctx.Set<T>().RemoveRange(items);
        }
    }

}
