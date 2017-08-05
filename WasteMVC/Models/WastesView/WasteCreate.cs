using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WasteMVC.Models.WastesView
{
    public class WasteCreate
    {
        public IEnumerable<WasteType> WasteTypes { get; set; }
        public IEnumerable<Person> Persons { get; set; }
        public Waste Waste { get; set; }
    }

    public class AssignedPartnert
    {
        public int PersonId { get; set; }
        public string FullName { get; set; }
        public double Procentage { get; set; }
        public bool Assigned { get; set; }
    }
}
