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
    public class EmployeeTypesController : BaseController
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public EmployeeTypesController(ApplicationDbContext context)
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
            return View(await _context.EmployeeTypes.ToListAsync());
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
        /// <param name="employeeType"></param>
        /// <returns></returns>
        [Authorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Active,Created,Modified")] EmployeeType employeeType)
        {
            if (EmployeeTypeExists(employeeType.Description))
            {
                ModelState.AddModelError("Error", "Ya existe un tipo de empleado con esa descripción.");
            }
            if (ModelState.IsValid)
            {
                employeeType.Active = true;
                employeeType.Created = DateTime.Now;
                employeeType.CreatedBy = UserLogged;
                employeeType.Modified = DateTime.Now;
                employeeType.ModifiedBy = UserLogged;
                _context.Add(employeeType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employeeType);
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
            var employeeType = await _context.EmployeeTypes.FindAsync(id);
            if (employeeType == null)
            {
                return NotFound();
            }
            return View(employeeType);
        }
        /// <summary>
        /// Edit post
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employeeType"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [Authorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Active,Created,Modified")] EmployeeType employeeType, IFormCollection collection)
        {
            var description = collection["hddDescription"].ToString();
            if (id != employeeType.Id) { return NotFound(); }
            if (employeeType.Description != description)
            {
                if (EmployeeTypeExists(employeeType.Description))
                {
                    ModelState.AddModelError("Error", "Ya existe un tipo de empleado con esa descripción.");
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    employeeType.Modified = DateTime.Now;
                    employeeType.ModifiedBy = UserLogged;
                    _context.Update(employeeType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeTypeExists(employeeType.Description)) { return NotFound(); }
                    else { throw; }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employeeType);
        }
        /// <summary>
        /// Valida si existe el nuevo item
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        private bool EmployeeTypeExists(string description)
        {
            return _context.EmployeeTypes.Any(e => e.Description == description);
        }
    }
}
