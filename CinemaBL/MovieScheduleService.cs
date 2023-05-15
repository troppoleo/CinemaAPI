using CinemaBL.Enums;
using CinemaBL.Repository;
using CinemaDAL.Models;
using CinemaDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaBL.Extension;
using Microsoft.Extensions.Configuration;

namespace CinemaBL
{
    public interface IMovieScheduleService
    {
        IEnumerable<object> GetDirect(DateTime dateTime);
        CrudCinemaEnum Insert(MovieScheduleForInsertDTO ms);
        CrudCinemaEnum Update(MovieScheduleForUpdateDTO ms);
    }

    public class MovieScheduleService : IMovieScheduleService
    {
        private readonly IUnitOfWorkGeneric _uow;
        private readonly IConfiguration _conf;
        private readonly object lockInsert = new object();
        public MovieScheduleService(IUnitOfWorkGeneric uow, IConfiguration conf)
        {
            _uow = uow;
            _conf = conf;
        }



        public CrudCinemaEnum Update(MovieScheduleForUpdateDTO ms)
        {
            var sh = _uow.GetMovieScheduleRep.GetByID(ms.Id);
            if (sh != null)
            {
                if (CheckSlot(ms, sh.Id) == CrudCinemaEnum.CREATED)     // non crea nulla serve solo se è valido il nuovo censimento
                {
                    sh.MovieId = ms.MovieId;
                    sh.CinemaRoomId = ms.CinemaRoomId;
                    sh.StartDate = ms.StartDate;
                    sh.IsApproved = ms.IsApproved;

                    // non ha senso che siano modificabili
                    //sh.VipSeat = ms.VipSeat;
                    //sh.StdSeat = ms.StdSeat;

                    _uow.GetMovieScheduleRep.Update(sh);

                    return CrudCinemaEnum.UPDATED;
                }
            }

            return CrudCinemaEnum.VIOLATION_MINIMUM_REQUIRED;
        }

        public Enums.CrudCinemaEnum Insert(MovieScheduleForInsertDTO ms)
        {
            lock (lockInsert)
            {
                return InsertMng(ms);
            }
        }

        private Enums.CrudCinemaEnum InsertMng(MovieScheduleForInsertDTO ms)
        {

            var result = CheckSlot(ms);
            if (result == CrudCinemaEnum.CREATED)   // qui significa che si può creare non che l'ha creato
            {
                // recupero i posti disponibili dalle proprietà della sala cinema
                var cr = _uow.GetCinemaRoomRep.GetByID(ms.CinemaRoomId);

                MovieSchedule movieSchedule = new MovieSchedule()
                {
                    MovieId = ms.MovieId,
                    CinemaRoomId = ms.CinemaRoomId,
                    StartDate = ms.StartDate,
                    //VipSeat = ms.VipSeat,
                    //StdSeat = ms.StdSeat
                    VipSeat = cr.MaxVipSeat,
                    StdSeat = cr.MaxStdSeat
                };

                _uow.GetMovieScheduleRep.Insert(movieSchedule);
                return CrudCinemaEnum.CREATED;
            }
            return result;


            ///// Un film non può essere proiettato in più di due sale contemporaneamente (in due potenzialmente sì) 

            ///// Può essere modificato finché non viene approvato dall’ADMIN
            /////     Non ci possono essere due film nella stessa sala allo stesso orario!

            ///// La data di inizio film non può essere minore di oggi, l’orario deve essere maggiore di adesso 
            /////     Attenzione agli orari del cinema(vedi in cima al word)
            //if (ms.StartDate < DateTime.Now)
            //{
            //    return CrudCinemaEnum.VIOLATION_MINIMUM_REQUIRED;
            //}


            //var d = _uow.GetWeekCalendarRep.GetByID((int)ms.StartDate.DayOfWeek);
            //// controllo che l'orario di inizio sia compatibile con l'orario di apertura del giorno scelto
            //if (ms.StartDate.TimeOfDay < d.StartTime)
            //{
            //    return CrudCinemaEnum.VIOLATION_MINIMUM_REQUIRED;
            //}

            ////// controllo che l'orario di fine sia compatibile con l'orario di chiusura del giorno scelto

            //// data e ora apertura cinema nel giorno scelto
            //DateTime start = ms.StartDate.Date.Add(d.StartTime);
            //DateTime end;
            //if (d.StartTime < d.EndTime)
            //{
            //    end = ms.StartDate.Date.Add(d.EndTime);
            //}
            //else
            //{
            //    // la chiusura è quindi prevista per il giorno successivo
            //    end = ms.StartDate.Date.AddDays(1).Add(d.EndTime);
            //}

            //// rimuovo i secondi dalla data dell'utente perchè non farebbero funzionare l'algoritmo
            //ms.StartDate = ms.StartDate.Trim(TimeSpan.TicksPerMinute);


            //var extraTime = int.Parse(_conf["Generic:CleaninigTime"]);
            //MngCheckSchedulationSlot mngCS = new MngCheckSchedulationSlot(_uow);
            //mngCS.Inizialize(start, end, extraTime, ms.CinemaRoomId);
            //// controlla l'orario di fine film sia compatibile e che non ci sia accavvalamento di film sulla sala
            //if (mngCS.InsertSlot(ms.StartDate, _uow.GetMovieRep.GetByID(ms.MovieId).Duration + extraTime))
            //{
            //    ///o	Un film non può essere proiettato in più di due sale contemporaneamente (in due potenzialmente sì) 
            //    /// conto le distinte sale cinema su cui c'è la proiezione e se >= 2 sono già al limite
            //    if (_uow.GetMovieScheduleRep.Get(x => x.MovieId == ms.MovieId && x.StartDate.Date == ms.StartDate.Date)
            //        .Select(x => x.CinemaRoomId).Distinct().Count() >= 2)
            //    {
            //        return CrudCinemaEnum.VIOLATION_MINIMUM_REQUIRED;
            //    }


            //    MovieSchedule movieSchedule = new MovieSchedule()
            //    {
            //        MovieId = ms.MovieId,
            //        CinemaRoomId = ms.CinemaRoomId,
            //        StartDate = ms.StartDate,
            //        VipSeat = ms.VipSeat,
            //        StdSeat = ms.StdSeat
            //    };

            //    _uow.GetMovieScheduleRep.Insert(movieSchedule);
            //    return CrudCinemaEnum.CREATED;
            //}
            //else
            //{
            //    return CrudCinemaEnum.VIOLATION_MINIMUM_REQUIRED;
            //}

        }

