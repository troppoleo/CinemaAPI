using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaDTO
{
    public class MovieScheduleForInsertDTO
    {
        public int MovieId { get; set; }

        public int CinemaRoomId { get; set; }

        public DateTime StartDate { get; set; }

        public int? StdSeat { get; set; }
        public int? VipSeat { get; set; }

        
    }

    public class MovieScheduleDTO : MovieScheduleForInsertDTO
    {
        public int Id { get; set; }

        public int? IsApproved { get; set; }
    }


    public class MovieScheduleDetailDTO : MovieScheduleDTO 
    { 

    }

}
