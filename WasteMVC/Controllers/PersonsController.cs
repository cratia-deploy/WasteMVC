using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WasteMVC.Data;
using WasteMVC.Models;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration;

namespace WasteMVC.Controllers
{
    public class PersonsController : Controller
    {
        private readonly UnitOfWork<SystemContext> _uow = null;

        public PersonsController(SystemContext _context)
        {
            _uow = new UnitOfWork<SystemContext>(_context);
        }

        // GET: Persons
        public async Task<IActionResult> Index(string sortOrder = "firstname_asc", string searchString = "")
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["FirstNameSortParm"] = sortOrder == "firstname_desc" ? "firstname_asc" : "firstname_desc";
            ViewData["LastNameSortParm"] = sortOrder == "lastname_desc" ? "lastname_asc" : "lastname_desc";
            ViewData["CurrentFilter"] = searchString;
            var _person = from p in _uow.GetRepository<Person>().Get()
                          select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                _person = _person.Where(s => (s.FirstName.Contains(searchString) || s.LastName.Contains(searchString)));
            }
            switch (sortOrder)
            {
                case "firstname_asc":
                    _person = _person.OrderBy(p => p.FirstName);
                    break;
                case "firstname_desc":
                    _person = _person.OrderByDescending(p => p.FirstName);
                    break;
                case "lastname_asc":
                    _person = _person.OrderBy(p => p.LastName);
                    break;
                case "lastname_desc":
                    _person = _person.OrderByDescending(p => p.LastName);
                    break;
                default:
                    break;
            }
            return View(await _person.AsNoTracking().ToListAsync());
        }

        // GET: Persons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Person _person = await _uow.GetRepository<Person>()
                                        .Get(p => p.Id == id)
                                        .Include(p => p.Business)
                                            .ThenInclude(p => p.Waste)
                                                .ThenInclude(wt => wt.WasteType)
                                        .SingleOrDefaultAsync();

            if (_person == null)
            {
                return NotFound();
            }

            return View(_person);
        }

        // GET: Persons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Persons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Id")] Person person)
        {
            if (ModelState.IsValid)
            {
                if (_uow.GetRepository<Person>().Add(person))
                {
                    await _uow.CommitAsync();
                    return RedirectToAction("Index");
                }
            }
            return View(person);
        }

        // GET: Persons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            Person _person = await _uow.GetRepository<Person>()
                                        .FindAsync(x => x.Id == id);
            if (_person == null)
            {
                return NotFound();
            }
            return View(_person);
        }

        // POST: Persons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FirstName,LastName,Id")] Person _person)
        {
            if (id != _person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (_uow.GetRepository<Person>().Update(_person))
                        await _uow.CommitAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(_person.Id))
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
            return View(_person);
        }

        // GET: Persons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Person _person = await _uow.GetRepository<Person>().FindAsync(x => x.Id == id);

            if (_person == null)
            {
                return NotFound();
            }

            return View(_person);
        }

        // POST: Persons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var _person = await _uow.GetRepository<Person>().FindAsync(x => x.Id == id);
            if (_person != null)
            {
                if (_uow.GetRepository<Person>().Delete(id))
                    if (await _uow.CommitAsync() <= 0)

                        ModelState.AddModelError("", "Unable to save changes. " +
                                            "Try again, and if the problem persists, " +
                                            "see your system administrator.");
            }
            return RedirectToAction("Index");
        }

        private bool PersonExists(int id)
        {
            return _uow.GetRepository<Person>().Any(p => p.Id == id);
        }
    }
}
