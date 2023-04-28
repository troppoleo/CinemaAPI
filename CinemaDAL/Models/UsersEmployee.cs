using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

public partial class UsersEmployee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public int JobQualificationId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime Birthdate { get; set; }

    public virtual JobEmployeeQualification JobQualification { get; set; } = null!;
}
