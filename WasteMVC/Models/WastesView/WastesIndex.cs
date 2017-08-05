using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WasteMVC.Models.WastesView
{
    public class WastesIndex
    {
        public IEnumerable<Waste> Wastes { get; set; }
        public IEnumerable<Partner> Patners { get; set; }
    }
}
