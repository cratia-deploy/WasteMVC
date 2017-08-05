using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WasteMVC.Models
{
    public class Person : EntityBase
    {
        [Display(Name = "Nombre")]
        [DataType(DataType.Text)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Nombre no puede estar vacio. [Required]")]
        public string FirstName { get; set; }

        [Display(Name = "Apellido", Description = "Apellido")]
        [DataType(DataType.Text)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Apellido no puede estar vacio. [Required]")]
        public string LastName { get; set; }

        public ICollection<Partner> Business { get; set; }

        [Display(Name ="Nombre del Socio")]
        public string FullName
        {
            get
            {
                return this.LastName + ", " + this.FirstName;
            }
        }
    }
}