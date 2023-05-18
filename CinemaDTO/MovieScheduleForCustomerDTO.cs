using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaDTO
{
    public class MovieFilterForCustomerDTO
    {
        public DateTime StartDate { get; set; }
        public string Genere { get; set; }
    }

    public class MovieScheduleForCustomerDTO : MovieFilterForCustomerDTO
    {
        public string FilmName { get; set; }
        //public DateTime StartDate { get; set; }
        public string Actors { get; set; }
        public int Duration { get; set; }
        //public string Genere { get; set; }
        public string Trama { get; set; }
        public int LimitAge { get; set; }
    }
}
