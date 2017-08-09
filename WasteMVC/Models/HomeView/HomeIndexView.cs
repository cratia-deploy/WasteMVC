using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WasteMVC.Data;

namespace WasteMVC.Models.IndexView
{
    public class HomeIndexView
    {
        private readonly UnitOfWork<SystemContext> _uow = null;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Inicio: ")]
        public DateTime DayStart { get; private set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Fin: ")]
        public DateTime DayEnd { get; private set; }

        public IQueryable<Waste> Waste { get; private set; } = null;
        public PaginatedList<ViewX> View { get; private set; } = null;
        public List<ViewX> Result { get; private set; } = null;

        internal HomeIndexView(SystemContext _context, string _DayStart = "", string _DayEnd = "")
        {
            SetDayStart(_DayStart);
            SetDayEnd(_DayEnd);
            if (DayStart > DayEnd)
            {
                DateTime temp = DayEnd.Date;
                DayEnd = DayStart.Date;
                DayStart = temp.Date;
            }
            _uow = new UnitOfWork<SystemContext>(_context);
            Waste = _uow.GetRepository<Waste>().Get()
                        .Where(w => w.DateTime.Date <= DayEnd.Date)
                        .Where(w => w.DateTime.Date >= DayStart.Date);
            Result = new List<ViewX>();

            DateTime _day = DayStart.Date;
            while (_day <= DayEnd)
            {
                Result.Add(new ViewX(Waste, _day));
                _day = _day.AddDays(1).Date;
            }
        }

        private void SetDayEnd(string day)
        {
            if (day != null && day != "" && day.Length == 10)
            {
                string[] values = day.Split(new char[] { '-', '/', '.' }, 3);
                if (values.Length == 3)
                {
                    int _day = int.Parse(values[2]);
                    int _month = int.Parse(values[1]);
                    int _year = int.Parse(values[0]);
                    DayEnd = new DateTime(_year, _month, _day);
                }
                else
                {
                    DayEnd = DateTime.Now.Date;
                }
            }
            else
            {
                DayEnd = DateTime.Now.Date;
            }
        }

        private void SetDayStart(string day)
        {
            if (day != null && day != "" && day.Length == 10)
            {
                string[] values = day.Split(new char[] { '-', '/', '.' }, 3);
                if (values.Length == 3)
                {
                    int _day = int.Parse(values[2]);
                    int _month = int.Parse(values[1]);
                    int _year = int.Parse(values[0]);
                    DayStart = new DateTime(_year, _month, _day);
                }
                else
                {
                    DayStart = DateTime.Now.AddDays(-6).Date;
                }
            }
            else
            {
                DayStart = DateTime.Now.AddDays(-6).Date;
            }
        }

        internal bool CreateView(int? page)
        {
            this.View = PaginatedList<ViewX>.CreateAsync(Result, page ?? 1, 7);
            int cant = View.Count;
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
