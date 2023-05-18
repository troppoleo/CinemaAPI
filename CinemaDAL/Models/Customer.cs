using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public DateTime Birthdate { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public virtual ICollection<MovieRate> MovieRates { get; set; } = new List<MovieRate>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
