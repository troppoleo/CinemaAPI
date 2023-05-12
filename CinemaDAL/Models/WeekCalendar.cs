using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

/// <summary>
/// contiene i giorni della settimana con le fasce di apertura
/// 
/// </summary>
public partial class WeekCalendar
{
    public int Id { get; set; }

    public string DayName { get; set; } = null!;

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }
}
