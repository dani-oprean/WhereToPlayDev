﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WhereToPlay.Models.DB
{
    [Serializable]
    public class Address
    {
        public Address()
        {
            this.Courts = new List<Court>();
        }

        [Key]
        public int IDAddress { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Campul Strada trebuie completat!")]
        public string AddressStreet { get; set; }

        [StringLength(50)]
        public string AddressNumber { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Campul Oras trebuie completat!")]
        public string AddressCity { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Campul Judet trebuie completat!")]
        public string AddressCounty { get; set; }

        public virtual List<Court> Courts { get; set; }
    }
}