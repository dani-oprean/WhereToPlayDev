using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WhereToPlay.Models.DB
{
    [Serializable]
    public class Reservation
    {
        public Reservation()
        {
            this.Hidden = false;
        }

        [Key]
        public int IDReservation { get; set; }

        [Column(TypeName = "datetime2")]
        [Required]
        public DateTime ReservationDate { get; set; }

        [ForeignKey("User")]
        [Required]
        public int UserID { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Court")]
        [Required]
        public int CourtID { get; set; }
        public virtual Court Court { get; set; }

        [ForeignKey("ReservationTime")]
        [Required]
        public int ReservationTimeID { get; set; }
        public virtual ReservationTime ReservationTime { get; set; }

        public bool Hidden { get; set; }
    }
}