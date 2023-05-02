using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaDTO
{
    /// <summary>
    /// utile per la definizione di una nuova Job Qualification
    /// </summary>
    public class JobEmployeeQualificationMinimalDTO
    {
        public string ShortDescr { get; set; }

        public string Description { get; set; }
    }

    public class JobEmployeeQualificationMapDTO : JobEmployeeQualificationMinimalDTO
    {
        public int Id { get; set; }
    }

    public class JobEmployeeQualificationMapRefDTO : JobEmployeeQualificationMapDTO
    {
        public virtual ICollection<UsersEmployeeDTO> Employees { get; set; } = new List<UsersEmployeeDTO>();
    }

}
