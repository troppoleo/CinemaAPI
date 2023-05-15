using CinemaBL.Extension;
using CinemaBL.Repository;
using CinemaDAL.Models;
using CinemaDTO;
using Microsoft.AspNetCore.SignalR;

namespace CinemaAPI.Hubs
{
    public interface INotify
    {
        void SendMessageToOwn_SALA(int userEmployeeId);
    }

    public class Notify : INotify
    {
        private readonly IHubContext<CinemaHub> _hub;
        private readonly IUnitOfWorkGeneric _uow;

        public Notify(IHubContext<CinemaHub> hub, IUnitOfWorkGeneric uow)
        {
            _hub = hub;
            _uow = uow;
        }

        public void SendMessageToOwn_SALA(int userEmployeeId)
        {
            var emplCross = _uow.GetCinemaRoomCrossUserEmployeeRep.Get(x => x.UserEmployeeId == userEmployeeId).FirstOrDefault();
            if (emplCross != null)
            {
                var movieSchedule = _uow.GetMovieScheduleRep.Get(x => x.CinemaRoomId == emplCross.CinemaRoomId, includeProperties: nameof(CinemaRoom)).ToList();
                foreach (var movieScheduleItem in movieSchedule)
                {
                    _hub.Clients.Group(userEmployeeId.ToString()).SendAsync("ReceiveSeatRoom", new SeatRoomDTO()
                    {
                        cinemaRoomId = movieScheduleItem.CinemaRoomId,
                        stdSeatBusy = movieScheduleItem.StdSeat.ToDefault(),
                        stdSeatFree = movieScheduleItem.CinemaRoom.MaxStdSeat.ToDefault() - movieScheduleItem.StdSeat.ToDefault(),
                        TotalSeat = movieScheduleItem.CinemaRoom.MaxStdSeat.ToDefault() + movieScheduleItem.CinemaRoom.MaxVipSeat.ToDefault(),
                        vipSeatBusy = movieScheduleItem.VipSeat.ToDefault(),
                        vipSeatFree = movieScheduleItem.CinemaRoom.MaxVipSeat.ToDefault() - movieScheduleItem.VipSeat.ToDefault()
                    });
                }

            }

        }
    }
}
