using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

/// <summary>
/// tiene traccia dei biglietti emessi
/// con l&apos;informazione dei film che un customer ha comprato
/// 
/// </summary>
public partial class Ticket
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public int? MovieScheduleId { get; set; }

    /// <summary>
    /// Valurazione del film
    /// </summary>
    public int? Rate { get; set; }

    /// <summary>
    /// Commento sul film
    /// </summary>
    public string? CommentNote { get; set; }

    /// <summary>
    /// è il prezzo del biglietto che eventualmente potrebbe essere maggiorato per vip
    /// </summary>
    public decimal? Price { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual MovieSchedule? MovieSchedule { get; set; }
}
