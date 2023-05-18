using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

/// <summary>
/// Definisce i prezzi di default per Standard e Vip Seat
/// </summary>
public partial class PriceTicketDefault
{
    public string SeatType { get; set; } = null!;

    public decimal? Price { get; set; }
}
