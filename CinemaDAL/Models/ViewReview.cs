using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

public partial class ViewReview
{
    public int? Rate { get; set; }

    public string? CommentNote { get; set; }

    public string? FilmName { get; set; }
}
