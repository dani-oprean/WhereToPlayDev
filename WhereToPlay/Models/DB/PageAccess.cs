using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WhereToPlay.Models.DB
{
    public class PageAccess
    {
        [Key]
        public int IDPageAccess { get; set; }

        [Required]
        [ForeignKey("UserGroup")]
        public int UserGroupID { get; set; }
        public virtual UserGroup UserGroup { get; set; }

        [Required]
        [StringLength(50)]
        public string PageName { get; set; }
    }
}