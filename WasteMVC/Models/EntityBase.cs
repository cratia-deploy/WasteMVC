using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WasteMVC.Models
{
    /// <summary>
    /// Clase Base Entidades
    /// </summary>
    public class EntityBase
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy (hh:mm:ss tt)}", ApplyFormatInEditMode = true)]
        [DisplayName("Creado En")]
        public DateTime Created_At { get; set; } = DateTime.MinValue;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy (hh:mm:ss tt)}", ApplyFormatInEditMode = true)]
        [DisplayName("Actualizado En")]
        public DateTime Updated_At { get; set; } = DateTime.MinValue;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy (hh:mm:ss tt)}", ApplyFormatInEditMode = true)]
        [DisplayName("Eliminado En")]
        public DateTime Deleted_At { get; set; } = DateTime.MinValue;

        //[DataType(DataType.Currency,ErrorMessage ="Mensaje de Error")]
        //[DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString ="", NullDisplayText ="None" )]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Mensaje de Error")]
        //[DisplayName(displayName:"DisplayName")]
        //[StringLength(60, MinimumLength = 4)]
        //[Range(type:typeof(double),minimum:0.00,maximum:1000.00,ErrorMessage ="Valor debe estar entre A y B")]
        //[ScaffoldColumn()] --> specify fields for hiding from editor forms.
        //[ReadOnly(true)] ---> ??
    }

}