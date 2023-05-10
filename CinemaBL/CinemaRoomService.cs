using CinemaBL.Enums;
using CinemaBL.Repository;
using CinemaDAL.Models;
using CinemaDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static CinemaBL.CinemaRoomService;

namespace CinemaBL
{
    public interface ICinemaRoomService
    {
        CrudCinemaEnum Delete(int idCinemaRoom);
        IEnumerable<CinemaRoomDTO> GetAll();
        CinemaRoomDTO? GetByID(int id);
        CrudCinemaEnum Insert(CinemaRoomForInsertDTO cRoom);
        CrudCinemaEnum Update(CinemaRoomDTO cRoom);
    }


    public class CinemaRoomService : ICinemaRoomService
    {
        private readonly IUnitOfWorkGeneric _uow;


        public CinemaRoomService(IUnitOfWorkGeneric uow)
        {
            _uow = uow;
        }



        public IEnumerable<CinemaRoomDTO> GetAll()
        {
            return _uow.GetCinemaRoomRep.Get().Select(r => FillProperty(r));
        }


        public CinemaRoomDTO? GetByID(int id)
        {
            var xx = _uow.GetCinemaRoomRep.GetByID(id);
            if (xx == null)
            {
                return null;
            }

            return FillProperty(xx);
        }

        private static CinemaRoomDTO FillProperty(CinemaRoom r)
        {
            return new CinemaDTO.CinemaRoomDTO()
            {
                Id = r.Id,
                MaxStdSeat = r.MaxStdSeat,
                MaxVipSeat = r.MaxVipSeat,
                RoomName = r.RoomName,
                StdSeat = r.StdSeat,
                UpgradeVipPrice = r.UpgradeVipPrice,
                VipSeat = r.VipSeat
            };
        }

        public CrudCinemaEnum Insert(CinemaRoomForInsertDTO cRoom)
        {
            if (_uow.GetCinemaRoomRep.Get(x => x.RoomName == cRoom.RoomName) == null)
            {
                CinemaRoom cr = new CinemaRoom()
                {
                    MaxStdSeat = cRoom.MaxStdSeat,
                    MaxVipSeat = cRoom.MaxVipSeat,
                    RoomName = cRoom.RoomName,
                    StdSeat = cRoom.StdSeat,
                    UpgradeVipPrice = cRoom.UpgradeVipPrice,
                    VipSeat = cRoom.VipSeat
                };

                _uow.GetCinemaRoomRep.Insert(cr);
                return CrudCinemaEnum.CREATED;
            }

            return CrudCinemaEnum.ALREADY_EXISTS;
        }

        public CrudCinemaEnum Update(CinemaRoomDTO cRoom)
        {
            var cr = _uow.GetCinemaRoomRep.GetByID(cRoom.Id);
            if (cr is null)
            {
                return CrudCinemaEnum.NOT_FOUND;
            }

            cr.MaxStdSeat = cRoom.MaxStdSeat;
            cr.MaxVipSeat = cRoom.MaxVipSeat;
            cr.RoomName = cRoom.RoomName;
            cr.StdSeat = cRoom.StdSeat;
            cr.UpgradeVipPrice = cRoom.UpgradeVipPrice;
            cr.VipSeat = cRoom.VipSeat;

            _uow.GetCinemaRoomRep.Update(cr);
            return CrudCinemaEnum.UPDATED;
        }


        public CrudCinemaEnum Delete(int idCinemaRoom)
        {
            
            var cr = _uow.GetCinemaRoomRep.GetByID(idCinemaRoom);
            if (cr is null)
            {
                return CrudCinemaEnum.NOT_FOUND;
            }

            ///	Una sala NON può essere eliminata se c’è un film programmato in quella sala 
            if (_uow.GetMovieScheduleRep.GetByID(idCinemaRoom).Status != MovieScheduleEnum.DONE.ToString())
            {
                return CrudCinemaEnum.VIOLATION_MINIMUM_REQUIRED;
            }

            

            _uow.GetCinemaRoomRep.Delete(cr);
            return CrudCinemaEnum.DELETED;
        }

        private bool CheckStatusIsDone(string status)
        {
            Enum.TryParse<MovieScheduleEnum>(status, true, out MovieScheduleEnum st);

            switch (st)
            {
                case MovieScheduleEnum.DONE:
                    return true;
                default:
                    return false;
            }
        }
    }


}
