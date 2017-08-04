using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WasteMVC.Models
{
    public class Waste : EntityBase
    {
        public DateTime DateTime { get; set; } = DateTime.Now;
        public WasteType WasteType { get; set; }
        public double Weight { get; set; } = 0.0;
        public double Cost { get; set; } = 0.0;
        public double SalePrice { get; set; } = 0.0;
        public HashSet<Partner> Partners { get; set; }
    }
}