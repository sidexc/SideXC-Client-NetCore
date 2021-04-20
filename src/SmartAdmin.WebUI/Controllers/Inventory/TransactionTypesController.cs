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
    public class TransactionTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TransactionTypes
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Code,Sign,Active,Created,Modified")] TransactionType transactionType)
        {
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
            return View(transactionType);
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Code,Sign,Active,Created,Modified")] TransactionType transactionType)
        {
            if (id != transactionType.Id)
            {
                return NotFound();
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
                    if (!TransactionTypeExists(transactionType.Id))
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

        // GET: TransactionTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionType = await _context.TransactionTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transactionType == null)
            {
                return NotFound();
            }

            return View(transactionType);
        }

        // POST: TransactionTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transactionType = await _context.TransactionTypes.FindAsync(id);
            _context.TransactionTypes.Remove(transactionType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionTypeExists(int id)
        {
            return _context.TransactionTypes.Any(e => e.Id == id);
        }
    }
}
