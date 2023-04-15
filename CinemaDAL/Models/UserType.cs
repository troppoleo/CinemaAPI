using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

public partial class UserType
{
    public int Id { get; set; }

    public string? UserTypeShort { get; set; }

    public string? Description { get; set; }
}
