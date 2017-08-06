using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WasteMVC.Data;
using WasteMVC.Models;
using WasteMVC.Models.WastesView;

namespace WasteMVC.Controllers
{
    public class WastesController : Controller
    {
        private readonly SystemContext _context = null;
        private readonly UnitOfWork<SystemContext> _uow = null;

        public WastesController(SystemContext context)
        {
            _context = context;
            _uow = new UnitOfWork<SystemContext>(context);
        }

        // GET: Wastes
        public async Task<IActionResult> Index(int? id, int? page, string sortOrder = "", string currentFilter = "")
        {
            var _viewModel = new WastesIndex(_context);

            if ((page == null) && (page.HasValue))
            {
                page = 1;
            }
            if (sortOrder == "")
            {
                sortOrder = "date_asc";
            }

            ViewData["CurrentFilter"] = currentFilter;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSort"] = sortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewData["WasteTypeSort"] = sortOrder == "wt_asc" ? "wt_desc" : "wt_asc"; ;
            ViewData["WeightSort"] = sortOrder == "weight_asc" ? "weight_desc" : "weight_asc"; ;
            ViewData["CostSort"] = sortOrder == "cost_asc" ? "cost_desc" : "cost_asc"; ;
            ViewData["SalePriceSort"] = sortOrder == "sale_asc" ? "sale_desc" : "sale_asc";

            _viewModel.Filter(currentFilter);
            _viewModel.Sort(sortOrder);
            await _viewModel.CreateView(page,10);

            if (id != null)
            {
                ViewData["PartnersID"] = id.Value;
                _viewModel.GetPartners(id.Value);
            }
            return View(_viewModel);
        }

        // GET: Wastes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waste = await _context.Wastes
                .SingleOrDefaultAsync(m => m.Id == id);
            if (waste == null)
            {
                return NotFound();
            }

            return View(waste);
        }

        // GET: Wastes/Create
        public IActionResult Create()
        {
            Waste _new_waste = new Waste();
            PopulateWasteTypesAndPersons(_new_waste);
            return View();
        }

        // POST: Wastes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DateTime,Weight,Cost,SalePrice,Id,WasteTypeId")] Waste waste, string[] selectecPartners, string [] id_persons, string [] values_Porcentaje)
        {
            waste.WasteType = _uow.GetRepository<WasteType>()
                                        .Get(wt => wt.Id == waste.WasteTypeId)
                                        .FirstOrDefault();

            //Asociando PersonId -> Porcentage
            Dictionary<int, double> valuesIdPorcentage = new Dictionary<int, double>();
            int i = 0;
            foreach (var item in id_persons)
            {
                int id = int.Parse(item);
                double porcentaje = double.Parse(values_Porcentaje[i++])/100.00;
                valuesIdPorcentage.Add(id, porcentaje);
            }
            //
            waste.Partners = new HashSet<Partner>();
            Person _p;
            int _id = 0;
            if (selectecPartners != null)
            {
                foreach (var item in selectecPartners)
                {
                    _id = int.Parse(item);
                    _p = _uow.GetRepository<Person>().Find(_id);
                    if (_p != null)
                    {
                        waste.Partners.Add(
                            new Partner
                            {
                                Person = _p,
                                Percentage = valuesIdPorcentage[_id],
                            });
                    }
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(waste);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            PopulateWasteTypesAndPersons(waste);
            return View(waste);
        }

        private void PopulateWasteTypesAndPersons(Waste Waste)
        {
            var _wasteType = from wt in _uow.GetRepository<WasteType>().Get()
                             orderby wt.Description
                             select wt;
            ViewBag._wasteTypes = _wasteType.AsNoTracking().ToList();

            var _persons = from p in _uow.GetRepository<Person>().Get()
                           orderby p.Id
                           select p;
            ViewBag._persons = _persons.AsNoTracking().ToList();

            HashSet<AssignedPartnert> _partners = new HashSet<AssignedPartnert>();
            foreach (var item in _persons)
            {
                _partners.Add(
                    new AssignedPartnert
                    {
                        PersonId = item.Id,
                        FullName = item.FullName,
                    });
            }
            ViewBag._partners = _partners;
        }

        // GET: Wastes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waste = await _context.Wastes.SingleOrDefaultAsync(m => m.Id == id);
            if (waste == null)
            {
                return NotFound();
            }
            return View(waste);
        }

        // POST: Wastes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DateTime,Weight,Cost,SalePrice,Id")] Waste waste)
        {
            if (id != waste.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(waste);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WasteExists(waste.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(waste);
        }

        // GET: Wastes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waste = await _context.Wastes
                .SingleOrDefaultAsync(m => m.Id == id);
            if (waste == null)
            {
                return NotFound();
            }

            return View(waste);
        }

        // POST: Wastes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var waste = await _context.Wastes.SingleOrDefaultAsync(m => m.Id == id);
            _context.Wastes.Remove(waste);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool WasteExists(int id)
        {
            return _context.Wastes.Any(e => e.Id == id);
        }
    }
}
