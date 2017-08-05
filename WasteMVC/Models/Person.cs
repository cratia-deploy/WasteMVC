using System;
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

        [Display(Name = "Nombre del Socio")]
        public string FullName
        {
            get
            {
                return this.LastName + ", " + this.FirstName;
            }
        }

        internal bool BelongsToBusiness(int _wastedId)
        {
            if (this.Business == null || _wastedId <= 0)
            {
                return false;
            }
            foreach (var item in Business)
            {
                if (item.WasteId == _wastedId)
                {
                    return true;
                }
            }
            return false;
        }

        internal double BelongsToBusinessProcentage(int _wastedId)
        {
            if (this.Business == null || _wastedId <= 0)
            {
                return 0.00;
            }
            foreach (var item in Business)
            {
                if (item.WasteId == _wastedId)
                {
                    return item.Percentage;
                }
            }
            return 0.00;
        }
    }
}