using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WhereToPlay.Models.DB
{
    public class User
    {
        public User()
        {
            this.Hidden = false;
            this.Reservations = new List<Reservation>();
            this.Courts = new List<Court>();
        }

        [Key]
        public int IDUser { get; set; }

        [StringLength(50)]
        [Required]
        public string UserName { get; set; }

        [StringLength(50)]
        public string UserPhone { get; set; }

        [StringLength(50)]
        [Required]
        public string UserEmail { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string UserPassword { get; set; }

        [Required]
        public string UserPasswordSalt { get; set; }

        [StringLength(50)]
        public string UserFullName { get; set; }

        [Required]
        [ForeignKey("UserGroup")]
        public int UserGroupID { get; set; }
        public virtual UserGroup UserGroup { get; set; }

        public bool Hidden { get; set; }

        public virtual List<Reservation> Reservations { get; set; }
        public virtual List<Court> Courts { get; set; }
    }
}