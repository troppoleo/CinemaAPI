using CinemaBL.Enums;
using CinemaBL.Extension;
using CinemaBL.Repository;
using CinemaDAL.Models;
using CinemaDTO;
using Microsoft.AspNetCore.SignalR;

namespace CinemaAPI.Hubs
{
    public interface INotify
    {
        void SendMessageToGET_TICKET(int idMovieSchedule);
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

        public void SendMessageToGET_TICKET(int idMovieSchedule = -1)
        {
            var bigliettai = _uow.GetUserEmployeeRep.Get(x=> x.JobQualificationId == (int)JobEmployeeQualificationEnum.GET_TICKET).ToList();
            foreach (var bi in bigliettai)
            {
                MovieSchedule ms;
                if (idMovieSchedule > 0)
                {
                    ms = _uow.GetMovieScheduleRep.Get(x => x.Id == idMovieSchedule && x.StartDate >= DateTime.Now,
                        includeProperties: $"{nameof(Movie)},{nameof(CinemaRoom)}").First();
                }
                else
                {
                    ms = _uow.GetMovieScheduleRep.Get(x => x.StartDate >= DateTime.Now,
                        includeProperties: $"{nameof(Movie)},{nameof(CinemaRoom)}").First();
                }
                _hub.Clients.Group(bi.Id.ToString()).SendAsync("ReceiveMovieScheduleIsApproved", new
                {
                    FilmName = ms.Movie.FilmName,
                    RoomName = ms.CinemaRoom.RoomName,
                    StartDate = ms.StartDate
                });
            }
            
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
