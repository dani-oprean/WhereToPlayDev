using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WhereToPlay.Models.DB
{
    [Serializable]
    public class ReservationTime
    {
        public ReservationTime()
        {
            this.Reservations = new List<Reservation>();
        }

        [Key]
        public int IDReservationTime { get; set; }

        [Required]
        [StringLength(5)]
        public string Hours { get; set; }

        public virtual List<Reservation> Reservations { get; set; } 
    }
}