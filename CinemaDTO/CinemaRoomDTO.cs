using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaDTO
{
    

    public class CinemaRoomForInsertDTO
    {
        

        /// <summary>
        /// Nome della sala
        /// </summary>
        public string? RoomName { get; set; }

        /// <summary>
        /// numero di posti VIP assegnati
        /// </summary>
        //public int? VipSeat { get; set; }

        /// <summary>
        /// numero di posto standard assegnati
        /// </summary>
        //public int? StdSeat { get; set; }

        /// <summary>
        /// massimo numero di posti VIP
        /// </summary>
        public int? MaxVipSeat { get; set; }

        /// <summary>
        /// Massimo numero di posto standard
        /// </summary>
        public int? MaxStdSeat { get; set; }

        /// <summary>
        /// percentuale di maggiorazione del prezzo VIP rispetto al prezzo standard
        /// </summary>
        public decimal? UpgradeVipPrice { get; set; }

    }


    /// <summary>
    /// per la creazione di una sala con il responsabile di sala (requisito per la creazione di una sala)
    /// </summary>
    public class CinemaRoomForInsertWithOwnDTO: CinemaRoomForInsertDTO
    {
        public int userEmployeeId { get; set; }
        //public int cinemaRoomId { get; set; }
    }

    public class CinemaRoomDTO : CinemaRoomForInsertDTO
    {
        public int Id { get; set; }
    }

}
