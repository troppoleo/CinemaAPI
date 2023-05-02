using System;
using System.Collections.Generic;

namespace CinemaDAL.Models;

/// <summary>
/// contiene le possibili mansioni che possono essere date ai soli EMPLOYEE:
/// &gt; Responsabili di sala
/// &gt; bigliettai
/// </summary>
public partial class JobEmployeeQualification
{
    public int Id { get; set; }

    public string ShortDescr { get; set; } = null!;

    public string Description { get; set; } = null!;

    /// <summary>
    /// Numero minimo richiesto di Employee
    /// </summary>
    public int? MinimumRequired { get; set; }

    public virtual ICollection<UsersEmployee> UsersEmployees { get; set; } = new List<UsersEmployee>();
}
