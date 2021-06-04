using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SideXC.WebUI.Data;
using SideXC.WebUI.Models.Inventory;

namespace SideXC.WebUI.Controllers.Inventory
{
    public class LocationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Locations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Locations.Include(h=> h.Hallway).ThenInclude(w=> w.Warehouse).ToListAsync());
        }

        public IActionResult Create()
        {
            var listWarehouse = _context.Warehouses.Where(c => c.Active == true).ToList();
            ViewBag.Warehouses = new SelectList(listWarehouse, "Id", "Description");
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Location location, IFormCollection collection)
        {
            var hallwayId = int.Parse(collection["ddHallways"].ToString());
            var hallway = _context.Hallways.Include(w=> w.Warehouse).FirstOrDefault(w => w.Id == hallwayId);
            location.Hallway = hallway;
            if (LocationExists(location.Description, location.Hallway, location.Hallway.Warehouse))
            {
                ModelState.AddModelError("Error", "Ya existe una locación con esa descripción.");
            }
            if (ModelState.IsValid)
            {
                location.Active = true;
                location.Created = DateTime.Now;
                location.CreatedBy = null;//Comms:Modificar a que sea variable
                location.Modified = DateTime.Now;
                location.ModifiedBy = null;//Comms:Modificar a que sea variable
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var listWarehouse = _context.Warehouses.Where(c => c.Active == true).ToList();
            ViewBag.Warehouses = new SelectList(listWarehouse, "Id", "Description");
            return View(location);
        }

        // GET: Locations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var listWarehouse = _context.Warehouses.Where(c => c.Active == true).ToList();
            ViewBag.Warehouses = new SelectList(listWarehouse, "Id", "Description");
            var location = _context.Locations.Include(h => h.Hallway).ThenInclude(w => w.Warehouse).FirstOrDefault(l=> l.Id == id);
            if (location == null)
            {
                return NotFound();
            }
            return View(location);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Location location, IFormCollection collection)
        {
            var description = collection["hddDescription"].ToString();
            var hallwayId = int.Parse(collection["ddHallways"].ToString());
            var hallway = _context.Hallways.Include(w => w.Warehouse).FirstOrDefault(w => w.Id == hallwayId);
            location.Hallway = hallway;
            if (id != location.Id)
            {
                return NotFound();
            }
            if (location.Description != description)
            {
                if (LocationExists(location.Description, location.Hallway, location.Hallway.Warehouse))
                {
                    ModelState.AddModelError("Error", "Ya existe una locación con esa descripción.");
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    location.Created = DateTime.Now;
                    location.CreatedBy = null;//Comms:Modificar a que sea variable
                    location.Modified = DateTime.Now;
                    location.ModifiedBy = null;//Comms:Modificar a que sea variable
                    _context.Update(location);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.Description, location.Hallway, location.Hallway.Warehouse))
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
            return View(location);
        }

        private bool LocationExists(string description, Hallway hallway, Warehouse warehouse)
        {
            return _context.Locations.Any(e => e.Description == description && e.Hallway.Id == hallway.Id && e.Hallway.Warehouse.Id == warehouse.Id );
        }

        public string GetHallways(int warehouseId)
        {
            var list = _context.Hallways.Where(h => h.Warehouse.Id == warehouseId).Select(s=> new { s.Id, s.Description }).ToList();
            return JsonConvert.SerializeObject(list);
        }
    }
}
