using System.ComponentModel.DataAnnotations;

namespace WasteMVC.Models
{
    public class Partner : EntityBase
    {
        public Person Person { get; set; }

        [DisplayFormat(DataFormatString = "{0:P2}")]
        public double Percentage { get; set; }

        public Waste Waste { get; set; }
    }
}