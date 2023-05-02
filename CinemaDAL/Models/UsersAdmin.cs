using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

/// <summary>
/// Specifica per gli amministratori
/// </summary>
public partial class UsersAdmin
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;
}
