using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaDTO
{
    public class SeatRoomDTO
    {
        public int cinemaRoomId { get; set; }
        public int TotalSeat { get; set; }
        public int vipSeatFree { get; set; }
        public int vipSeatBusy { get; set; }
        public int stdSeatFree { get; set;}
        public int stdSeatBusy { get; set; }
    }
}
