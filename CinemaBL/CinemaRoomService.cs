using CinemaBL.Repository;
using CinemaDAL.Models;
using CinemaDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CinemaBL.CinemaRoomService;

namespace CinemaBL
{
    public interface ICinemaRoomService
    {
        CinemaRoomEnum Add(CinemaRoomForAddDTO cRoom);
        CinemaRoomEnum Delete(int idCinemaRoom);
        IEnumerable<CinemaRoomDTO> GetAll();
        IEnumerable<CinemaRoomDTO> GetAllWhere(string roomName);
        CinemaRoomEnum Update(CinemaRoomDTO cRoom);
    }

    public class CinemaRoomService : ICinemaRoomService
    {
        private readonly IUnitOfWork _uow;

        //private readonly CinemaContext _ctx;

        //public CinemaRoomService(CinemaDAL.Models.CinemaContext ctx)
        //{
        //    _ctx = ctx;
        //}


        public CinemaRoomService(IUnitOfWork uow)
        {
            _uow = uow;
        }



        public enum CinemaRoomEnum
        {
            CREATED,
            DELETED,
            INSERTED,
            UPDATED,
            ALREADY_EXISTS,
            NONE,
            NOT_FOUND,
            /// <summary>
            /// violazione di un requisito minimo
            /// </summary>
            VIOLATION_MINIMUM_REQUIRED
        }

        public IEnumerable<CinemaRoomDTO> GetAll()
        {
            //return _ctx.CinemaRooms.ToList();

            return _uow.CinemaRoomRep.GetAll().Select(r => FillProperty(r));
        }


        public IEnumerable<CinemaRoomDTO> GetAllWhere(string roomName)
        {
            return _uow.CinemaRoomRep.GetAllWhere(x => x.RoomName == roomName).Select(r => FillProperty(r));
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

        public CinemaRoomEnum Add(CinemaRoomForAddDTO cRoom)
        {
            if (_uow.CinemaRoomRep.Find(x => x.RoomName == cRoom.RoomName) == null)
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

                _uow.CinemaRoomRep.Add(cr);
                return CinemaRoomEnum.CREATED;
            }

            return CinemaRoomEnum.ALREADY_EXISTS;
        }

        public CinemaRoomEnum Update(CinemaRoomDTO cRoom)
        {
            var cr = _uow.CinemaRoomRep.Find(x => x.Id == cRoom.Id);
            if (cr is null)
            {
                return CinemaRoomEnum.NOT_FOUND;
            }

            cr.MaxStdSeat = cRoom.MaxStdSeat;
            cr.MaxVipSeat = cRoom.MaxVipSeat;
            cr.RoomName = cRoom.RoomName;
            cr.StdSeat = cRoom.StdSeat;
            cr.UpgradeVipPrice = cRoom.UpgradeVipPrice;
            cr.VipSeat = cRoom.VipSeat;


            _uow.CinemaRoomRep.Update(cr);
            return CinemaRoomEnum.UPDATED;
        }


        public CinemaRoomEnum Delete(int idCinemaRoom)
        {
            var cr = _uow.CinemaRoomRep.Find(x => x.Id == idCinemaRoom);
            if (cr is null)
            {
                return CinemaRoomEnum.NOT_FOUND;
            }

            _uow.CinemaRoomRep.Delete(cr);
            return CinemaRoomEnum.DELETED;
        }

    }


}
