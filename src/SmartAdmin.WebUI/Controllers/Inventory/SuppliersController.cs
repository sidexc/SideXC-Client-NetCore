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
using SideXC.WebUI.Models.Local;
using SideXC.WebUI.Models.Map;

namespace SideXC.WebUI.Controllers.Inventory
{
    public class SuppliersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuppliersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Suppliers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Suppliers.Include(c=> c.Contacts).ThenInclude(c => c.ContactType).Include(a => a.Address).Include(p => p.PaymentMethod).Include(c => c.Currency).Include(a => a.Address).ThenInclude(c => c.Neighborhood).ThenInclude(c => c.City).ThenInclude(s => s.State).ThenInclude(c => c.Country).ToListAsync());
        }

        public IActionResult Create()
        {
            var listPaymentMethods = _context.PaymentMethods.Where(c => c.Active == true).ToList();
            var listCurrencies = _context.Currencies.Where(c => c.Active == true).ToList();
            var listContactTypes = _context.ContactTypes.Where(c => c.Active == true).ToList();
            ViewBag.PaymentMethods = new SelectList(listPaymentMethods, "Id", "Description");
            ViewBag.Currencies = new SelectList(listCurrencies, "Id", "Description");
            ViewBag.ContactTypes = new SelectList(listContactTypes, "Id", "Description", 0);
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Supplier supplier, IFormCollection collection)
        {
            var contacts = collection["hddContacts"].ToString();
            var listContacts = JsonConvert.DeserializeObject<List<lContacts>>(contacts).ToList();
            var coloniaId = int.Parse(collection["ddColonia"].ToString());
            var colonia = _context.Neighborhoods.Include(c => c.City).ThenInclude(s => s.State).ThenInclude(c => c.Country).FirstOrDefault(n => n.Id == coloniaId);
            var paymentMethod = _context.PaymentMethods.FirstOrDefault(p => p.Id == int.Parse(collection["PaymentMethod"].ToString()));
            var currency = _context.Currencies.FirstOrDefault(p => p.Id == int.Parse(collection["Currency"].ToString()));
            var neighborhood = _context.Neighborhoods.Include(i => i.City).ThenInclude(s => s.State).ThenInclude(c => c.Country).FirstOrDefault(i => i.Id == int.Parse(collection["ddColonia"].ToString()));
            //Create address
            var address = new Address()
            {
                Street = collection["txtStreet"].ToString(),
                ExternalNumber = collection["txtExternalNumber"].ToString(),
                InternalNumber = collection["txtInternalNumber"].ToString(),
                Neighborhood = neighborhood,
                Active = true,
                Created = DateTime.Now,
                CreatedBy = null,//Comms:Modificar a que sea variable
                Modified = DateTime.Now,
                ModifiedBy = null//Comms:Modificar a que sea variable
            };
            //Create contacts
            foreach (var item in listContacts)
            {
                var contactType = _context.ContactTypes.FirstOrDefault(c => c.Id == item.ContactTypeId);
                var contact = new SupplierContact()
                {
                    ContactName = item.ContactName,
                    ContactType = contactType,
                    Description = item.Description,
                    Created = DateTime.Now,
                    CreatedBy = null,//Comms:Modificar a que sea variable
                    Modified = DateTime.Now,
                    ModifiedBy = null//Comms:Modificar a que sea variable
                };
                supplier.AddContact(contact);
            }
            supplier.PaymentMethod = paymentMethod;
            supplier.Currency = currency;
            supplier.Address = address;
            supplier.Active = true;
            supplier.Created = DateTime.Now;
            supplier.CreatedBy = null;//Comms:Modificar a que sea variable
            supplier.Modified = DateTime.Now;
            supplier.ModifiedBy = null;//Comms:Modificar a que sea variable

            _context.Add(supplier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Suppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var listPaymentMethods = _context.PaymentMethods.Where(c => c.Active == true).ToList();
            var listCurrencies = _context.Currencies.Where(c => c.Active == true).ToList();
            var listContactTypes = _context.ContactTypes.Where(c => c.Active == true).ToList();
            ViewBag.PaymentMethods = new SelectList(listPaymentMethods, "Id", "Description");
            ViewBag.Currencies = new SelectList(listCurrencies, "Id", "Description");
            ViewBag.ContactTypes = new SelectList(listContactTypes, "Id", "Description", 0);
            var supplier = _context.Suppliers.Include(c => c.Contacts).ThenInclude(c => c.ContactType).Include(a => a.Address).Include(p => p.PaymentMethod).Include(c => c.Currency).Include(a => a.Address).ThenInclude(c => c.Neighborhood).ThenInclude(c => c.City).ThenInclude(s => s.State).ThenInclude(c => c.Country).FirstOrDefault(i => i.Id == id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Supplier supplier, IFormCollection collection)
        {
            if (id != supplier.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var paymentMethod = _context.PaymentMethods.FirstOrDefault(p => p.Id == int.Parse(collection["PaymentMethod"].ToString()));
                    var currency = _context.Currencies.FirstOrDefault(p => p.Id == int.Parse(collection["Currency"].ToString()));
                    var neighborhood = _context.Neighborhoods.Include(i => i.City).ThenInclude(s => s.State).ThenInclude(c => c.Country).FirstOrDefault(i => i.Id == int.Parse(collection["ddColonia"].ToString()));
                    var address = _context.Addresses.Include(n => n.Neighborhood).ThenInclude(i => i.City).ThenInclude(s => s.State).ThenInclude(c => c.Country).FirstOrDefault(a => a.Id == int.Parse(collection["hddAddressId"].ToString()));

                    address.Street = collection["txtStreet"].ToString();
                    address.ExternalNumber = collection["txtExternalNumber"].ToString();
                    address.InternalNumber = collection["txtInternalNumber"].ToString();
                    address.Neighborhood = neighborhood;
                    address.Active = true;
                    address.Modified = DateTime.Now;
                    address.ModifiedBy = null; //Comms:Modificar a que sea variable

                    supplier.PaymentMethod = paymentMethod;
                    supplier.Currency = currency;
                    supplier.Address = address;
                    supplier.Modified = DateTime.Now;
                    supplier.ModifiedBy = null;//Comms:Modificar a que sea variable
                    _context.Update(supplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(supplier.Id))
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
            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplierExists(int id)
        {
            return _context.Suppliers.Any(e => e.Id == id);
        }
    }
}
