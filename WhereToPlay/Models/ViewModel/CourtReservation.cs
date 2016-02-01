using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhereToPlay.Models.DB;
using WhereToPlay.Models;

namespace WhereToPlay.Models.ViewModel
{
    [Serializable]
    public class CourtReservation
    {
        private WhereToPlayDb db = new WhereToPlayDb();
        private DateTime date;
        
        public Court Court { get; set; }
        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                List<String> res = db.Reservations.Where(r => (r.ReservationDate.Year == date.Year)&& (r.ReservationDate.Month == date.Month)&& (r.ReservationDate.Day == date.Day))
                                    .Select(t => t.ReservationTime.Hours).ToList();

                NZeceDoispe = res.Contains("10-12");
                NDoispePaispe = res.Contains("12-14");
                NPaispeSaispe = res.Contains("14-16");
                NSaispeOptspe = res.Contains("16-18");
                NOptspeDouazeci = res.Contains("18-20");
                NDouazeciDouajdoi = res.Contains("20-22");
            }
        }
        public bool TZeceDoispe { get; set; }
        public bool TDoispePaispe { get; set; }
        public bool TPaispeSaispe { get; set; }
        public bool TSaispeOptspe { get; set; }
        public bool TOptspeDouazeci { get; set; }
        public bool TDouazeciDouajdoi { get; set; }

        public bool NZeceDoispe { get; set; }
        public bool NDoispePaispe { get; set; }
        public bool NPaispeSaispe { get; set; }
        public bool NSaispeOptspe { get; set; }
        public bool NOptspeDouazeci { get; set; }
        public bool NDouazeciDouajdoi { get; set; }

        public CourtReservation() { }
        public CourtReservation(int? id)
        {
            this.Court = db.Courts.Find(id);
            this.Date = DateTime.Now;
        }
    }
}