using CinemaBL.Enums;
using CinemaBL.Repository;
using CinemaDAL.Models;
using CinemaDTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CinemaBL.MovieService;
using CinemaBL.Extension;

namespace CinemaBL
{
    public interface IMovieService
    {
        //MovieServiceEnum CreateMovie(MovieForAddDTO movie);
        IEnumerable<MovieDTO> GetAll();
        MovieDTO? GetById(int id);
        CrudCinemaEnum Delete(int idMovie);
        CrudCinemaEnum Insert(MovieForAddDTO movie);
        CrudCinemaEnum Update(MovieDTO movie);
    }



    public class MovieService : IMovieService
    {
        private readonly IUnitOfWorkGeneric _uow;

        public MovieService(IUnitOfWorkGeneric uow)
        {
            _uow = uow;
        }

        public IEnumerable<MovieDTO> GetAll()
        {
            var x = _uow.GetMovieRep.Get();

            return x.Select(x =>
                new MovieDTO()
                {
                    Actors = x.Actors.ToDefault(),
                    Cover = x.Cover.ToDefault(),
                    Director = x.Director.ToDefault(),
                    Duration = x.Duration,
                    FilmName = x.FilmName.ToDefault(),
                    Genere = x.Genere.ToDefault(),
                    ID = x.Id,
                    MoviePlot = x.MoviePlot.ToDefault(),
                    ProductionYear = x.ProductionYear.ToDefault(),
                    Trama = x.Trama.ToDefault()
                });
        }


        public CrudCinemaEnum Insert(MovieForAddDTO movie)
        {
            if (!_uow.GetMovieRep.Get(x => x.FilmName == movie.FilmName).Any())
            {
                _uow.GetMovieRep.Insert(new Movie()
                {
                    Actors = movie.Actors,
                    Cover = movie.Cover,
                    Director = movie.Director,
                    Duration = movie.Duration,
                    FilmName = movie.FilmName,
                    Genere = movie.Genere,
                    MoviePlot = movie.MoviePlot,
                    ProductionYear = movie.ProductionYear,
                    Trama = movie.Trama
                });

                return CrudCinemaEnum.CREATED;
            }

            return CrudCinemaEnum.ALREADY_EXISTS;
        }


        //public MovieServiceEnum CreateMovie(MovieForAddDTO movie)
        //{
        //    try
        //    {
        //        var mv = _uow.GetMovieRep.Get(x => x.FilmName == movie.FilmName);
        //        //var mv = await _uow.MovieRepository.FindAsync(x => x.FilmName == movie.FilmName);
        //        if (mv == null)
        //        {
        //            _uow.GetMovieRep.Insert(new Movie()
        //            {
        //                Actors = movie.Actors,
        //                Cover = movie.Cover,
        //                Director = movie.Director,
        //                Duration = movie.Duration,
        //                FilmName = movie.FilmName,
        //                Genere = movie.Genere,
        //                MoviePlot = movie.MoviePlot,
        //                ProductionYear = movie.ProductionYear,
        //                Trama = movie.Trama
        //            });

        //            return MovieServiceEnum.CREATED;
        //        }
        //        else
        //        {
        //            return MovieServiceEnum.ALREADY_EXISTS;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //    }

        //    return MovieServiceEnum.CREATED;
        //}
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

        public CrudCinemaEnum Update(MovieDTO movie)
        {
            var mv = _uow.GetMovieRep.GetByID(movie.ID);
            if (mv is null)
            {
                return CrudCinemaEnum.NOT_FOUND;
            }

            mv.Actors = movie.Actors;
            mv.Cover = movie.Cover;
            mv.Director = movie.Director;
            mv.Duration = movie.Duration;
            mv.FilmName = movie.FilmName;
            mv.Genere = movie.Genere;
            mv.MoviePlot = movie.MoviePlot;
            mv.ProductionYear = movie.ProductionYear;
            mv.Trama = movie.Trama;

            _uow.GetMovieRep.Update(mv);
            return CrudCinemaEnum.UPDATED;
        }

        public CrudCinemaEnum Delete(int idMovie)
        {
            var mv = _uow.GetMovieRep.GetByID(idMovie);
            if (mv is null)
            {
                return CrudCinemaEnum.NOT_FOUND;
            }

            _uow.GetMovieRep.Delete(mv);
            return CrudCinemaEnum.DELETED;
        }

        public MovieDTO? GetById(int id)
        {
            var mv = _uow.GetMovieRep.GetByID(id);
            if (mv is null)
            {
                return null;
            }

            return new MovieDTO()
            {
                Actors = mv.Actors.ToDefault(),
                Cover = mv.Cover.ToDefault(),
                Director = mv.Director.ToDefault(),
                Duration = mv.Duration,
                FilmName = mv.FilmName.ToDefault(),
                Genere = mv.Genere.ToDefault(),
                ID = mv.Id,
                MoviePlot = mv.MoviePlot.ToDefault(),
                ProductionYear = mv.ProductionYear.ToDefault(),
                Trama = mv.Trama.ToDefault()
            };

        }
    }
}
