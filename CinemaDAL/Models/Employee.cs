using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public int? JobQaulificationId { get; set; }

    public virtual JobQualification? JobQaulification { get; set; }
}
