using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WhereToPlay.Models.DB
{
    public class Court
    {
        public Court()
        {
            this.Reservations = new List<Reservation>();
            this.Hidden = false;
        }

        [Key]
        public int IDCourt { get; set; }

        [StringLength(50)]
        [Required]
        public string CourtName { get; set; }

        [ForeignKey("Sport")]
        [Required]
        public int SportID { get; set; }
        public virtual Sport Sport { get; set; }

        [ForeignKey("Address")]
        [Required]
        public int AddressID { get; set; }
        public virtual Address Address { get; set; }

        [Range(0,300)]
        public int Length { get; set; }

        [Range(0, 300)]
        public int Width { get; set; }

        [StringLength(50)]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string EmailAddress { get; set; }

        [ForeignKey("User")]
        [Required]
        public int CreateUserID { get; set; }
        public virtual User User { get; set; }

        [Required]
        public int SessionPrice { get; set; }

        public bool Hidden { get; set; }

        public virtual List<Reservation> Reservations { get; set; }
    }
}