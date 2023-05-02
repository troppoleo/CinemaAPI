using CinemaDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBL
{
    public class JobEmployeeQualificationService
    {
        private static JobEmployeeQualificationService _instance;
        public JobEmployeeQualificationService() { }

        public int MinumRequired { get; private set; }

        public static JobEmployeeQualificationService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new JobEmployeeQualificationService();

                CinemaContext ctx = new CinemaContext ();

                // TODO: "OWN_SALA" andrebbe preso da file di configurazione
                var own_sala = ctx.JobEmployeeQualifications.Where(x => x.ShortDescr == JobEmployeeQualificationEnum.OWN_SALA.ToString());
                _instance.MinumRequired = own_sala.Select(x=> x.MinimumRequired).First().Value;
            }
            return _instance;
        }

        /// <summary>
        /// contiene le mansioni fisse
        /// </summary>
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
}
