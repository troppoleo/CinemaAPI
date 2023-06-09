﻿using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

/// <summary>
/// mette in relazione i film con le sale cinematografiche, contiene:
/// &gt; data e ora di inizio 
/// &gt; l&apos;approvazione dell&apos;ADMIN {1, 0}
/// 
/// </summary>
public partial class MovieSchedule
{
    public int Id { get; set; }

    public int MovieId { get; set; }

    public int CinemaRoomId { get; set; }

    public DateTime StartDate { get; set; }

    /// <summary>
    /// 1 se è stato approvato dall&apos;Admin
    /// </summary>
    public int? IsApproved { get; set; }

    /// <summary>
    /// Si intende i posti VIP rimasti
    /// </summary>
    public int? VipSeat { get; set; }

    /// <summary>
    /// Si intende i posti Standard rimasti
    /// </summary>
    public int? StdSeat { get; set; }

    /// <summary>
    /// dominio:
    /// WAITING --&gt; deve ancora iniziare
    /// IN_PROGRESS --&gt; è in corso di visione
    /// CLEAN_TIME --&gt; è finito e stanno facendo le pulizie
    /// DONE --&gt; finito e sala liberata, include i 10 min extra film
    /// 
    /// utile per semplificare i filtri, aggiornata dal BGW
    /// </summary>
    public string? Status { get; set; }

    public virtual CinemaRoom CinemaRoom { get; set; } = null!;

    public virtual Movie Movie { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
