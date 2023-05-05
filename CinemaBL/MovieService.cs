using CinemaBL.Repository;
using CinemaDAL.Models;
using CinemaDTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CinemaBL.MovieService;

namespace CinemaBL
{
    public interface IMovieService
    {
      //  MovieServiceEnum CreateMovie(MovieDTO movie);
        Task<IEnumerable<MovieDTO>> GetAllMovie();
        //MovieServiceEnum UpdateMovie(MovieDTO movie);
    }

    //public interface IMovieService
    //{
    //    MovieServiceEnum CreateMovie(MovieDTO movie);

    //    IEnumerable<MovieDTO> GetAllMovie();
    //    MovieServiceEnum UpdateMovie(MovieDTO movie);
    //}

    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _uow;

        public MovieService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public enum MovieServiceEnum
        {
            CREATED,
            DELETED,
            INSERTED,
            UPDATED,
            ALREADY_EXISTS,
            NONE,
            NOT_FOUND,
            /// <summary>
            /// violazione di un requisito minimo
            /// </summary>
            VIOLATION_MINIMUM_REQUIRED
        }

        public async Task<IEnumerable<MovieDTO>> GetAllMovie()
        {
            var x = await _uow.MovieRepository.GetAll();

            return x.Select(x =>
                new MovieDTO()
                {
                    Actors = x.Actors,
                    Cover = x.Cover,
                    Director = x.Director,
                    Duration = x.Duration.Value,
                    FilmName = x.FilmName,
                    Genere = x.Genere,
                    ID = x.Id,
                    MoviePlot = x.MoviePlot,
                    ProductionYear = x.ProductionYear.Value,
                    Trama = x.Trama
                });
        }

        //public MovieServiceEnum CreateMovie(MovieDTO movie)
        //{
        //    if (_uow.Movies.Where(x => x.FilmName == movie.FilmName).Any())
        //    {
        //        return MovieServiceEnum.ALREADY_EXISTS;
        //    }

        //    _uow.Movies.Add(new Movie()
        //    {
        //        Actors = movie.Actors,
        //        Cover = movie.Cover,
        //        Director = movie.Director,
        //        Duration = movie.Duration,
        //        FilmName = movie.FilmName,
        //        Genere = movie.Genere,
        //        MoviePlot = movie.MoviePlot,
        //        ProductionYear = movie.ProductionYear,
        //        Trama = movie.Trama
        //    });

        //    return MovieServiceEnum.CREATED;
        //}

        //public MovieServiceEnum UpdateMovie(MovieDTO movie)
        //{
        //    var mv = _uow.Movies.Where(x => x.Id == movie.ID).FirstOrDefault();
        //    if (mv is null)
        //    {
        //        return MovieServiceEnum.NOT_FOUND;
        //    }

        //    mv.Actors = movie.Actors;
        //    mv.Cover = movie.Cover;
        //    mv.Director = movie.Director;
        //    mv.Duration = movie.Duration;
        //    mv.FilmName = movie.FilmName;
        //    mv.Genere = movie.Genere;
        //    mv.MoviePlot = movie.MoviePlot;
        //    mv.ProductionYear = movie.ProductionYear;
        //    mv.Trama = movie.Trama;

        //    _uow.Entry(mv).State = EntityState.Modified;
        //    return MovieServiceEnum.UPDATED;
        //}
    }
}
