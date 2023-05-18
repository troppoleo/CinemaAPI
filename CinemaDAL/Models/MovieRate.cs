using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

public partial class MovieRate
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int? MovieId { get; set; }

    public int? ActorRate { get; set; }

    public int? TramaRate { get; set; }

    public int? AmbientRate { get; set; }

    public string? CommentNote { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Movie? Movie { get; set; }
}
