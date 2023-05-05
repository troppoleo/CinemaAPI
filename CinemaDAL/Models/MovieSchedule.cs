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
    public int MovieId { get; set; }

    public int CinemaRoomId { get; set; }

    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 1 se è stato approvato dall&apos;Admin
    /// </summary>
    public int? IsApproved { get; set; }

    public virtual CinemaRoom CinemaRoom { get; set; } = null!;

    public virtual Movie Movie { get; set; } = null!;
}
