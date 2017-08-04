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
    public class WastesController : Controller
    {
        private readonly SystemContext _context;

        public WastesController(SystemContext context)
        {
            _context = context;    
        }

        // GET: Wastes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Wastes.ToListAsync());
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
            return View();
        }

        // POST: Wastes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DateTime,Weight,Cost,SalePrice,Id")] Waste waste)
        {
            if (ModelState.IsValid)
            {
                _context.Add(waste);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(waste);
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
