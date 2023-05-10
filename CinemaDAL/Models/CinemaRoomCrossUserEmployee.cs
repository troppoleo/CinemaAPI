using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

/// <summary>
/// tabella che associa gli impiegati alle sale cinema
/// </summary>
public partial class CinemaRoomCrossUserEmployee
{
    public int Id { get; set; }

    public int? UserEmployeeId { get; set; }

    public int? CinemaRoomId { get; set; }

    public virtual CinemaRoom? CinemaRoom { get; set; }

    public virtual UserEmployee? UserEmployee { get; set; }
}
