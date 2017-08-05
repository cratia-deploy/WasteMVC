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

        /// <summary>
        /// Metodo que permite averiguar si una persona pertene a un negocio _wastesdId
        /// </summary>
        /// <param name="_wastedId">_wastedId --> Identificador del Negocio</param>
        /// <returns>bool:true: Si es Socio y bool:false Si no es Socio</returns>
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

        /// <summary>
        /// Metodo que permite averiguar el procentaje de participacion de una persona que pertene a un negocio _wastesdId
        /// </summary>
        /// <param name="_wastedId">_wastedId --> Identificador del Negocio</param>
        /// <returns>double:Porcentage y 0.00  Si no es Socio</returns>
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