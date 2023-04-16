using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaDTO
{
    public class JobQualificationDTO
    {
        public int Id { get; set; }

        public string ShortDescr { get; set; } = null!;

        public string Description { get; set; } = null!;

        public virtual ICollection<EmployeeDTO> Employees { get; set; } = new List<EmployeeDTO>();

    }
}
