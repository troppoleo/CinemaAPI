using CinemaBL.Enums;
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
        private readonly IServiceScopeFactory _ssf;

        public ServiceSetStatusToDONE(IServiceScopeFactory ssf, IConfiguration conf)
        {
            _ssf = ssf;
            _conf = conf;
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

            // imposta i film a DONE
            var scheduledMoveToCheck = ctx.MovieSchedules.Include(x => x.Movie).Where(x => x.IsApproved == 1 &&
                x.Status != MovieScheduleEnum.DONE.ToString());
            foreach (var sm in scheduledMoveToCheck)
            {
                if (CheckIfMovieIsDone(sm))
                {
                    sm.Status = MovieScheduleEnum.DONE.ToString();
                    ctx.MovieSchedules.Update(sm);
                }
            }

            // TODO: IMPOSTARE status a: (mi serve per la generazione dei biglietti)
            //MovieScheduleEnum.IN_PROGRESS
            //MovieScheduleEnum.CLEAN_TIME
            //MovieScheduleEnum.IN_PROGRESS

            // annulla i film che non hanno venduto biglietti
            var moviesToCancel = ctx.MovieSchedules.Where(x => x.IsApproved == 1 && x.VipSeat == 0 && x.StdSeat == 0);
            foreach (var sm in moviesToCancel)
            {
                sm.Status = MovieScheduleEnum.CANCELLED.ToString();
                sm.IsApproved = 0;
                ctx.MovieSchedules.Update(sm);
            }

            ctx.SaveChanges();
        }


        /// <summary>
        /// verifica se il film è finito
        /// </summary>
        /// <param name="sm"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private bool CheckIfMovieIsDone(MovieSchedule sm)
        {
            int duration = sm.Movie.Duration;
            DateTime dtEndMovie = sm.StartDate.AddMinutes(duration).AddMinutes(int.Parse(_conf["Generic:CleaninigTime"]));

            if (dtEndMovie >= DateTime.Now)
            {
                // film ancora da proiettare o cmq non pronto per lo stato "DONE"
                return false;
            }
            return true;
        }
    }
}
