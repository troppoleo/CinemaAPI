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
        private readonly CinemaContext _context;
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
            var scheduledMoveToCheck = ctx.MovieSchedules.Include(x => x.Movie).Where(x => x.Status != CinemaBL.Enums.MovieScheduleEnum.DONE.ToString());
            foreach (var sm in scheduledMoveToCheck)
            {                
                if (CheckIfMovieIsDone(sm))
                {
                    sm.Status = CinemaBL.Enums.MovieScheduleEnum.DONE.ToString();
                    ctx.MovieSchedules.Update(sm);
                }
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
            //int duration = ctx.Movies.Where(x => x.Id == sm.MovieId).FirstOrDefault().Duration.Value;
            int duration = sm.Movie.Duration.Value;
            DateTime dtEndMovie = sm.StartDate.Value.AddMinutes(duration).AddMinutes(int.Parse(_conf["Generic:CleaninigTime"]));

            if (dtEndMovie >= DateTime.Now)
            {
                // film ancora da proiettare o cmq non pronto per lo stato "DONE"
                return false;
            }
            return true;
        }
    }
}
