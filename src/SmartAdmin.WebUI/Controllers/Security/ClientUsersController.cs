using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SideXC.WebUI.Classes;
using SideXC.WebUI.Data;
using SideXC.WebUI.Models.Security;

namespace SideXC.WebUI.Controllers.Security
{
    public class ClientUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ClientUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.ApplicationUsers.ToListAsync());
        }

        // GET: ClientUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ClientUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationUser clientUser)
        {
            if (ClientUserExists(clientUser.Email))
            {
                ModelState.AddModelError("Err", "The User already exists in database.");
                return View(clientUser);
            }

            if (ModelState.IsValid)
            {

                clientUser.UID = Guid.NewGuid();
                //clientUser.CompanyId = 1; //Comms:Modificar a que sea variable
                //clientUser.Password = Common.GetRandomAlphanumericString(8);
                //clientUser.LastAccess = DateTime.Now;
                //clientUser.FailNumberAccess = 0;
                //clientUser.Status = statusUserActive;
                //clientUser.Created = DateTime.Now;
                //clientUser.CreatedBy = 1;//Comms:Modificar a que sea variable
                //clientUser.Modified = DateTime.Now;
                //clientUser.ModifiedBy = 1;//Comms:Modificar a que sea variable

                _context.Add(clientUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clientUser);
        }

        // GET: ClientUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientUser = await _context.ClientUsers.FindAsync(id);
            if (clientUser == null)
            {
                return NotFound();
            }
            return View(clientUser);
        }

        // POST: ClientUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UID,CompanyId,Email,Password,LastAccess,FailNumberAccess,Active,Created,CreatedBy,Modified,ModifiedBy")] ClientUser clientUser)
        {
            if (id != clientUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clientUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(clientUser);
        }


        private bool ClientUserExists(string email)
        {
            return _context.ClientUsers.Any(e => e.Email == email);
        }
    }
}
