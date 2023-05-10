using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

/// <summary>
/// contiene la schedulazione/programmazione delle proiezioni e l&apos;associazione della sala cinema
/// </summary>
public partial class Projection
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public decimal? StartShow { get; set; }

    public decimal? EndShow { get; set; }

    public int? CinemaRoomId { get; set; }

    public virtual CinemaRoom? CinemaRoom { get; set; }
}
