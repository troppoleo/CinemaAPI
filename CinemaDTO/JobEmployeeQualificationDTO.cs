using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaDTO
{
    public class JobEmployeeQualificationForInsertDTO
    {
        public string ShortDescr { get; set; }
        public string Description { get; set; }
        public int? MinimumRequired { get; set; }
        
    }
    public class JobEmployeeQualificationDTO : JobEmployeeQualificationForInsertDTO
    {
        public int Id { get; set; }


    }
}
