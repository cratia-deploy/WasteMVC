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
        public IQueryable<Partner> Partners { get; private set; }

        public List<int> WastesID { get; private set; } = null;
        public string WasteType { get; private set; } = string.Empty;
        public DateTime DateTime { get; set; } = DateTime.MinValue.Date;

        [Required]
        [Display(Name = "Costo Final")]
        [DataType(DataType.Currency)]
        public double? Cost2 { get; set; } = null;
        [Required]
        [Display(Name = "Precio Final")]
        [DataType(DataType.Currency)]
        public double? SalePrice2 { get; set; } = null;
        [Required]
        [Display(Name = "Merma [Kg.]")]
        public double? Decrease { get; set; } = null;

        public HomeEditView()
        { }

        public HomeEditView(SystemContext context, int? partnersID, string day = "", string wasteType = "")
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
            Wastes = uow.GetRepository<Waste>()
                        .Get(w => w.DateTime.Date == DateTime.Date)
                        .Include(w => w.WasteType)
                        .AsNoTracking();
            if (wasteType != "")
            {
                Wastes = Wastes.Where(w => w.WasteType.Description == wasteType)
                                .AsNoTracking();
                WasteType = wasteType;
            }
            WastesID = Wastes.Select(w => w.Id).ToList();
            if (partnersID != null)
            {
                Partners = uow.GetRepository<Partner>()
                                .Get(p => p.WasteId == partnersID.Value)
                                .Include(p => p.Person)
                                .AsNoTracking();
            }
            Waste item = Wastes.LastOrDefault();
            if (item != null)
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

        internal void SetWastesID(int[] wastesID)
        {
            WastesID = new List<int>(wastesID);
        }

        public async Task CreateView(int pageIndex, int pageSize)
        {
            View = await PaginatedList<Waste>.CreateAsync(Wastes, pageIndex, pageSize);
        }

        internal async Task<int> Edit(SystemContext context)
        {
            uow = new UnitOfWork<SystemContext>(context);
            List<Waste> data = new List<Waste>();
            Wastes = uow.GetRepository<Waste>()
                        .Get(w => w.DateTime.Date == DateTime.Date)
                        .Where(w => w.WasteType.Description == WasteType)
                        .Include(w => w.WasteType)
                        .AsNoTracking();
            await CreateView(1, 4); //Falta Definir el Ambito PageSize (Controlador+ View)
            foreach (var item in WastesID)
            {
                data.Add(await uow.GetRepository<Waste>().FindAsync(w => w.Id == item));
            }
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
