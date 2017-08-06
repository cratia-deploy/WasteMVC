using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WasteMVC.Models.WastesView
{
    public class WastesIndex
    {
        public IQueryable<Waste> Wastes { get; set; }
        public IEnumerable<Partner> Patners { get; set; }
        public Data.PaginatedList<Waste> View { get; set; }
    }
}
