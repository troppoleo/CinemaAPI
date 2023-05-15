using CinemaAPI.Hubs;
using CinemaDAL.Models;
using CinemaDTO;
using Microsoft.AspNetCore.SignalR;

namespace CinemaAPI.Tasks
{
    public class ServicePushBySignalRSeatRoom : BackgroundService
    {
        private readonly TimeSpan _period = TimeSpan.FromSeconds(5);
        private readonly IHubContext<CinemaHub> _hub;

        public ServicePushBySignalRSeatRoom(IHubContext<CinemaHub> hub)
        {
            _hub = hub;
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

            //await _hub.Clients.Groups("3002").SendAsync("ReceiveSeatRoom", new SeatRoomDTO()
            //{
            //    cinemaRoomId = 1,
            //    stdSeatBusy = 10,
            //    stdSeatFree = 10,
            //    TotalSeat = 111,
            //    vipSeatBusy = 10,
            //    vipSeatFree = 11
            //});

            //await _hub.Clients.All.SendAsync("ReceiveSeatRoom", new SeatRoomDTO()
            //{
            //    cinemaRoomId = 1,
            //    stdSeatBusy = 10,
            //    stdSeatFree = 10,
            //    TotalSeat = 111,
            //    vipSeatBusy = 10,
            //    vipSeatFree = 11
            //});
        }
    }
}
