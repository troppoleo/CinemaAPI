using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

public partial class UsersAdmin
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;
}
