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
    public class MaterialTypesController : BaseController
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public MaterialTypesController(ApplicationDbContext context)
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
            return View(await _context.MaterialTypes.ToListAsync());
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
        /// <param name="materialType"></param>
        /// <returns></returns>
        [Authorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Active,Created,Modified")] MaterialType materialType)
        {
            if (MaterialTypeExists(materialType.Description))
            {
                ModelState.AddModelError("Error", "Ya existe un tipo de material con esa descripción.");
            }
            if (ModelState.IsValid)
            {
                materialType.Active = true;
                materialType.Created = DateTime.Now;
                materialType.CreatedBy = UserLogged;
                materialType.Modified = DateTime.Now;
                materialType.ModifiedBy = UserLogged;
                _context.Add(materialType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(materialType);
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
            var materialType = await _context.MaterialTypes.FindAsync(id);
            if (materialType == null) { return NotFound(); }
            return View(materialType);
        }
        /// <summary>
        /// Edit post
        /// </summary>
        /// <param name="id"></param>
        /// <param name="materialType"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [Authorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Active,Created,Modified")] MaterialType materialType, IFormCollection collection)
        {
            var description = collection["hddDescription"].ToString();
            if (id != materialType.Id) { return NotFound(); }
            if (materialType.Description != description)
            {
                if (MaterialTypeExists(materialType.Description))
                {
                    ModelState.AddModelError("Error", "Ya existe un tipo de material con esa descripción.");
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    materialType.Modified = DateTime.Now;
                    materialType.ModifiedBy = UserLogged;
                    _context.Update(materialType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialTypeExists(materialType.Description)) { return NotFound(); }
                    else { throw; }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(materialType);
        }
        /// <summary>
        /// validation
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        private bool MaterialTypeExists(string description)
        {
            return _context.MaterialTypes.Any(e => e.Description == description);
        }
    }
}
