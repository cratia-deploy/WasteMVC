using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WasteMVC.Data;

namespace WasteMVC.Models.HomeView
{
    public class HomeEditView
    {
        private UnitOfWork<SystemContext> uow = null;
        public IQueryable<Waste> Wastes { get; private set; } = null;
        public PaginatedList<Waste> View { get; set; }
        public DateTime DateTime { get; set; } = DateTime.MinValue.Date;
        public IQueryable<Partner> Partners { get; private set; }

        [Required]
        [Display(Name = "Costo Final")]
        [DataType(DataType.Currency)]
        public double Cost2 { get; set; } = 0.00;
        [Required]
        [Display(Name = "Precio Final")]
        [DataType(DataType.Currency)]
        public double SalePrice2 { get; set; } = 0.00;
        [Required]
        [Display(Name = "Merma [Kg.]")]
        public double Decrease { get; set; } = 0.00;

        public string WasteType { get; set; } = string.Empty;

        public HomeEditView()
        { }

        public HomeEditView(SystemContext context, int? id, string day = "", string wasteType = "")
        {
            uow = new UnitOfWork<SystemContext>(context);
            if (day != "")
            {
                string[] values = day.Split('-');
                int _day = int.Parse(values[0]);
                int _month = int.Parse(values[1]);
                int _year = int.Parse(values[2]);
                DateTime = new DateTime(_year, _month, _day);
            }
            Wastes = uow.GetRepository<Waste>().Get(w => w.DateTime.Date == DateTime.Date).Include(w => w.WasteType);
            if (wasteType != "")
            {
                Wastes = Wastes.Where(w => w.WasteType.Description == wasteType);
                WasteType = wasteType;
            }
            if (id != null)
            {
                Partners = uow.GetRepository<Partner>().Get(p => p.WasteId == id.Value).Include(p => p.Person);
            }
            foreach (var item in Wastes)
            {
                if (item.Cost2.HasValue)
                {
                    Cost2 = item.Cost2.Value;
                }
                if (item.SalePrice2.HasValue)
                {
                    SalePrice2 = item.SalePrice2.Value;
                }
                if (item.Decrease.HasValue)
                {
                    Decrease = item.Decrease.Value;
                }
            }
        }

        public async Task CreateView(int pageIndex, int pageSize)
        {
            View = await PaginatedList<Waste>.CreateAsync(Wastes, pageIndex, pageSize);
        }

        internal async Task<int> Edit(SystemContext context)
        {
            uow = new UnitOfWork<SystemContext>(context);
            Wastes = uow.GetRepository<Waste>()
                    .Get(w => w.DateTime.Date == DateTime.Date)
                    .Include(w => w.WasteType)
                    .AsNoTracking();
            Wastes = Wastes.Where(w => w.WasteType.Description == WasteType)
                            .AsNoTracking();
            List<Waste> data = await Wastes.AsNoTracking().ToListAsync();
            foreach (var item in data)
            {
                item.SalePrice2 = this.SalePrice2;
                item.Cost2 = this.Cost2;
                item.Decrease = this.Decrease;
                uow.GetRepository<Waste>().Update(item);
            }
            return await uow.CommitAsync();
        }
    }
}
