using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WhereToPlay.Models.DB
{
    public class User
    {
        public User()
        {
            this.Hidden = false;
            this.Reservations = new List<Reservation>();
            this.Courts = new List<Court>();
            this.UserGroup = new UserGroup();
        }

        [Key]
        public int IDUser { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Campul Nume Utilizator trebuie completat!")]
        [Display(Name ="Nume utilizator*")]
        public string UserName { get; set; }

        [StringLength(50)]
        [Display(Name = "Numar de telefon")]
        public string UserPhone { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Campul Adresa de email trebuie completat!")]
        [Display(Name = "Adresa de email*")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Campul Parola trebuie completat!")]
        [Display(Name = "Parola*")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        [Required(ErrorMessage = "Campul Confirma Parola trebuie completat!")]
        [Display(Name = "Confirma Parola*")]
        [DataType(DataType.Password)]
        [NotMapped]
        [System.ComponentModel.DataAnnotations.Compare("UserPassword", ErrorMessage = "Valoarea din campul Confirmati Parola trebuie sa fie egala cu valoarea din campul Parola!")]
        public string UserPasswordConfirm { get; set; }

        [Required]
        public string UserPasswordSalt { get; set; }

        [StringLength(50)]
        [Display(Name ="Nume si prenume")]
        public string UserFullName { get; set; }

        [Required]
        [ForeignKey("UserGroup")]
        [HiddenInput(DisplayValue = false)]
        public int UserGroupID { get; set; }
        public virtual UserGroup UserGroup { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool Hidden { get; set; }

        [NotMapped]
        [Display(Name = "Sunt proprietar de terenuri!")]
        public bool IAmOwner { get; set; }

        public virtual List<Reservation> Reservations { get; set; }
        public virtual List<Court> Courts { get; set; }
    }
}