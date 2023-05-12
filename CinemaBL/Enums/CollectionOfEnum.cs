﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBL.Enums
{
    public enum CrudCinemaEnum
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
    

    /// <summary>
    /// serve per idenfiticare se un film è stato approvato
    /// </summary>
    public enum MovieApprovedStatusEnum
    { 
        IS_APPROVED = 1,
        IS_NOT_APPROVED = 0
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


    /// <summary>
    /// rappresenta i possibile valori della colonna: MOvieSchedule.status
    /// </summary>
    public enum MovieScheduleEnum
    {
        /// <summary>
        /// deve ancora iniziare
        /// </summary>
        WAITING,
        /// <summary>
        /// è in corso di visione
        /// </summary>
        IN_PROGRESS,
        /// <summary>
        /// è finito e stanno facendo le pulizie
        /// </summary>
        CLEAN_TIME,
        /// <summary>
        /// finito e sala liberata, include i 10 min extra film
        /// </summary>
        DONE
    }

}