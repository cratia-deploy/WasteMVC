using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WasteMVC.Data;

namespace WasteMVC.Models.IndexView
{
    public class HomeIndexView
    {
        private readonly UnitOfWork<SystemContext> _uow = null;
        public DateTime DayStart { get; private set; }
        public DateTime DayEnd { get; private set; }
        public IQueryable<Waste> Waste { get; private set; } = null;
        public PaginatedList<Waste> View { get; private set; } = null;
        public List<ViewX> Result { get; private set; } = null;

        internal HomeIndexView(SystemContext _context)
        {
            DayStart = DateTime.Now.AddDays(-6).Date;
            DayEnd = DateTime.Now.Date;
            _uow = new UnitOfWork<SystemContext>(_context);
            Waste = _uow.GetRepository<Waste>().Get()
                        .Where(w => w.DateTime.Date <= DayEnd.Date)
                        .Where(w => w.DateTime.Date >= DayStart.Date);
            Result = new List<ViewX>();
            for (int i = 0; i < 7; i++)
            {
                Result.Add(new ViewX(Waste, DayStart.AddDays(i).Date));
            }
        }

        internal async Task<bool> CreateView(int? page, int cant)
        {
            this.View = await PaginatedList<Waste>.CreateAsync(Waste, page ?? 1, cant);
            cant = View.Count;
            if (cant > 0)
            {
                return true;
            }
            return false;
        }
    }

    public class ViewX
    {
        public DateTime DayofWeek { get; private set; } = DateTime.MinValue.Date;
        public double TotalWeight { get; private set; } = 0.00;
        public double TotalCost { get; private set; } = 0.00;
        public double TotalSale { get; private set; } = 0.00;
        public List<SubTotalByWasteType> SubTotal { get; private set; } = null;

        internal ViewX(IQueryable<Waste> _wastes, DateTime _dayofWeek)
        {
            if (_wastes != null)
            {
                DayofWeek = _dayofWeek.Date;
                /////
                ///// Calculando Subtotales por WasteType
                /////
                var _subTotal = from wt in _wastes
                                where wt.DateTime.Date == _dayofWeek.Date
                                group wt by wt.WasteType.Description into g
                                select new SubTotalByWasteType()
                                {
                                    Key = g.Key,
                                    SubTotalWeight = Math.Round(g.Sum(w => w.Weight), 2),
                                    SubTotalCost = Math.Round(g.Average(w => w.Cost.Value), 2),
                                    SubTotalSale = Math.Round(g.Average(w => w.SalePrice.Value), 2),
                                };
                if (_subTotal != null)
                {
                    SubTotal = _subTotal.ToList();
                }
                /////
                ///// Calculando Totalizaciones
                /////
                TotalWeight = _wastes
                              .Where(w => w.DateTime.Date == _dayofWeek.Date)
                              .Select(w => w.Weight)
                              .ToList()
                              .Sum();
                TotalCost = _wastes
                             .Where(w => w.DateTime.Date == _dayofWeek.Date)
                             .Select(w => w.Cost.Value)
                             .ToList()
                             .Sum();
                TotalSale = _wastes
                              .Where(w => w.DateTime.Date == _dayofWeek.Date)
                              .Select(w => w.SalePrice.Value)
                              .ToList()
                              .Sum();   
                /////
                /////
            }
        }
    }

    public class SubTotalByWasteType
    {
        public string Key { get; set; } = string.Empty;
        public double SubTotalWeight { get; set; } = 0.0;
        public double SubTotalCost { get; set; } = 0.0;
        public double SubTotalSale { get; set; } = 0.0;
    }
}
