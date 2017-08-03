using System.ComponentModel.DataAnnotations;

namespace WasteMVC.Models
{
    public class Person : EntityBase
    {
        [Display(Name ="Nombre")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }
        [Display(Name = "Apellido",Description ="Apellido")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }
    }
}