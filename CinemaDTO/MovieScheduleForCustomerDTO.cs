﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaDTO
{
    public class MovieFilterForCustomerDTO
    {
        public DateTime StartDate { get; set; }
        public string Genere { get; set; }
    }

    public class MovieScheduleForCustomerDTO : MovieFilterForCustomerDTO
    {
        public int MovieScheduleId { get; set; }
        public string FilmName { get; set; }
        //public DateTime StartDate { get; set; }
        public string Actors { get; set; }
        public int Duration { get; set; }
        //public string Genere { get; set; }
        public string Trama { get; set; }
        public int LimitAge { get; set; }
    }

    public class MovieDetailForCustomerDTO
    {

        public string FilmName { get; set; }
        public DateTime StartDate { get; set; }
        public string Actors { get; set; }
        public int Duration { get; set; }
        public string Genere { get; set; }
        public string Trama { get; set; }
        public int LimitAge { get; set; }
        public string RoomName { get; set; }
        public int FreeSeatStd { get; set; }
        public int FreeSeatVip { get; set; }
    }

    public class BuyTicketCustomerDTO
    {
        public int MovieScheduleId { get; set; }
        public int ReservedStdSeat { get; set; }
        public int ReservedVipSeat { get; set; }
    }

    public class BuyedTicketsCustomerDTO
    {
        public int IdTicket { get; set; }
        public int MovieScheduleId { get; set; }
        public int ReservedStdSeat { get; set; }
        public int ReservedVipSeat { get; set; }
        public decimal TotalPrice { get; set; }
        public string MessageBackForCustomer { get; set; }
    }

    public class MovieReviewDTO
    {
        public int IdTicket { get; set; }
        public int actorRate { get; set; }
        public int tramaRate { get; set; }
        public int ambientRate { get; set; }
        public string commentNote { get; set; }
    }

    public class MovieWatchedDTO
    {
        public string FilmName { get; set; }
        public DateTime DateTicket { get; set; }

        public string  Actors { get; set; }
        public string director { get; set; }
        public int duration { get; set; }
        public string genere { get; set; }
        public string trama { get; set; }
    }

}
