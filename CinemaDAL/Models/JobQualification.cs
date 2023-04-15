using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

public partial class JobQualification
{
    public int Id { get; set; }

    public string ShortDescr { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
