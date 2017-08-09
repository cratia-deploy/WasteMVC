using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WasteMVC.Data;

namespace WasteMVC.Models.HomeView
{
    public class HomeDetailsView
    {
        private UnitOfWork<SystemContext> uow = null;

        public IQueryable<Waste> Wastes { get; private set; } = null;
        public PaginatedList<Waste> View { get; set; }
        public IQueryable<Partner> Partners { get; private set; }

        public List<int> WastesID { get; private set; } = null;
        public string WasteType { get; private set; } = string.Empty;
        public DateTime DateTime { get; private set; } = DateTime.MinValue.Date;

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

        public HomeDetailsView()
        { }

        public HomeDetailsView(SystemContext context, int? partnersID, string day = "", string _WasteType = "")
        {
            uow = new UnitOfWork<SystemContext>(context);
            SetDateTime(day);
            SetWastes(this.DateTime, _WasteType);
            SetWastesID();
            SetPartners(partnersID);
            if (Wastes != null)
            {
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
        }

        private void SetWastesID()
        {
            if (Wastes != null)
            {
                WastesID = Wastes.Select(w => w.Id).ToList();
            }
        }

        private void SetPartners(int? partnersID)
        {
            if (partnersID != null)
            {
                Partners = uow.GetRepository<Partner>()
                                .Get(p => p.WasteId == partnersID.Value)
                                .Include(p => p.Person)
                                .AsNoTracking();
            }
        }

        private void SetWastes(DateTime _DateTime, string _wasteType)
        {
            if (uow.GetRepository<Waste>().Any(w => w.DateTime.Date == _DateTime.Date))
            {
                Wastes = uow.GetRepository<Waste>()
                            .Get(w => w.DateTime.Date == _DateTime.Date)
                            .Include(w => w.WasteType)
                            .AsNoTracking();
                if ((_wasteType != "") && (Wastes.Any(w => w.WasteType.Description == _wasteType)))
                {
                    Wastes = Wastes.Where(w => w.WasteType.Description == _wasteType)
                                    .AsNoTracking();
                    WasteType = _wasteType;
                }
            }
            else
            {
                Wastes = null;
            }
        }

        private void SetDateTime(string day)
        {
            if (day != null && day != "" && day.Length == 10)
            {
                string[] values = day.Split(new char[] { '-', '/', '.' }, 3);
                if (values.Length == 3)
                {
                    int _day = int.Parse(values[0]);
                    int _month = int.Parse(values[1]);
                    int _year = int.Parse(values[2]);
                    DateTime = new DateTime(_year, _month, _day);
                    SetWastes(this.DateTime, this.WasteType);
                }
                else
                {
                    DateTime = DateTime.Now.Date;
                }
            }
        }

        internal void SetWastesID(int[] wastesID)
        {
            WastesID = new List<int>(wastesID);
        }

        public async Task CreateView(int pageIndex)
        {
            if (Wastes != null)
            {
                View = await PaginatedList<Waste>.CreateAsync(Wastes, pageIndex, this.Wastes.Count());
            }
        }
    }
}