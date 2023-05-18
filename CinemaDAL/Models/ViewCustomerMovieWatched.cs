using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

public partial class ViewCustomerMovieWatched
{
    public string FilmName { get; set; } = null!;

    public DateTime? DateTicket { get; set; }

    public int? CustomerId { get; set; }

    public string? Actors { get; set; }

    public string? Director { get; set; }

    public int Duration { get; set; }

    public string? Genere { get; set; }

    public string? Trama { get; set; }
}
