﻿using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

public partial class ViewReview
{
    public string FilmName { get; set; } = null!;

    public int? ActorRate { get; set; }

    public int? TramaRate { get; set; }

    public int? AmbientRate { get; set; }

    public string? CommentNote { get; set; }
}
