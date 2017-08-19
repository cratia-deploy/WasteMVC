using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WasteMVC.Data;
using WasteMVC.Models;

namespace WasteMVC.Controllers
{
    public class WasteTypesController : Controller
    {
        private readonly SystemContext _context;

        public WasteTypesController(SystemContext context)
        {
            _context = context;
        }

        // GET: WasteTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.WasteTypes.ToListAsync());
        }

        // GET: WasteTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wasteType = await _context.WasteTypes
                .SingleOrDefaultAsync(m => m.Id == id);
            if (wasteType == null)
            {
                return NotFound();
            }

            return View(wasteType);
        }

        // GET: WasteTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WasteTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description,Id")] WasteType wasteType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wasteType);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(wasteType);
        }

        // GET: WasteTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wasteType = await _context.WasteTypes.SingleOrDefaultAsync(m => m.Id == id);
            if (wasteType == null)
            {
                return NotFound();
            }
            return View(wasteType);
        }

        // POST: WasteTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Description,Id")] WasteType wasteType)
        {
            if (id != wasteType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wasteType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WasteTypeExists(wasteType.Id))
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
            return View(wasteType);
        }

        // GET: WasteTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wasteType = await _context.WasteTypes
                .SingleOrDefaultAsync(m => m.Id == id);
            if (wasteType == null)
            {
                return NotFound();
            }

            return View(wasteType);
        }

        // POST: WasteTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wasteType = await _context.WasteTypes.SingleOrDefaultAsync(m => m.Id == id);
            _context.WasteTypes.Remove(wasteType);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool WasteTypeExists(int id)
        {
            return _context.WasteTypes.Any(e => e.Id == id);
        }

        [HttpGet]
        [ActionName("Json")]
        public async Task<IActionResult> IndexJson(int? id)
        {
            if ((id != null) && (id.HasValue))
            {
                return Json(_context.WasteTypes.Find(id.Value));
            }
            return Json(await _context.WasteTypes.ToListAsync());
        }
    }
}
