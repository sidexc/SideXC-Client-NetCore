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
    public class EmployeeTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EmployeeTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.EmployeeTypes.ToListAsync());
        }

        // GET: EmployeeTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EmployeeTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                employeeType.CreatedBy = null;//Comms:Modificar a que sea variable
                employeeType.Modified = DateTime.Now;
                employeeType.ModifiedBy = null;//Comms:Modificar a que sea variable
                _context.Add(employeeType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employeeType);
        }

        // GET: EmployeeTypes/Edit/5
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

        // POST: EmployeeTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Active,Created,Modified")] EmployeeType employeeType, IFormCollection collection)
        {
            var description = collection["hddDescription"].ToString();
            if (id != employeeType.Id)
            {
                return NotFound();
            }
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
                    _context.Update(employeeType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeTypeExists(employeeType.Description))
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
            return View(employeeType);
        }

        private bool EmployeeTypeExists(string description)
        {
            return _context.EmployeeTypes.Any(e => e.Description == description);
        }
    }
}
