using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaDTO
{
    public class EmployeeDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public int? JobQaulificationId { get; set; }

        public virtual JobEmployeeQualificationMapRefDTO? JobQaulification { get; set; }
    }
}
