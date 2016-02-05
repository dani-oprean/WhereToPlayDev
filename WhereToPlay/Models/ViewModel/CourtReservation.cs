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

        public bool NZeceDoispe { get; set; }
        public bool NDoispePaispe { get; set; }
        public bool NPaispeSaispe { get; set; }
        public bool NSaispeOptspe { get; set; }
        public bool NOptspeDouazeci { get; set; }
        public bool NDouazeciDouajdoi { get; set; }
    }
}