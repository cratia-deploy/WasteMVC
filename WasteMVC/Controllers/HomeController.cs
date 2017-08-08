﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WasteMVC.Data;
using WasteMVC.Models.IndexView;
using WasteMVC.Models.HomeView;

namespace WasteMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly SystemContext _context = null;
        private readonly UnitOfWork<SystemContext> _uow = null;

        public HomeController(SystemContext context)
        {
            _uow = new UnitOfWork<SystemContext>(context);
            _context = context;
        }

        public async Task<IActionResult> Index(int? page)
        {
            HomeIndexView _view = new HomeIndexView(_context);
            if (page == null)
            {
                page = 1;
            }
            ViewData["Page"] = page;
            await _view.CreateView(page ?? 1, 5);
            return View(_view);
        }

        public async Task<IActionResult> Edit(int? partnersID, int? page, string day = "", string wasteType = "")
        {
            if (day == "" || wasteType == "")
            {
                return RedirectToAction("Index");
            }
            if (page == null)
            {
                page = 1;
            }

            ViewData["Day"] = day;
            ViewData["WastedType"] = wasteType;
            ViewData["Page"] = page;
            ViewData["Day"] = day;
            ViewData["WasteType"] = wasteType;
            ViewData["PartnersID"] = partnersID;

            HomeEditView _view = new HomeEditView(_context, partnersID, day, wasteType);
            await _view.CreateView(page ?? 1, 4); // Definir PageSize
            return View(_view);
        }

        // POST: Home/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("WasteType,DateTime,Cost2,SalePrice2,Decrease")] HomeEditView homeEditView, int[] wastesID)
        {
            if (wastesID == null || wastesID.Length <=0)
            {
                return NotFound();
            }
            homeEditView.SetWastesID(wastesID);
            if(await homeEditView.Edit(_context) > 0)
                return RedirectToAction("Index");
            return View(homeEditView);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
