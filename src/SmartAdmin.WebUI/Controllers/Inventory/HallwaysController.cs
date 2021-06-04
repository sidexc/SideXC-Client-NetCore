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
    public class HallwaysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HallwaysController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Hallways
        public async Task<IActionResult> Index()
        {
            return View(await _context.Hallways.Include(i => i.Warehouse).ToListAsync());
        }

        // GET: Hallways/Create
        public IActionResult Create()
        {
            var listWarehouse = _context.Warehouses.Where(c => c.Active == true).ToList();
            ViewBag.Warehouses = new SelectList(listWarehouse, "Id", "Description");
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
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
                hallway.CreatedBy = null;//Comms:Modificar a que sea variable
                hallway.Modified = DateTime.Now;
                hallway.ModifiedBy = null;//Comms:Modificar a que sea variable
                _context.Add(hallway);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var listWarehouse = _context.Warehouses.Where(c => c.Active == true).ToList();
            ViewBag.Warehouses = new SelectList(listWarehouse, "Id", "Description");
            return View(hallway);
        }

        // GET: Hallways/Edit/5
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

        // POST: Hallways/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Hallway hallway, IFormCollection collection)
        {
            var description = collection["hddDescription"].ToString();
            var warehouseId = int.Parse(collection["Warehouse"].ToString());
            var warehouse = _context.Warehouses.FirstOrDefault(w => w.Id == warehouseId);
            hallway.Warehouse = warehouse;

            if (id != hallway.Id)
            {
                return NotFound();
            }
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
                    hallway.ModifiedBy = null;//Comms:Modificar a que sea variable
                    _context.Update(hallway);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HallwayExists(hallway.Description, hallway.Warehouse))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var listWarehouse = _context.Warehouses.Where(c => c.Active == true).ToList();
            ViewBag.Warehouses = new SelectList(listWarehouse, "Id", "Description");
            return View(hallway);
        }

        private bool HallwayExists(string description, Warehouse warehouse)
        {
            return _context.Hallways.Any(e => e.Description == description && e.Warehouse.Id == warehouse.Id);
        }
    }
}
