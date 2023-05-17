using CinemaBL.Enums;
using CinemaDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Tasks
{
    public class ServiceLiberaOWN_SALA : BackgroundService
    {
        private readonly TimeSpan _period = TimeSpan.FromSeconds(5);

        private IConfiguration _conf;
        private readonly IServiceScopeFactory _ssf;

        public ServiceLiberaOWN_SALA(IServiceScopeFactory ssf, IConfiguration conf)
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

            // individua le schedulazioni a DONE 
            foreach (var cinemaRoomId in ctx.MovieSchedules
                .Where(x => x.Status == MovieScheduleEnum.DONE.ToString())
                .Select(x => x.CinemaRoomId).ToList())
            {
                foreach (var uf in ctx.CinemaRoomCrossUserEmployees.Where(x => x.CinemaRoomId == cinemaRoomId).ToList())
                {
                    uf.UserEmployeeId = null;
                    ctx.CinemaRoomCrossUserEmployees.Update(uf);
                }
            }



            // questo va in errore perchè il primo loop non chiude il cursore (manca .toList()
            //foreach (var cinemaRoomId in ctx.MovieSchedules
            //    .Where(x => x.Status == MovieScheduleEnum.DONE.ToString())
            //    .Select(x => x.CinemaRoomId))
            //{
            //    foreach (var uf in ctx.CinemaRoomCrossUserEmployees.Where(x => x.CinemaRoomId == cinemaRoomId))
            //    {
            //        uf.UserEmployeeId = null;
            //        ctx.CinemaRoomCrossUserEmployees.Update(uf);
            //    }
            //}

            ctx.SaveChanges();
        }

    }
}
