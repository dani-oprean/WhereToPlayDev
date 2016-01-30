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
            set { date = value; }
        }
        public bool TZeceDoispe { get; set; }
        public bool TDoispePaispe { get; set; }
        public bool TPaispeSaispe { get; set; }
        public bool TSaispeOptspe { get; set; }
        public bool TOptspeDouazeci { get; set; }
        public bool TDouazeciDouajdoi { get; set; }

        public CourtReservation() { }
        public CourtReservation(int? id)
        {
            this.Court = db.Courts.Find(id);
            this.Date = DateTime.Now;
        }
    }
}