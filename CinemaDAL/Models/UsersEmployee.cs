using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

/// <summary>
/// specifica per i soli Employee (no admin)
/// </summary>
public partial class UsersEmployee
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int JobQualificationId { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public DateTime? Birthdate { get; set; }

    public int? CinemaRoomId { get; set; }

    /// <summary>
    /// serve solo ai bilbiettai, per indicare se sono attivi o meno {null/0, 1 }
    /// </summary>
    public int? IsActive { get; set; }

    public virtual CinemaRoom? CinemaRoom { get; set; }

    public virtual JobEmployeeQualification JobQualification { get; set; } = null!;
}
