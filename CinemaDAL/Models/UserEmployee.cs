using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

/// <summary>
/// specifica per i soli Employee (no admin)
/// </summary>
public partial class UserEmployee
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int JobQualificationId { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    /// <summary>
    /// serve solo ai bigliettai, per indicare se sono attivi o meno {null/0, 1 }
    /// </summary>
    public int? IsActive { get; set; }

    public virtual ICollection<CinemaRoomCrossUserEmployee> CinemaRoomCrossUserEmployees { get; set; } = new List<CinemaRoomCrossUserEmployee>();

    public virtual JobEmployeeQualification JobQualification { get; set; } = null!;
}
