using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaDTO
{
    

    public class MovieForAddDTO
    {
        public string FilmName { get; set; }
        public int Duration { get; set; }
        public string Genere { get; set; }
        public string Trama { get; set; }
        public string MoviePlot { get; set; }
        public string Actors { get; set; }
        public string Director { get; set; }
        public int ProductionYear { get; set; }
        public string Cover { get; set; }
    }

    public class MovieDTO : MovieForAddDTO
    {
        public int ID { get; set; }
    }
}
