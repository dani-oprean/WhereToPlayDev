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
        private DateTime date;

        public Court Court { get; set; }
        public List<CourtReservationTime> CourtReservationTimes { get; set; }
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                if(Court !=null)
                {
                    List<ReservationTime> reservedTimes = db.Reservations.Where(r => (r.CourtID == Court.IDCourt && r.ReservationDate == date)).Select(rt => rt.ReservationTime).ToList();
                    List<CourtReservationTime> reserved = CourtReservationTimes.Where(t => reservedTimes.Contains(t.ReservationTime)).ToList();
                    foreach (CourtReservationTime item in reserved)
                        item.Taken = true;
                }
            }
        }

        public CourtReservation()
        {
            this.CourtReservationTimes = new List<CourtReservationTime>();
            this.date = DateTime.Now;
        }

        public CourtReservation(int? courtId)
        {
            this.CourtReservationTimes = new List<CourtReservationTime>();
            this.date = DateTime.Now;
            this.Court = db.Courts.Find(courtId);
            foreach (ReservationTime item in db.ReservationTimes.ToList())
            {
                CourtReservationTimes.Add(new CourtReservationTime(item));
            }
        }
    }
}