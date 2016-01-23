using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhereToPlay.Models.DB;
using WhereToPlay.Models;

namespace WhereToPlay.Models.ViewModel
{
    public class CourtReservation
    {
        private WhereToPlayDb db = new WhereToPlayDb();

        public Court Court { get; set; }
        public List<ReservationTime> Times { get; set; }
        public List<ReservationTime> Taken { get; set; }
        public List<ReservationTime> ReservedNow { get; set; }

        public CourtReservation()
        {

        }

        public CourtReservation(int? courtId)
        {
            this.Court = db.Courts.Find(courtId);
            this.Times = db.ReservationTimes.ToList();
        }

        public List<ReservationTime> GetTakenHours(DateTime date)
        {
            //orele rezervarilor pentru toate rezervarile dintr-o data anume pentru un anume court
            List<ReservationTime> res = db.Reservations.Where(r => (r.CourtID == Court.IDCourt && r.ReservationDate == date)).Select(rt=>rt.ReservationTime).ToList();
            return res;
        }
    }
}