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
    public class HallwaysController : BaseController
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public HallwaysController(ApplicationDbContext context)
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
            return View(await _context.Hallways.Include(i => i.Warehouse).ToListAsync());
        }
        /// <summary>
        /// Create view
        /// </summary>
        /// <returns></returns>
        [Authorization]
        public IActionResult Create()
        {
            var listWarehouse = _context.Warehouses.Where(c => c.Active == true).ToList();
            ViewBag.Warehouses = new SelectList(listWarehouse, "Id", "Description");
            return View();
        }
        /// <summary>
        /// Create post
        /// </summary>
        /// <param name="hallway"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [Authorization]
        [HttpPost]
        public async Task<IActionResult> Create(Hallway hallway, IFormCollection collection)
        {
            var warehouseId = int.Parse(collection["Warehouse"].ToString());
            var warehouse = _context.Warehouses.FirstOrDefault(w => w.Id == warehouseId);
            hallway.Warehouse = warehouse;

            if (HallwayExists(hallway.Description, hallway.Warehouse))
            {
                ModelState.AddModelError("Error", "Ya existe un pasillo con esa descripción.");
            }
            if (ModelState.IsValid)
            {
                hallway.Active = true;
                hallway.Created = DateTime.Now;
                hallway.CreatedBy = UserLogged;
                hallway.Modified = DateTime.Now;
                hallway.ModifiedBy = UserLogged;
                _context.Add(hallway);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var listWarehouse = _context.Warehouses.Where(c => c.Active == true).ToList();
            ViewBag.Warehouses = new SelectList(listWarehouse, "Id", "Description");
            return View(hallway);
        }
        /// <summary>
        /// Edit view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorization]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var listWarehouse = _context.Warehouses.Where(c => c.Active == true).ToList();
            ViewBag.Warehouses = new SelectList(listWarehouse, "Id", "Description");
            var hallway = await _context.Hallways.FindAsync(id);
            if (hallway == null)
            {
                return NotFound();
            }
            return View(hallway);
        }
        /// <summary>
        /// Edit post
        /// </summary>
        /// <param name="id"></param>
        /// <param name="hallway"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [Authorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Hallway hallway, IFormCollection collection)
        {
            var description = collection["hddDescription"].ToString();
            var warehouseId = int.Parse(collection["Warehouse"].ToString());
            var warehouse = _context.Warehouses.FirstOrDefault(w => w.Id == warehouseId);
            hallway.Warehouse = warehouse;

            if (id != hallway.Id) { return NotFound(); }
            if (hallway.Description != description)
            {
                if (HallwayExists(hallway.Description, hallway.Warehouse))
                {
                    ModelState.AddModelError("Error", "Ya existe un pasillo con esa descripción.");
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    hallway.Modified = DateTime.Now;
                    hallway.ModifiedBy = UserLogged;
                    _context.Update(hallway);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HallwayExists(hallway.Description, hallway.Warehouse)) { return NotFound(); }
                    else { throw; }
                }
                return RedirectToAction(nameof(Index));
            }
            var listWarehouse = _context.Warehouses.Where(c => c.Active == true).ToList();
            ViewBag.Warehouses = new SelectList(listWarehouse, "Id", "Description");
            return View(hallway);
        }
        /// <summary>
        /// Valida si existe el nuevo item
        /// </summary>
        /// <param name="description"></param>
        /// <param name="warehouse"></param>
        /// <returns></returns>
        private bool HallwayExists(string description, Warehouse warehouse)
        {
            return _context.Hallways.Any(e => e.Description == description && e.Warehouse.Id == warehouse.Id);
        }
    }
}
