﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WasteMVC.Models
{
    public class Waste : EntityBase
    {
        [Required]
        [DataType(DataType.Date, ErrorMessage = "Tipo de dato debe ser una Fecha (dd/mm/yyyy)")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy (hh: mm:ss tt)}")]
        [Display(Name = "Fecha")]
        public DateTime DateTime { get; set; } = DateTime.Now;

        [Display(Name = "Tipo de Desperdicio")]
        public WasteType WasteType { get; set; }
        public int WasteTypeId { get; set; }

        [Required]
        [Display(Name = "Peso [Kg.]")]
        [DisplayFormat(DataFormatString = "{0:N2} Kg.")]
        public double Weight { get; set; } = 0.0;

        [Display(Name = "Percio Costo [BsF.]")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public double? Cost { get; set; } = null;

        [Display(Name = "Precio Venta [BsF.]")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public double? SalePrice { get; set; } = null;

        public HashSet<Partner> Partners { get; set; }

        [Display(Name = "Costo Final [BsF.]")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public double? Cost2 { get; set; } = null;

        [Display(Name = "Precio Final [BsF.]")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public double? SalePrice2 { get; set; } = null;

        [Display(Name = "Merma [Kg.]")]
        [DisplayFormat(DataFormatString = "{0:N2} Kg.")]
        public double? Decrease { get; set; } = null;

    }
}