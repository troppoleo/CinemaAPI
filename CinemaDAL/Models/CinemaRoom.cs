using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

/// <summary>
/// tabelle delle &quot;sale cinema&quot;, utile per associare un employee alle sale cinema
/// </summary>
public partial class CinemaRoom
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Projection> Projections { get; set; } = new List<Projection>();

    public virtual ICollection<UsersEmployee> UsersEmployees { get; set; } = new List<UsersEmployee>();
}
