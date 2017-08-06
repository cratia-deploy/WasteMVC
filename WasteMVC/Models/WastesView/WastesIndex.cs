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

        internal void Sort(string sortOrder)
        {
            switch (sortOrder)
            {
                case "date_asc":
                    Wastes = Wastes.OrderBy(w => w.DateTime);
                    break;
                case "wt_asc":
                    Wastes = Wastes.OrderBy(w => w.WasteType);
                    break;
                case "weight_asc":
                    Wastes = Wastes.OrderBy(w => w.Weight);
                    break;
                case "cost_asc":
                    Wastes = Wastes.OrderBy(w => w.Cost);
                    break;
                case "sale_asc":
                    Wastes = Wastes.OrderBy(w => w.SalePrice);
                    break;
                case "date_desc":
                    Wastes = Wastes.OrderByDescending(w => w.DateTime);
                    break;
                case "wt_desc":
                    Wastes = Wastes.OrderByDescending(w => w.WasteType);
                    break;
                case "weight_desc":
                    Wastes = Wastes.OrderByDescending(w => w.Weight);
                    break;
                case "cost_desc":
                    Wastes = Wastes.OrderByDescending(w => w.Cost);
                    break;
                case "sale_desc":
                    Wastes = Wastes.OrderByDescending(w => w.SalePrice);
                    break;
                default:
                    Wastes = Wastes.OrderBy(w => w.DateTime);
                    break;
            }
        }
    }
}
