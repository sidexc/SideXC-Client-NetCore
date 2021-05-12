using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SideXC.WebUI.Data;
using SideXC.WebUI.Models.Enumerator;
using SideXC.WebUI.Models.Inventory;

namespace SideXC.WebUI.Controllers.Inventory
{
    public class TransactionTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.TransactionTypes.ToListAsync());
        }

        public IActionResult Create()
        {
            var listSigns = from eSign e in Enum.GetValues(typeof(eSign))
                            select new { Id = e, Description = e.ToString() };
            ViewBag.Signs = new SelectList(listSigns, "Id", "Description");
            return View();
        }
        /// <summary>
        /// Create post
        /// </summary>
        /// <param name="transactionType"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Code,Sign,IsAdjustment,Active,Created,Modified")] TransactionType transactionType)
        {
            if (TransactionTypeExists(transactionType.Description, transactionType.Code))
            {
                ModelState.AddModelError("Error", "Ya existe un tipo de transacción con esa descripción y código.");
            }

            if (ModelState.IsValid)
            {
                transactionType.Active = true;
                transactionType.Created = DateTime.Now;
                transactionType.CreatedBy = null;//Comms:Modificar a que sea variable
                transactionType.Modified = DateTime.Now;
                transactionType.ModifiedBy = null;//Comms:Modificar a que sea variable
                _context.Add(transactionType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var listSigns = from eSign e in Enum.GetValues(typeof(eSign))
                            select new { Id = e, Description = e.ToString() };
            ViewBag.Signs = new SelectList(listSigns, "Id", "Description");
            return View(transactionType);
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
            var listSigns = from eSign e in Enum.GetValues(typeof(eSign))
                            select new { Id = e, Description = e.ToString() };
            ViewBag.Signs = new SelectList(listSigns, "Id", "Description");
            var transactionType = await _context.TransactionTypes.FindAsync(id);
            if (transactionType == null)
            {
                return NotFound();
            }
            return View(transactionType);
        }
        /// <summary>
        /// Edit post
        /// </summary>
        /// <param name="id"></param>
        /// <param name="transactionType"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Code,Sign,IsAdjustment,Active,Created,Modified")] TransactionType transactionType, IFormCollection collection)
        {
            var description = collection["hddDescription"].ToString();
            if (id != transactionType.Id)
            {
                return NotFound();
            }
            if (transactionType.Description != description)
            {
                if (TransactionTypeExists(transactionType.Description, transactionType.Code))
                {
                    ModelState.AddModelError("Error", "Ya existe un tipo de transacción con esa descripción y código.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    transactionType.Modified = DateTime.Now;
                    transactionType.ModifiedBy = null;//Comms:Modificar a que sea variable
                    _context.Update(transactionType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionTypeExists(transactionType.Description, transactionType.Code))
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
            return View(transactionType);
        }
        /// <summary>
        /// Validation
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        private bool TransactionTypeExists(string description, string code)
        {
            return _context.TransactionTypes.Any(e => e.Description == description && e.Code == code);
        }
    }
}
