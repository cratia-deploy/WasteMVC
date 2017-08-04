using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WasteMVC.Models
{
    public class WasteType : EntityBase
    {
        public string Description { get; set; }
        public ICollection<Waste> Wastes { get; set; }
    }
}