using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhereToPlay.Models.DB;

namespace WhereToPlay.Models.ViewModel
{
    public class CourtReservationTimes
    {
        public List<ReservationTime> All { get; set; }
        public List<ReservationTime> Taken { get; set; }

        public CourtReservationTimes()
        {}

        public CourtReservationTimes(List<ReservationTime> all, List<ReservationTime> taken)
        {
            this.All = all.ToList();
            this.Taken = taken.ToList();
        }
    }
}