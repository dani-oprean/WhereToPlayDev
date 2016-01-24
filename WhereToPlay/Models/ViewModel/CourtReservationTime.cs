using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhereToPlay.Models.DB;

namespace WhereToPlay.Models.ViewModel
{
    public class CourtReservationTime
    {
        private WhereToPlayDb db = new WhereToPlayDb();

        public ReservationTime ReservationTime { get; set; }
        public bool Taken { get; set; }
        public bool NewReservation { get; set; }

        public CourtReservationTime()
        {

        }

        public CourtReservationTime(ReservationTime res)
        {
            this.NewReservation = false;
            this.Taken = false;
            this.ReservationTime = res;
        }
    }
}