using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

/// <summary>
/// tiene traccia dei film che un customer ha prenotato
/// </summary>
public partial class CustomerCrossMovieSchedule
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public int? MovieScheduleId { get; set; }

    public int? Ticket { get; set; }

    /// <summary>
    /// Valurazione del film
    /// </summary>
    public int? Rate { get; set; }

    /// <summary>
    /// Commento sul film
    /// </summary>
    public string? CommentNote { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual MovieSchedule? MovieSchedule { get; set; }
}
