using CinemaBL.Enums;
using CinemaBL.Extension;
using CinemaBL.Repository;
using CinemaDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaAPI.Tasks
{
    public class ServiceSetStatusToDONE : BackgroundService
    {
        private readonly TimeSpan _period = TimeSpan.FromSeconds(5);
        //private IUnitOfWorkGeneric _uow;

        private IConfiguration _conf;
        private int _timeToClean;
        private readonly IServiceScopeFactory _ssf;

        public ServiceSetStatusToDONE(IServiceScopeFactory ssf, IConfiguration conf)
        {
            _ssf = ssf;
            _conf = conf;
            _timeToClean = int.Parse(_conf["Generic:CleaninigTime"]);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new(_period);
            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                await DoWorkAsync();
            }
        }

        private async Task DoWorkAsync()
        {
            using var scope = _ssf.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<CinemaContext>();

            // now do your work

            var movSched = ctx.MovieSchedules.Include(x => x.Movie).Include(x => x.Tickets)
                .Where(x => x.IsApproved == 1 
                && x.Status != MovieScheduleEnum.DONE.ToString()
                && x.Status != MovieScheduleEnum.CANCELLED.ToString()).ToList();

            foreach (var sm in movSched)
            {
                var originalStatus = sm.Status;

                if (CheckIfMovieIsDone(sm))
                {
                    sm.Status = MovieScheduleEnum.DONE.ToString();
                }
                else if (CheckIfMovieIsInCleaning(sm))
                {
                    sm.Status = MovieScheduleEnum.CLEAN_TIME.ToString();
                }
                else if (CheckIsMovieInProgress(sm))
                {
                    sm.Status = MovieScheduleEnum.IN_PROGRESS.ToString();
                }

                switch (sm.Status.ToEnum<MovieScheduleEnum>())
                {
                    case MovieScheduleEnum.IN_PROGRESS:
                    case MovieScheduleEnum.DONE:
                    case MovieScheduleEnum.CLEAN_TIME:
                        if (sm.Tickets.Count() == 0)
                        {
                            sm.Status = MovieScheduleEnum.CANCELLED.ToString();
                            sm.IsApproved = 0;
                        }
                        break;
                }

                // verifico se ho cambiato lo stato
                if (originalStatus != sm.Status)
                {
                    ctx.MovieSchedules.Update(sm);
                }
            }

            //// imposta i film a DONE o CLEAN_TIME
            //var scheduledMoveToCheck = ctx.MovieSchedules.Include(x => x.Movie).Where(x => x.IsApproved == 1 &&
            // x.Status != MovieScheduleEnum.DONE.ToString()).ToList();
            //foreach (var sm in scheduledMoveToCheck)
            //{
            //    if (CheckIfMovieIsDone(sm))
            //    {
            //        sm.Status = MovieScheduleEnum.DONE.ToString();
            //        ctx.MovieSchedules.Update(sm);
            //    }
            //    else if (CheckIfMovieIsInCleaning(sm))
            //    {
            //        sm.Status = MovieScheduleEnum.CLEAN_TIME.ToString();
            //        ctx.MovieSchedules.Update(sm);
            //    }
            //}


            // annulla i film che non hanno venduto biglietti, lo capisco dal fatto che la tabella Ticket non ha biglietti associati
            //var moviesToCancel = ctx.MovieSchedules.Include(x => x.Tickets).Where(x => x.IsApproved == 1
            //    && x.Tickets.Where(t => t.MovieScheduleId == t.MovieScheduleId).Count() == 0).ToList();
            //var moviesToCancel = ctx.MovieSchedules.Include(x => x.Tickets).Where(x => x.IsApproved == 1 && x.Tickets.Count() == 0).ToList();
            //foreach (var sm in moviesToCancel)
            //{
            //    sm.Status = MovieScheduleEnum.CANCELLED.ToString();
            //    sm.IsApproved = 0;
            //    ctx.MovieSchedules.Update(sm);

            //}

            //var moviesInProgress = ctx.MovieSchedules.Include(x => x.Movie).Where(x => x.IsApproved == 1).ToList();
            //foreach (var ms in moviesInProgress)
            //{
            //    if (CheckIsMovieInProgress(ms))
            //    {
            //        ms.Status = MovieScheduleEnum.IN_PROGRESS.ToString();
            //        ctx.MovieSchedules.Update(ms);
            //    }
            //}

            ctx.SaveChanges();
        }

        private bool CheckIsMovieInProgress(MovieSchedule ms)
        {
            int duration = ms.Movie.Duration;
            var dtNow = DateTime.Now;
            DateTime dtEndMovie = ms.StartDate.AddMinutes(duration);
            //DateTime dtCleanTime = dtEndMovie.AddMinutes(duration).AddMinutes(_timeToClean);

            if (ms.StartDate <= dtNow && dtNow <= dtEndMovie)
            {
                // film in proiezione
                return true;
            }
            return false;
        }


        /// <summary>
        /// verifica se il film è finito, inclusa la parte di pulizie
        /// </summary>
        /// <param name="sm"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private bool CheckIfMovieIsDone(MovieSchedule sm)
        {
            int duration = sm.Movie.Duration;
            DateTime dtEndMovie = sm.StartDate.AddMinutes(duration).AddMinutes(_timeToClean);

            if (dtEndMovie >= DateTime.Now)
            {
                // film ancora da proiettare o cmq non pronto per lo stato "DONE"
                return false;
            }
            return true;
        }

        private bool CheckIfMovieIsInCleaning(MovieSchedule sm)
        {
            int duration = sm.Movie.Duration;
            DateTime dtEndMovie = sm.StartDate.AddMinutes(duration);
            DateTime dtEndMovieAndCleaning = sm.StartDate.AddMinutes(duration).AddMinutes(_timeToClean);
            DateTime dtNow = DateTime.Now;

            if (dtNow >= dtEndMovie && dtNow <= dtEndMovieAndCleaning)
            {
                // film è finito e la sala è in fase di pulizia
                return true;
            }
            return false;
        }
    }
}
