using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public DateTime? Birthdate { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
