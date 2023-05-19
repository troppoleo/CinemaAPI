using CinemaBL.Enums;
using CinemaBL.Extension;
using CinemaBL.Repository;
using CinemaDAL.Models;
using CinemaDTO;
using Microsoft.AspNetCore.SignalR;
using System.Runtime.InteropServices.ComTypes;
using static CinemaDTO.UserModelDTO;

namespace CinemaAPI.Hubs
{
    public interface INotify
    {
        void SendMessageToAdmin();
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

        public void SendMessageToAdmin()
        {
            //   var ms = _uow.GetMovieScheduleRep.Get(x => !IsCompleted(x.Status)).ToList();
            var ms = _uow.GetMovieScheduleRep.Get().ToList();

            foreach (var m in ms.Where(x => ! IsCompleted(x.Status)))
            {
                _hub.Clients.Group(UserModelType.ADMIN.ToString()).SendAsync("ReceiveScheduledMovie", new
                {
                    movieScheduleID = m.Id,
                    Status = m.Status
                });
            }
        }

        private bool IsCompleted(string? status)
        {
            var st = status.ToEnum<CinemaBL.Enums.MovieScheduleEnum>();
            switch (st)
            {
                case MovieScheduleEnum.DONE:
                case MovieScheduleEnum.CANCELLED:
                    return true;
                default:
                    return false;
            }
        }

        public void SendMessageToGET_TICKET(int movieScheduleId = -1)
        {
            var bigliettai = _uow.GetUserEmployeeRep.Get(x => x.JobQualificationId == (int)JobEmployeeQualificationEnum.GET_TICKET).ToList();
            foreach (var bi in bigliettai)
            {
                MovieSchedule ms;
                if (movieScheduleId > 0)
                {
                    ms = _uow.GetMovieScheduleRep.Get(x => x.Id == movieScheduleId && x.StartDate >= DateTime.Now,
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
                        stdSeatFree = movieScheduleItem.CinemaRoom.MaxStdSeat - movieScheduleItem.StdSeat.ToDefault(),
                        TotalSeat = movieScheduleItem.CinemaRoom.MaxStdSeat + movieScheduleItem.CinemaRoom.MaxVipSeat,
                        vipSeatBusy = movieScheduleItem.VipSeat.ToDefault(),
                        vipSeatFree = movieScheduleItem.CinemaRoom.MaxVipSeat - movieScheduleItem.VipSeat.ToDefault()
                    });
                }

            }

        }
    }
}
