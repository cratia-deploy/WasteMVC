namespace WasteMVC.Models
{
    public class Partner : EntityBase
    {
        public Person Person { get; set; }
        public double Percentage { get; set; }
        public Waste Waste { get; set; }
    }
}