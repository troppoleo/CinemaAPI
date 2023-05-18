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
    /// è il prezzo del biglietto che eventualmente potrebbe essere maggiorato per vip
    /// </summary>
    public decimal? PriceStd { get; set; }

    /// <summary>
    /// Numero di bilgietti starndard acquistati
    /// </summary>
    public int? ReservedStdSeats { get; set; }

    /// <summary>
    /// numero di biglietti Vip acquistati
    /// </summary>
    public int? ReservedVipSeat { get; set; }

    /// <summary>
    /// percentuale di maggiorazione per i prezzi Vip
    /// </summary>
    public decimal? PriceVipPercent { get; set; }

    /// <summary>
    /// la data in cui è stato generato il ticket
    /// </summary>
    public DateTime? DateTicket { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual MovieSchedule? MovieSchedule { get; set; }
}
