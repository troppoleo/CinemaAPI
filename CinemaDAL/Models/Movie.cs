using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

/// <summary>
/// elenco dei film
/// </summary>
public partial class Movie
{
    public int Id { get; set; }

    public string FilmName { get; set; } = null!;

    public int Duration { get; set; }

    public string? Genere { get; set; }

    public string? Trama { get; set; }

    /// <summary>
    /// è la trama
    /// </summary>
    public string? MoviePlot { get; set; }

    /// <summary>
    /// lista degli attori principali
    /// </summary>
    public string? Actors { get; set; }

    /// <summary>
    /// regista
    /// </summary>
    public string? Director { get; set; }

    public int? ProductionYear { get; set; }

    /// <summary>
    /// questo andrebbe fatto come &quot;image&quot; ma posso mettere anche l&apos;url per ora mi semplifico la vita
    /// </summary>
    public string? Cover { get; set; }

    /// <summary>
    /// indica se è vietato ai minori di anni X
    /// </summary>
    public int? LimitAge { get; set; }

    public virtual ICollection<MovieRate> MovieRates { get; set; } = new List<MovieRate>();

    public virtual ICollection<MovieSchedule> MovieSchedules { get; set; } = new List<MovieSchedule>();
}
