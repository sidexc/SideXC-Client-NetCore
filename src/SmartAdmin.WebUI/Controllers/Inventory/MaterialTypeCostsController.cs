using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SideXC.WebUI.Data;
using SideXC.WebUI.Models.Enumerator;
using SideXC.WebUI.Models.Inventory;

namespace SideXC.WebUI.Controllers.Inventory
{
    public class MaterialTypeCostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MaterialTypeCostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MaterialTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.MaterialTypeCosts.Where(a=> a.Active).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Code,MinimunRange,MaximunRange,Active,Created,Modified")] MaterialTypeCost materialTypeCost)
        {
            if (ModelState.IsValid)
            {
                materialTypeCost.Active = true;
                materialTypeCost.Created = DateTime.Now;
                materialTypeCost.CreatedBy = null;//Comms:Modificar a que sea variable
                materialTypeCost.Modified = DateTime.Now;
                materialTypeCost.ModifiedBy = null;//Comms:Modificar a que sea variable
                _context.Add(materialTypeCost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(materialTypeCost);
        }

        // GET: MaterialTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialType = await _context.MaterialTypeCosts.FindAsync(id);
            if (materialType == null)
            {
                return NotFound();
            }
            return View(materialType);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Code,MinimunRange,MaximunRange,Active,Created,Modified")] MaterialTypeCost materialTypeCost)
        {
            if (id != materialTypeCost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    materialTypeCost.Modified = DateTime.Now;
                    materialTypeCost.ModifiedBy = null;//Comms:Modificar a que sea variable
                    _context.Update(materialTypeCost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialTypeExists(materialTypeCost.Id))
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
            return View(materialTypeCost);
        }

        private bool MaterialTypeExists(int id)
        {
            return _context.MaterialTypeCosts.Any(e => e.Id == id);
        }
    }
}
