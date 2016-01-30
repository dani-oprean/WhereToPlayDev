using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WhereToPlay.Models.DB
{
    [Serializable]
    public class Sport
    {
        public Sport()
        {
            this.Courts = new List<Court>();
        }

        [Key]
        public int IDSport { get; set; }

        [Required]
        [StringLength(50)]
        public string SportName { get; set; }

        public virtual List<Court> Courts { get; set; }
    }
}