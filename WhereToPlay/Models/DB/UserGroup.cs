using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WhereToPlay.Models.DB
{
    public class UserGroup
    {
        public UserGroup()
        {
            this.PageAccesses = new List<PageAccess>();
            this.Users = new List<User>();
        }

        [Key]
        public int IDUserGroup { get; set; }

        [Required]
        [StringLength(50)]
        public string UserGroupName { get; set; }

        public virtual List<PageAccess> PageAccesses { get; set; }
        public virtual List<User> Users { get; set; }
    }
}