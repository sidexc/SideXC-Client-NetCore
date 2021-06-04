using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SideXC.WebUI.Data;
using SideXC.WebUI.Models.Human_Resources;

namespace SideXC.WebUI.Controllers.HumanResources
{
    public class PositionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PositionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Index view
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _context.Positions.ToListAsync());
        }
        /// <summary>
        /// Create view
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// Create Post
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Active,Created,Modified")] Position position)
        {
            if (PositionExists(position.Description))
            {
                ModelState.AddModelError("Error", "Ya existe una posición con esa descripción.");
            }
            if (ModelState.IsValid)
            {
                position.Active = true;
                position.Created = DateTime.Now;
                position.CreatedBy = null;//Comms:Modificar a que sea variable
                position.Modified = DateTime.Now;
                position.ModifiedBy = null;//Comms:Modificar a que sea variable
                _context.Add(position);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(position);
        }
        /// <summary>
        /// Edit view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = await _context.Positions.FindAsync(id);
            if (position == null)
            {
                return NotFound();
            }
            return View(position);
        }
        /// <summary>
        /// Edit Post
        /// </summary>
        /// <param name="id"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Active,Created,Modified")] Position position, IFormCollection collection)
        {
            var description = collection["hddDescription"].ToString();
            if (id != position.Id)
            {
                return NotFound();
            }
            if (position.Description != description)
            {
                if (PositionExists(position.Description))
                {
                    ModelState.AddModelError("Error", "Ya existe una posición con esa descripción.");
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    position.Modified = DateTime.Now;
                    _context.Update(position);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PositionExists(position.Description))
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
            return View(position);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool PositionExists(string description)
        {
            return _context.Positions.Any(e => e.Description == description);
        }
    }
}
