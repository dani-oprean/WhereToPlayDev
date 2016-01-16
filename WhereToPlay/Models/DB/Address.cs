using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WhereToPlay.Models.DB
{
    public class Address
    {
        public Address()
        {
            this.Courts = new List<Court>();
        }

        [Key]
        public int IDAddress { get; set; }
        
        [StringLength(50)]
        [Required]
        public string AddressStreet { get; set; }

        [StringLength(50)]
        public string AddressNumber { get; set; }

        [StringLength(50)]
        [Required]
        public string AddressCity { get; set; }

        [StringLength(50)]
        [Required]
        public string AddressCounty { get; set; }

        public virtual List<Court> Courts { get; set; }
    }
}