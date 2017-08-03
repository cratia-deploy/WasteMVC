using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace WasteMVC.Models
{
    /// <summary>
    /// Clase Base Entidades
    /// </summary>
    public class EntityBase
    {
        [HiddenInput(DisplayValue = true)]
        public int Id { get; set; }
        [HiddenInput(DisplayValue = true)]
        [Editable(allowEdit:false)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Created_At { get; set; } = DateTime.MinValue;
        [Editable(allowEdit: false)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Updated_At { get; set; } = DateTime.MinValue;
        [Editable(allowEdit: false)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Deleted_At { get; set; } = DateTime.MinValue;
    }
    
}