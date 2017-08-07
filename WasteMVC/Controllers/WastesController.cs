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
using System.Text;

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

            ViewBag.Page = page;
            ViewData["CurrentFilter"] = currentFilter;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSort"] = sortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewData["WasteTypeSort"] = sortOrder == "wt_asc" ? "wt_desc" : "wt_asc"; ;
            ViewData["WeightSort"] = sortOrder == "weight_asc" ? "weight_desc" : "weight_asc"; ;
            ViewData["CostSort"] = sortOrder == "cost_asc" ? "cost_desc" : "cost_asc"; ;
            ViewData["SalePriceSort"] = sortOrder == "sale_asc" ? "sale_desc" : "sale_asc";

            _viewModel.Filter(currentFilter);
            _viewModel.Sort(sortOrder);
            await _viewModel.CreateView(page, 7);

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

            var waste = await _uow.GetRepository<Waste>()
                .Get(w => w.Id == id)
                .Include(w => w.WasteType)
                .Include(w => w.Partners)
                    .ThenInclude(pt => pt.Person)
                .FirstOrDefaultAsync();

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
            PopulateWasteTypesAndPersons(_new_waste, true);
            return View();
        }

        // POST: Wastes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DateTime,Weight,Cost,SalePrice,Id,WasteTypeId")] Waste waste, string[] selectecPartners, string[] id_persons, string[] values_Porcentaje)
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
                double porcentaje = double.Parse(values_Porcentaje[i++]) / 100.00;
                valuesIdPorcentage.Add(id, porcentaje);
            }
            waste.Partners = new HashSet<Partner>();
            Person _p;
            int _id = 0;
            double totalporcentage = 0.0;
            if (selectecPartners != null)
            {
                foreach (var item in selectecPartners)
                {
                    _id = int.Parse(item);
                    _p = _uow.GetRepository<Person>().Find(_id);
                    totalporcentage += valuesIdPorcentage[_id];
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
            bool band = true;
            if (waste.Partners.Count <= 0)
            {
                band = false;
                ModelState.AddModelError("", "Error::Validadndo Cantidad de Socio." +
                                            "\nNo se puede guardar los cambios." +
                                            "\nDebe seleccionar al menos un socio.");
            }
            if (totalporcentage != 1.00)
            {
                band = false;
                string errorMessage = "Error::Validando la Participacion de los Socios." +
                                            " No se puede guardar los cambios." +
                                            " Total Participacion: " +
                                            totalporcentage.ToString("P2") + "," +
                                            " Falta: " +
                                            (1.00 - totalporcentage).ToString("P2");
                ModelState.AddModelError("", errorMessage);
            }
            if (ModelState.IsValid && band == true)
            {
                if (_uow.GetRepository<Waste>().Add(waste))
                {
                    int count = await _uow.CommitAsync();
                    if (count > 0)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error::Creando El Desperdicio. No se puedo Guardar en la Base de Datos.");
                    }
                }
            }
            PopulateWasteTypesAndPersons(waste, true);
            return View(waste);
        }

        private void PopulateWasteTypesAndPersons(Waste waste, bool isNew)
        {
            var _wasteType = from wt in _uow.GetRepository<WasteType>().Get()
                             orderby wt.Description
                             select wt;
            ViewBag._wasteTypes = _wasteType.AsNoTracking().ToList();

            var _persons = from p in _uow.GetRepository<Person>().Get()
                           select p;
            if (isNew == false)
            {
                _persons = _persons.Include(p => p.Business);
            }
            _persons = _persons.OrderBy(p => p.LastName);
            ViewBag._persons = _persons.AsNoTracking().ToList();

            HashSet<AssignedPartnert> _partners = new HashSet<AssignedPartnert>();
            foreach (var item in _persons)
            {
                _partners.Add(
                    new AssignedPartnert
                    {
                        PersonId = item.Id,
                        FullName = item.FullName,
                        Assigned = item.BelongsToBusiness(waste.Id),
                        Procentage = item.BelongsToBusinessProcentage(waste.Id) * 100.00,
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

            Waste waste = await _uow.GetRepository<Waste>()
                                    .Get(w => w.Id == id)
                                    .Include(w => w.WasteType)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync();
            if (waste == null)
            {
                return NotFound();
            }
            PopulateWasteTypesAndPersons(waste, false);
            return View(waste);
        }

        // POST: Wastes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DateTime,Weight,Cost,SalePrice,Id,WasteTypeId")] Waste waste, string[] selectecPartners, string[] id_persons, string[] values_Porcentaje)
        {
            if (id != waste.Id)
            {
                return NotFound();
            }
            waste.WasteType = _uow.GetRepository<WasteType>().Find(waste.WasteTypeId);
            //Cargando Antiguos Valores de Socios
            HashSet<Partner> oldPartners = _uow.GetRepository<Waste>()
                                                .Get(w => w.Id == waste.Id)
                                                .Include(w => w.Partners)
                                                .AsNoTracking()
                                                .FirstOrDefault()
                                                .Partners;
            waste.Partners = new HashSet<Partner>();
            double totalporcentage = 0.0;
            if (selectecPartners != null)
            {
                //Asociando PersonId -> Porcentage
                Dictionary<int, double> valuesIdPorcentage = new Dictionary<int, double>();
                int i = 0;
                int personId = 0;
                foreach (var item in id_persons)
                {
                    personId = int.Parse(item);
                    double porcentaje = double.Parse(values_Porcentaje[i++]) / 100.00;
                    valuesIdPorcentage.Add(personId, porcentaje);
                }
                //Agregando Socios 
                foreach (var item in selectecPartners)
                {
                    personId = int.Parse(item);
                    totalporcentage += valuesIdPorcentage[personId];
                    waste.Partners.Add(new Partner
                    {
                        Percentage = valuesIdPorcentage[personId],
                        PersonId = personId,
                        Person = _uow.GetRepository<Person>().Find(personId),
                    });
                }
            }
            bool band = true;
            if (waste.Partners.Count <= 0)
            {
                band = false;
                ModelState.AddModelError("", "Error::Validadndo Cantidad de Socio." +
                                            "\nNo se puede guardar los cambios." +
                                            "\nDebe seleccionar al menos un socio.");
            }
            if (totalporcentage != 1.00)
            {
                band = false;
                string errorMessage = "::Error::Validando la Participacion de los Socios." +
                                            " No se puede guardar los cambios." +
                                            " Total Participacion: " +
                                            totalporcentage.ToString("P2") + "," +
                                            " Falta: " +
                                            (1.00 - totalporcentage).ToString("P2");
                ModelState.AddModelError("", errorMessage);
            }
            if (ModelState.IsValid && band == true)
            {
                try
                {
                    if (_uow.GetRepository<Waste>().Update(waste))
                    {
                        int count = await _uow.CommitAsync();
                        if (count > 0)
                        {
                            //Eliminando Los viejos Socios
                            foreach (var item in oldPartners)
                            {
                                _uow.GetRepository<Partner>().Delete(item.Id);
                            }
                            await _uow.CommitAsync();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Error::Actualizando El Desperdicio. No se puedo Guardar en la Base de Datos.");
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "Error::Actualizando El Desperdicio. No se puedo Guardar en la Base de Datos.");
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
            }
            PopulateWasteTypesAndPersons(waste, false);
            return View(waste);
        }

        // GET: Wastes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var waste = await _uow.GetRepository<Waste>()
                .Get(w => w.Id == id)
                .Include(w => w.WasteType)
                .Include(w => w.Partners)
                    .ThenInclude(pt => pt.Person)
                .FirstOrDefaultAsync();
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
            List<Partner> pts = await _uow.GetRepository<Partner>()
                                            .Get(p => p.WasteId == id)
                                            .ToListAsync();
            foreach (var item in pts)
            {
                _uow.GetRepository<Partner>().Delete(item.Id);
            }
            await _uow.CommitAsync();

            if (_uow.GetRepository<Waste>().Delete(id))
            {
                await _uow.CommitAsync();
            }
            return RedirectToAction("Index");
        }

        private bool WasteExists(int id)
        {
            return _uow.GetRepository<Waste>().Any(e => e.Id == id);
        }
    }
}
