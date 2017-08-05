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
        UnitOfWork<SystemContext> _uow = null;

        public WastesController(SystemContext context)
        {
            _context = context;
            _uow = new UnitOfWork<SystemContext>(context);
        }

        // GET: Wastes
        public IActionResult Index(int? id)
        {
            var _viewModel = new WastesIndex()
            {
                Wastes = _uow.GetRepository<Waste>()
                                        .Get()
                                        .Include(w => w.WasteType)
                                        .AsNoTracking()
                                        .ToList()
            };
            if (id != null)
            {
                ViewData["PartnersID"] = id.Value;
                _viewModel.Patners = _uow.GetRepository<Waste>().Get()
                                                .Where(w => w.Id == id.Value)
                                                .Include(w => w.Partners)
                                                    .ThenInclude(p => p.Person)
                                                .FirstOrDefault()
                                                .Partners;

            }
            return View(_viewModel);
        }

        // GET: Wastes/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waste = _context.Wastes
                .SingleOrDefault(m => m.Id == id);
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
        public IActionResult Create([Bind("DateTime,Weight,Cost,SalePrice,Id,WasteTypeId")] Waste waste, string[] selectecPartners)
        {
            waste.WasteType = _uow.GetRepository<WasteType>()
                                        .Get(wt => wt.Id == waste.WasteTypeId)
                                        .FirstOrDefault();
            waste.Partners = new HashSet<Partner>();
            Person _p;
            int _id = -1;
            if (selectecPartners != null)
            {
                foreach (var item in selectecPartners)
                {
                    if (int.TryParse(item, out _id))
                    {
                        _p = _uow.GetRepository<Person>().Find(_id);
                        if (_p != null)
                        {
                            waste.Partners.Add(
                                new Partner
                                {
                                    Person = _p,
                                    Percentage = 0.50,
                                });
                        }
                    }
                }
            }
            if (ModelState.IsValid)
            {
                if (_uow.GetRepository<Waste>().Add(waste))
                {
                    _uow.Commit();
                    return RedirectToAction("Index");
                }
            }
            PopulateWasteTypesAndPersons(waste);
            return View(waste);
        }

        private void PopulateWasteTypesAndPersons(Waste _waste)
        {
            var _wasteType = from wt in _uow.GetRepository<WasteType>().Get()
                             orderby wt.Description
                             select wt;
            ViewBag._wasteTypes = _wasteType.AsNoTracking().ToList();

            var _persons = from p in _uow.GetRepository<Person>().Get()
                           orderby p.FirstName
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
                        Assigned = item.BelongsToBusiness(_waste.Id),
                        Procentage = item.BelongsToBusinessProcentage(_waste.Id),
                    });
            }
            ViewBag._partners = _partners;
        }

        // GET: Wastes/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<Waste> _wastes = _uow.GetRepository<Waste>()
                                            .Get()
                                            .Where(w => w.Id == id)
                                            .Include(w => w.WasteType)
                                            .Include(w => w.Partners)
                                                .ThenInclude(p => p.Person)
                                            .ToList();
            Waste _waste = _wastes.FirstOrDefault();
            if (_waste == null)
            {
                return NotFound();
            }
            PopulateWasteTypesAndPersons(_waste);
            return View(_waste);
        }

        // POST: Wastes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("WasteTypeId,DateTime,Weight,Cost,SalePrice,Id")] Waste waste, string[] selectecPartners)
        {
            if (id != waste.Id)
            {
                return NotFound();
            }

            waste.WasteType = _uow.GetRepository<WasteType>()
                                        .Get(wt => wt.Id == waste.WasteTypeId)
                                        .FirstOrDefault();
            List<Partner> x = _uow.GetRepository<Partner>()
                                        .Get(p => p.WasteId == waste.Id)
                                        .ToList();


            if (selectecPartners != null)
            {
                int _PersonId = 0;
                waste.Partners = new HashSet<Partner>(x);
                HashSet<Partner> _oldPartners = new HashSet<Partner>(x);
                foreach (var item in selectecPartners)
                {
                    if (int.TryParse(item, out _PersonId))
                    {
                        if (waste.BelongsToBusiness(_PersonId))
                        {
                            //Actualizar porcentaje
                            waste.Partners.Where(p => p.PersonId == _PersonId).FirstOrDefault().Percentage = 0.33;
                        }
                        else
                        {
                            //Agregar Nuevo Socio
                            waste.Partners.Add(
                                new Partner
                                {
                                    Person = _uow.GetRepository<Person>().Find(_PersonId),
                                    Percentage = 0.77,
                                }
                            );
                        }
                    }
                }
                //Eliminando los viejos socios
                //_oldPartners.ExceptWith(waste.Partners);
                //waste.Partners.ExceptWith(_oldPartners);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (_uow.GetRepository<Waste>().Update(waste))
                    {
                        _uow.Commit();
                    }
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
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waste =  _context.Wastes
                .SingleOrDefault(m => m.Id == id);
            if (waste == null)
            {
                return NotFound();
            }

            return View(waste);
        }

        // POST: Wastes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var waste =  _context.Wastes.SingleOrDefault(m => m.Id == id);
            _context.Wastes.Remove(waste);
             _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool WasteExists(int id)
        {
            return _context.Wastes.Any(e => e.Id == id);
        }
    }
}
