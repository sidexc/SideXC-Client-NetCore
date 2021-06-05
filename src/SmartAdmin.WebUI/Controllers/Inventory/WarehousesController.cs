using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SideXC.WebUI.Data;
using SideXC.WebUI.Models.Inventory;

namespace SideXC.WebUI.Controllers.Inventory
{
    public class WarehousesController : BaseController
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public WarehousesController(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Index view
        /// </summary>
        /// <returns></returns>
        [Authorization]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Warehouses.ToListAsync());
        }
        /// <summary>
        /// Create view
        /// </summary>
        /// <returns></returns>
        [Authorization]
        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// Create post
        /// </summary>
        /// <param name="warehouse"></param>
        /// <returns></returns>
        [Authorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Active,Created,Modified")] Warehouse warehouse)
        {
            if (WarehouseExists(warehouse.Description))
            {
                ModelState.AddModelError("Error", "Ya existe un almacen con esa descripción.");
            }
            if (ModelState.IsValid)
            {
                warehouse.Active = true;
                warehouse.Created = DateTime.Now;
                warehouse.CreatedBy = UserLogged;
                warehouse.Modified = DateTime.Now;
                warehouse.ModifiedBy = UserLogged;
                _context.Add(warehouse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(warehouse);
        }
        /// <summary>
        /// Edit view
        /// </summary>
        [Authorization]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) { return NotFound(); }
            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null) { return NotFound(); }
            return View(warehouse);
        }
        /// <summary>
        /// Edit post
        /// </summary>
        /// <param name="id"></param>
        /// <param name="warehouse"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [Authorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Active,Created,Modified")] Warehouse warehouse, IFormCollection collection)
        {
            var description = collection["hddDescription"].ToString();
            if (id != warehouse.Id) { return NotFound(); }
            if (warehouse.Description != description)
            {
                if (WarehouseExists(warehouse.Description))
                {
                    ModelState.AddModelError("Error", "Ya existe un almacen con esa descripción.");
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    warehouse.Modified = DateTime.Now;
                    warehouse.ModifiedBy = UserLogged;
                    _context.Update(warehouse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WarehouseExists(warehouse.Description)) { return NotFound(); }
                    else { throw; }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(warehouse);
        }
        /// <summary>
        /// Valida nuevo item
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        private bool WarehouseExists(string description)
        {
            return _context.Warehouses.Any(e => e.Description == description);
        }
    }
}