        /// <summary>
        /// contiene tutta la logica per impedire l'accavallamento degli orari 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="idSlot">solo per l'update</param>
        /// <returns></returns>
        private CrudCinemaEnum CheckSlot(MovieScheduleForInsertDTO ms, int idSlot = -1)
        {
            if (ms.StartDate < DateTime.Now)
            {
                return CrudCinemaEnum.VIOLATION_MINIMUM_REQUIRED;
            }


            var d = _uow.GetWeekCalendarRep.GetByID((int)ms.StartDate.DayOfWeek);
            // controllo che l'orario di inizio sia compatibile con l'orario di apertura del giorno scelto
            if (ms.StartDate.TimeOfDay < d.StartTime)
            {
                return CrudCinemaEnum.VIOLATION_MINIMUM_REQUIRED;
            }

            //// controllo che l'orario di fine sia compatibile con l'orario di chiusura del giorno scelto

            // data e ora apertura cinema nel giorno scelto
            DateTime start = ms.StartDate.Date.Add(d.StartTime);
            DateTime end;
            if (d.StartTime < d.EndTime)
            {
                end = ms.StartDate.Date.Add(d.EndTime);
            }
            else
            {
                // la chiusura è quindi prevista per il giorno successivo
                end = ms.StartDate.Date.AddDays(1).Add(d.EndTime);
            }

            // rimuovo i secondi dalla data dell'utente perchè non farebbero funzionare l'algoritmo
            ms.StartDate = ms.StartDate.Trim(TimeSpan.TicksPerMinute);


            var extraTime = int.Parse(_conf["Generic:CleaninigTime"]);
            MngCheckSchedulationSlot mngCS = new MngCheckSchedulationSlot(_uow);
            mngCS.Inizialize(start, end, extraTime, ms.CinemaRoomId);

            if (idSlot != -1)
            {
                mngCS.DeleteSlot(idSlot);
            }



            // controlla l'orario di fine film sia compatibile e che non ci sia accavvalamento di film sulla sala
            if (mngCS.InsertSlot(ms.StartDate, _uow.GetMovieRep.GetByID(ms.MovieId).Duration + extraTime))
            {
                ///o	Un film non può essere proiettato in più di due sale contemporaneamente (in due potenzialmente sì) 
                /// conto le distinte sale cinema su cui c'è la proiezione e se >= 2 sono già al limite
                var salaCounter = _uow.GetMovieScheduleRep.Get(x => x.MovieId == ms.MovieId && x.StartDate.Date == ms.StartDate.Date)
                    .Select(x => x.CinemaRoomId).Distinct().Count();

                if (idSlot == -1)   // questo controllo ha senso solo nel caso di insert perchè nel caso di update posso essere 2 
                {
                    if (salaCounter >= 2)
                    {
                        return CrudCinemaEnum.VIOLATION_MINIMUM_REQUIRED;
                    }
                }


                //MovieSchedule movieSchedule = new MovieSchedule()
                //{
                //    MovieId = ms.MovieId,
                //    CinemaRoomId = ms.CinemaRoomId,
                //    StartDate = ms.StartDate,
                //    VipSeat = ms.VipSeat,
                //    StdSeat = ms.StdSeat
                //};

                //_uow.GetMovieScheduleRep.Insert(movieSchedule);
                return CrudCinemaEnum.CREATED;
            }
            else
            {
                return CrudCinemaEnum.VIOLATION_MINIMUM_REQUIRED;
            }
        }

