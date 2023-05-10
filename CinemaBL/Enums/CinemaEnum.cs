using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBL.Enums
{
    public enum CinemaEnum
    {
        CREATED,
        DELETED,
        INSERTED,
        UPDATED,
        ALREADY_EXISTS,
        JOB_QUALIFICATION_NOT_EXISTS,
        NONE,
        NOT_FOUND,
        /// <summary>
        /// violazione di un requisito minimo
        /// </summary>
        VIOLATION_MINIMUM_REQUIRED
    }

    public enum JobEmployeeQualificationEnum
    {
        /// <summary>
        /// (bigliettaio) fornitore di biglietti
        /// </summary>
        GET_TICKET = 1,
        /// <summary>
        /// Responsabile di sala
        /// </summary>
        OWN_SALA = 2
    }
}
