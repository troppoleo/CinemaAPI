using CinemaBL.Enums;
using CinemaBL.Extension;
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
        CrudCinemaEnum InsertWithOwn(CinemaRoomForInsertWithOwnDTO cc);
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
            };
        }


        //public CrudCinemaEnum Update(CinemaRoomDTO cRoom)
        //{
        //    var cr = _uow.GetCinemaRoomRep.GetByID(cRoom.Id);
        //    if (cr is null)
        //    {
        //        return CrudCinemaEnum.NOT_FOUND;
        //    }

        //    cr.MaxStdSeat = cRoom.MaxStdSeat;
        //    cr.MaxVipSeat = cRoom.MaxVipSeat;
        //    cr.RoomName = cRoom.RoomName;
        //    cr.StdSeat = cRoom.StdSeat;
        //    cr.UpgradeVipPrice = cRoom.UpgradeVipPrice;
        //    cr.VipSeat = cRoom.VipSeat;

        //    _uow.GetCinemaRoomRep.Update(cr);
        //    return CrudCinemaEnum.UPDATED;
        //}

        public CrudCinemaEnum Update(CinemaRoomDTO cRoom)
        {
            if (_uow.GetCinemaRoomRep.GetByIdAndFillMovieSchedule(cRoom.Id, out var cr2))
            {                
                // verifico se il film è stato schedulato 
                // e se stanno cercando di cambiare il numero di posti vip
                if (cr2.MovieSchedules.Count > 0
                    && cr2.MovieSchedules.Any(x => x.IsApproved.ToEnum<MovieApprovedStatusEnum>() == MovieApprovedStatusEnum.IS_APPROVED)
                    && cr2.MaxVipSeat != cRoom.MaxVipSeat)
                {
                    return CrudCinemaEnum.VIOLATION_MINIMUM_REQUIRED;
                }
            }

            if (_uow.GetCinemaRoomRep.GetByID(cRoom.Id, out var cr))
            {
                /// I posti, posti vip ecc, possono essere modificati solo quando non ci sono in programma film approvati in quella sala 
                ///     Il numero e il nome (facoltativo) devono essere unici
                /// 	Il numero può essere cambiato ma non può essere < 1 

                // controllo se il la sala è impegnata in una schedulazione in cui c'è l'approvazione
                //if (_uow.GetMovieScheduleRep.Get(x => x.CinemaRoomId ))
                //cr.MovieSchedules
                //if (_uow.GetCinemaRoomRep.HasScheduledApprovedMovie(cr.))
                //{ 
                //}

                cr.MaxStdSeat = cRoom.MaxStdSeat;
                cr.MaxVipSeat = cRoom.MaxVipSeat;
                cr.RoomName = cRoom.RoomName;
                
                _uow.GetCinemaRoomRep.Update(cr);
                return CrudCinemaEnum.UPDATED;
            }

            return CrudCinemaEnum.NOT_FOUND;
        }


        public CrudCinemaEnum Delete(int idCinemaRoom)
        {

            var cr = _uow.GetCinemaRoomRep.GetByID(idCinemaRoom);
            if (cr is null)
            {
                return CrudCinemaEnum.NOT_FOUND;
            }

            ///	Una sala NON può essere eliminata se c’è un film programmato in quella sala 
            ///	prima verifico se c'è una schedulazione:            
            if (_uow.GetMovieScheduleRep.GetByID(idCinemaRoom, out var entity))
            {
                if (entity.Status.ToEnum<MovieScheduleEnum>() != MovieScheduleEnum.DONE)
                {
                    return CrudCinemaEnum.VIOLATION_MINIMUM_REQUIRED;
                }
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

        public CrudCinemaEnum Insert(CinemaRoomForInsertDTO cRoom)
        {
            if (InsertCinemaRoom(cRoom) is null)
            {
                return CrudCinemaEnum.ALREADY_EXISTS;
            }
            return CrudCinemaEnum.CREATED;
        }


        private CrudCinemaEnum InsertCinemaRoomCrossUserEmployee(CinemaRoomForInsertWithOwnDTO cRoom)
        {
            /// Per creare una sala serve un Responsabile di Sala da associare (uno solo per sala) 
            ///     Non può esserci lo stesso Responsabile di Sala per più sale
            ///     

            // controllo che l''id fornito sia di un responsabile di sala
            if (!_uow.GetUserEmployeeRep.IsOwnSala(cRoom.userEmployeeId))
            {
                return CrudCinemaEnum.VIOLATION_MINIMUM_REQUIRED;
            }

            ///     Non può esserci lo stesso Responsabile di Sala per più sale
            //if (_uow.GetCinemaRoomCrossUserEmployeeRep.Get(x => x.UserEmployeeId == cRoom.userEmployeeId).Any())
            //{
            //    return CrudCinemaEnum.VIOLATION_MINIMUM_REQUIRED;
            //}

            ///     Non può esserci lo stesso Responsabile di Sala per più sale
            if (_uow.GetCinemaRoomCrossUserEmployeeRep.Exists(x => x.UserEmployeeId == cRoom.userEmployeeId))
            {
                return CrudCinemaEnum.VIOLATION_MINIMUM_REQUIRED;
            }

            var cr = InsertCinemaRoom(cRoom);
            if (cr == null)
            {
                return CrudCinemaEnum.ALREADY_EXISTS;
            }

            CinemaRoomCrossUserEmployee cross = new CinemaRoomCrossUserEmployee()
            {
                //https://stackoverflow.com/questions/5212751/how-can-i-retrieve-id-of-inserted-entity-using-entity-framework
                CinemaRoom = cr,    // fornendo l'oggetto che è FK quando vengono fatte le insert verranno popolate automaticamente le identity
                UserEmployeeId = cRoom.userEmployeeId
            };
            _uow.GetCinemaRoomCrossUserEmployeeRep.Insert(cross);

            return CrudCinemaEnum.CREATED;
        }


        private CinemaRoom? InsertCinemaRoom(CinemaRoomForInsertDTO cRoom)
        {
            if (_uow.GetCinemaRoomRep.Get(x => x.RoomName == cRoom.RoomName).Any())
            {
                return null;
            }

            CinemaRoom cr = new CinemaRoom()
            {
                MaxStdSeat = cRoom.MaxStdSeat,
                MaxVipSeat = cRoom.MaxVipSeat,
                RoomName = cRoom.RoomName,           
            };

            _uow.GetCinemaRoomRep.Insert(cr);
            return cr;
        }



        public CrudCinemaEnum InsertWithOwn(CinemaRoomForInsertWithOwnDTO cc)
        {
            return InsertCinemaRoomCrossUserEmployee(cc);
        }
    }


}
