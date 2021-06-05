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
    public class MaterialTypeCostsController : BaseController
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public MaterialTypeCostsController(ApplicationDbContext context)
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
            return View(await _context.MaterialTypeCosts.Where(a=> a.Active).ToListAsync());
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
        /// <param name="materialTypeCost"></param>
        /// <returns></returns>
        [Authorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Code,MinimunRange,MaximunRange,Active,Created,Modified")] MaterialTypeCost materialTypeCost)
        {
            if (ModelState.IsValid)
            {
                materialTypeCost.Active = true;
                materialTypeCost.Created = DateTime.Now;
                materialTypeCost.CreatedBy = UserLogged;
                materialTypeCost.Modified = DateTime.Now;
                materialTypeCost.ModifiedBy = UserLogged;
                _context.Add(materialTypeCost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(materialTypeCost);
        }
        /// <summary>
        /// Edit view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorization]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) { return NotFound(); }
            var materialType = await _context.MaterialTypeCosts.FindAsync(id);
            if (materialType == null) { return NotFound(); }
            return View(materialType);
        }
        /// <summary>
        /// Edit post
        /// </summary>
        /// <param name="id"></param>
        /// <param name="materialTypeCost"></param>
        /// <returns></returns>
        [Authorization]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Code,MinimunRange,MaximunRange,Active,Created,Modified")] MaterialTypeCost materialTypeCost)
        {
            if (id != materialTypeCost.Id) { return NotFound(); }
            if (ModelState.IsValid)
            {
                try
                {
                    materialTypeCost.Modified = DateTime.Now;
                    materialTypeCost.ModifiedBy = UserLogged;
                    _context.Update(materialTypeCost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialTypeExists(materialTypeCost.Id)) { return NotFound(); }
                    else { throw; }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(materialTypeCost);
        }
        /// <summary>
        /// Validar item nuevo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool MaterialTypeExists(int id)
        {
            return _context.MaterialTypeCosts.Any(e => e.Id == id);
        }
    }
}