        public IEnumerable<object> GetDirect(DateTime dateTime)
        {
            var ss = _uow.GetMovieScheduleRep.Get(x => x.StartDate.Date == dateTime.Date,
                orderBy: x => x.OrderBy(d => d.StartDate),
                $"{nameof(MovieSchedule.Movie)},{nameof(MovieSchedule.CinemaRoom)}");

            return ss.Select(x => new { x.StartDate, x.Movie.FilmName, x.CinemaRoom.RoomName, x.Status });
        }
    }



    public class MngCheckSchedulationSlot
    {
        public List<MinutSlotDTO> MinuteList = new List<MinutSlotDTO>();
        private DateTime StartProgammation;
        private DateTime EndProgrammation;

        public int ExtraTime { get; private set; }
        public int cinemaRoomId { get; private set; }
        public IUnitOfWorkGeneric _uow { get; }


        public MngCheckSchedulationSlot(IUnitOfWorkGeneric uow)
        {
            _uow = uow;
        }

        private void CreateSlot()
        {
            DateTime t = StartProgammation;

            int i = 0;
            while (t < EndProgrammation)
            {
                t = StartProgammation.AddMinutes(i);
                MinuteList.Add(new MinutSlotDTO()
                {
                    Slot = t,
                    StatusEnum = MinutSlotDTO.SlotStatusEnum.FREE
                });
                i++;
            }
        }

        public bool InsertSlot(DateTime startDate, int duration)
        {
            DateTime endFilm = startDate.AddMinutes(duration).AddMinutes(ExtraTime);
            bool right = true;

            DateTime t = startDate;

            int i = 0;
            while (t < endFilm)
            {
                t = startDate.AddMinutes(i);
                var sl = MinuteList.Where(x => x.Slot == t).FirstOrDefault();

                if (sl == null)
                {
                    right = false;
                    break;
                }
                else
                {
                    if (sl.StatusEnum == MinutSlotDTO.SlotStatusEnum.FREE)  // è libero
                    {
                        sl.StatusEnum = MinutSlotDTO.SlotStatusEnum.PENDING;
                    }
                    else
                    {
                        right = false;
                        break;
                    }
                }
                i++;
            }

            if (right)
            {
                // la programmazione ha avuto successo: porto lo status da pending a Busy
                foreach (var lm in MinuteList.Where(x => x.StatusEnum == MinutSlotDTO.SlotStatusEnum.PENDING))
                {
                    lm.StatusEnum = MinutSlotDTO.SlotStatusEnum.BUSY;
                }
            }
            else
            {
                // programmazione non valida, ripristino lo stato
                foreach (var lm in MinuteList.Where(x => x.StatusEnum == MinutSlotDTO.SlotStatusEnum.PENDING))
                {
                    lm.StatusEnum = MinutSlotDTO.SlotStatusEnum.FREE;
                }
            }

            return right;
        }

        public void Inizialize(DateTime start, DateTime end, int extraTime, int cinemaRoomId)
        {
            this.StartProgammation = start;
            this.EndProgrammation = end;
            this.ExtraTime = extraTime;
            this.cinemaRoomId = cinemaRoomId;

            CreateSlot();
            FillSlotFromDB();
        }

        private void FillSlotFromDB()
        {
            // estraggo i film della giornata relativamente alla sala scelta
            var moviesInDay = _uow.GetMovieScheduleRep.Get(x => x.StartDate.Date == this.StartProgammation.Date && x.CinemaRoomId == cinemaRoomId);
            foreach (var i in moviesInDay)
            {
                InsertSlot(i.StartDate, _uow.GetMovieRep.GetByID(i.MovieId).Duration + ExtraTime);
            }
        }


        /// <summary>
        /// serve per liberare uno slot preso dal DB che potenzialmente vado a modificare,
        /// ad esempio se devo spostare di pochi minuti l'orario di inizio
        /// utile solo per l'update
        /// </summary>
        /// <param name="idSlot"></param>
        public void DeleteSlot(int idSlot)
        {
            var recordOnDB = _uow.GetMovieScheduleRep.GetByID(idSlot);
            var duration = _uow.GetMovieRep.GetByID(recordOnDB.MovieId).Duration;
            var startDate = recordOnDB.StartDate;

            DateTime endFilm = startDate.AddMinutes(duration).AddMinutes(ExtraTime);
            DateTime t = startDate;

            int i = 0;
            while (t < endFilm)
            {
                t = startDate.AddMinutes(i);
                var sl = MinuteList.Where(x => x.Slot == t).FirstOrDefault();

                if (sl == null)
                {
                    break;
                }
                else
                {
                    if (sl.StatusEnum == MinutSlotDTO.SlotStatusEnum.BUSY)
                    {
                        sl.StatusEnum = MinutSlotDTO.SlotStatusEnum.FREE;
                    }
                    else
                    {
                        break;
                    }
                }
                i++;
            }

        }
    }


    public class MinutSlotDTO
    {
        public enum SlotStatusEnum
        {
            FREE,
            BUSY,
            /// <summary>
            /// sto verificando la possibilità di occuparlo
            /// </summary>        
            PENDING
        }

        public DateTime Slot { get; set; }
        public SlotStatusEnum StatusEnum { get; set; }
    }
}
