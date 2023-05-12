using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

/// <summary>
/// tabelle delle &quot;sale cinema&quot;, utile per associare un employee alle sale cinema
/// </summary>
public partial class CinemaRoom
{
    public int Id { get; set; }

    /// <summary>
    /// Nome della sala
    /// </summary>
    public string? RoomName { get; set; }

    /// <summary>
    /// massimo numero di posti VIP
    /// </summary>
    public int? MaxVipSeat { get; set; }

    /// <summary>
    /// Massimo numero di posto standard
    /// </summary>
    public int? MaxStdSeat { get; set; }

    /// <summary>
    /// percentuale di maggiorazione del prezzo VIP rispetto al prezzo standard
    /// </summary>
    public decimal? UpgradeVipPrice { get; set; }

    public virtual ICollection<CinemaRoomCrossUserEmployee> CinemaRoomCrossUserEmployees { get; set; } = new List<CinemaRoomCrossUserEmployee>();

    public virtual ICollection<MovieSchedule> MovieSchedules { get; set; } = new List<MovieSchedule>();

    public virtual ICollection<Projection> Projections { get; set; } = new List<Projection>();
}
