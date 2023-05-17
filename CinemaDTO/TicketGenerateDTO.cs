using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaDTO
{
    /// <summary>
    /// serve come modello per la creazione di un ticket
    /// </summary>
    public class TicketGenerateDTO
    {
        public int MovieScheduleId { get; set; }
        public double PriceStd { get; set; }
        public double PriceVipPercent { get; set; }
        public int reservedStdSeats { get; set; }
        public int reservedVipSeat { get; set; }
    }


    /// <summary>
    /// l'oggetto restituito post creazione 
    /// </summary>
    public class TicketGenerateResultDTO
    { 
        public int IdTicket { get; set; }
        public double TotalPrice { get; set;}
    }
}
