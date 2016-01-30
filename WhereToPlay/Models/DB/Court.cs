using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WhereToPlay.Models.DB
{
    [Serializable]
    public class Court
    {
        public Court()
        {
            this.Reservations = new List<Reservation>();
            this.Hidden = false;
            this.Length = 50;
            this.Width = 25;
        }

        [Key]
        public int IDCourt { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Campul Denumire trebuie completat!")]
        [Display(Name = "Denumire")]
        public string CourtName { get; set; }

        [ForeignKey("Sport")]
        [Required(ErrorMessage = "Campul Sport trebuie completat!")]
        public int SportID { get; set; }
        public virtual Sport Sport { get; set; }

        [ForeignKey("Address")]
        [Required]
        public int AddressID { get; set; }
        public virtual Address Address { get; set; }

        [Range(0, 300, ErrorMessage = "Campul Lungime trebuie sa fie intre 0 si 300")]
        [Display(Name = "Lungime")]
        public int Length { get; set; }

        [Range(0, 300, ErrorMessage = "Campul Latime trebuie sa fie intre 0 si 300")]
        [Display(Name = "Latime")]
        public int Width { get; set; }

        [StringLength(50)]
        [Display(Name = "Numar telefon")]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        [Display(Name = "Adresa email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Acest camp trebuie sa aiba forma unei adrese de email!")]
        public string EmailAddress { get; set; }

        [ForeignKey("User")]
        [Required]
        public int CreateUserID { get; set; }
        public virtual User User { get; set; }

        [Required(ErrorMessage = "Campul Pret/Sesiune trebuie completat!")]
        [Display(Name = "Pret/Sesiune")]
        public int SessionPrice { get; set; }

        public bool Hidden { get; set; }

        public virtual List<Reservation> Reservations { get; set; }

        public string FileName1 { get; set; }
        public byte[] Content1 { get; set; }

        public string FileName2 { get; set; }
        public byte[] Content2 { get; set; }

        public string FileName3 { get; set; }
        public byte[] Content3 { get; set; }
    }
}