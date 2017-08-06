using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WasteMVC.Data;

namespace WasteMVC.Models.WastesView
{
    public class WastesIndex
    {
        private readonly UnitOfWork<SystemContext> uow = null;
        public IQueryable<Waste> Wastes { get; set; }
        public IEnumerable<Partner> Patners { get; set; }
        public Data.PaginatedList<Waste> View { get; set; }

        public WastesIndex(SystemContext systemContext)
        {
            uow = new UnitOfWork<SystemContext>(systemContext);
            Wastes = uow.GetRepository<Waste>()
                .Get()
                .Include(w => w.WasteType)
                .AsNoTracking();
        }

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

        internal async Task<bool> CreateView(int? page, int cant)
        {
            this.View = await PaginatedList<Waste>.CreateAsync(Wastes, page ?? 1, cant);
            cant = View.Count;
            if (cant > 0)
            {
                return true;
            }
            return false;           
        }

        internal void GetPartners(int id)
        {
            Patners = uow.GetRepository<Waste>().Get()
                         .Where(w => w.Id == id)
                         .Include(w => w.Partners)
                             .ThenInclude(p => p.Person)
                         .FirstOrDefault()
                         .Partners;
        }
    }
}
