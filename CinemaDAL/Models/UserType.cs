using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

/// <summary>
/// tabella con i vari ruoli:
/// ADMIN, EMPLOYEE, CUSTOMER
/// 
/// </summary>
public partial class UserType
{
    /// <summary>
    /// Tabella delle qualifiche che possono essere CRUD dal&apos;admin
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// nome breve
    /// </summary>
    public string? UserTypeShort { get; set; }

    public string? Description { get; set; }
}
