using CinemaDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBL.Repository
{
    public interface IMovieRepository
    {
        
    }
    
    public class MovieRepository : Repository<Movie>
    {
        public MovieRepository(CinemaContext ctx) : base(ctx)
        {
        }
    }



}
