using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

public partial class CinemaRoom
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Projection> Projections { get; set; } = new List<Projection>();
}
