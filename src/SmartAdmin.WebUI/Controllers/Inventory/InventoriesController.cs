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
using SideXC.WebUI.Models.Enumerator;
using SideXC.WebUI.Models.Inventory;
using SideXC.WebUI.Models.Local;

namespace SideXC.WebUI.Controllers.Inventory
{
    public class InventoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _context.Inventories.Include(i => i.Material).Include(i => i.Location).ThenInclude(h => h.Hallway).ThenInclude(w => w.Warehouse).ToListAsync());
        }
        /// <summary>
        /// View create
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            var listMaterials = _context.Materials.Where(c => c.Active == true).ToList();
            var listWarehouse = _context.Warehouses.Where(c => c.Active == true).ToList();
            var listTransactionTypes = _context.TransactionTypes.Where(c => c.Active == true && c.IsAdjustment == true).ToList();
            ViewBag.Materials = new SelectList(listMaterials, "Id", "Description");
            ViewBag.Warehouses = new SelectList(listWarehouse, "Id", "Description");
            ViewBag.TransactionTypes = new SelectList(listTransactionTypes, "Id", "Description");
            return View();
        }
        /// <summary>
        /// Create Method post action
        /// </summary>
        /// <param name="inventory"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SideXC.WebUI.Models.Inventory.Inventory inventory, IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                var location = _context.Locations.FirstOrDefault(u => u.Id == int.Parse(collection["ddLocations"].ToString()));
                var material = _context.Materials.FirstOrDefault(u => u.Id == int.Parse(collection["hddMaterialId"].ToString()));
                var transactionType = _context.TransactionTypes.FirstOrDefault(u => u.Id == int.Parse(collection["ddTransactionTypes"].ToString()));
                var modelInventory = _context.Inventories.FirstOrDefault(i => i.Location.Id == location.Id && i.Material.Id == material.Id);
                inventory.Location = location;
                inventory.Material = material;
                inventory.Created = DateTime.Now;
                inventory.CreatedBy = null;//Comms:Modificar a que sea variable
                inventory.Modified = DateTime.Now;
                inventory.ModifiedBy = null;//Comms:Modificar a que sea variable
                if (modelInventory != null)
                {
                    if (transactionType.Sign == eSign.Plus)
                    {
                        modelInventory.QuantityAvailable += inventory.QuantityAvailable;
                    }
                    else
                    {
                        modelInventory.QuantityAvailable -= inventory.QuantityAvailable;
                    }
                    _context.Update(modelInventory);
                }
                else
                {
                    _context.Add(inventory);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inventory);
        }
        /// <summary>
        /// Edit view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var listMaterials = _context.Materials.Where(c => c.Active == true).ToList();
            var listWarehouse = _context.Warehouses.Where(c => c.Active == true).ToList();
            var listTransactionTypes = _context.TransactionTypes.Where(c => c.Active == true && c.IsAdjustment == true).ToList();
            ViewBag.Materials = new SelectList(listMaterials, "Id", "Description");
            ViewBag.Warehouses = new SelectList(listWarehouse, "Id", "Description");
            ViewBag.TransactionTypes = new SelectList(listTransactionTypes, "Id", "Description");

            var inventory = _context.Inventories.Include(m => m.Material).Include(i => i.Location).ThenInclude(h => h.Hallway).ThenInclude(w => w.Warehouse).FirstOrDefault(i => i.Id == id);
            if (inventory == null)
            {
                return NotFound();
            }
            return View(inventory);
        }
        /// <summary>
        /// Edit Post
        /// </summary>
        /// <param name="id"></param>
        /// <param name="inventory"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, SideXC.WebUI.Models.Inventory.Inventory inventory, IFormCollection collection)
        {
            if (id != inventory.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var model = _context.Inventories.Include(m => m.Material).Include(l => l.Location).ThenInclude(h => h.Hallway).ThenInclude(w => w.Warehouse).FirstOrDefault(i => i.Id == id);
                var QtyToMove = double.Parse(collection["txtCantidadMover"].ToString());
                var Newlocation = _context.Locations.Include(h => h.Hallway).ThenInclude(w => w.Warehouse).FirstOrDefault(u => u.Id == int.Parse(collection["ddLocations"].ToString()));
                var material = _context.Materials.FirstOrDefault(u => u.Id == int.Parse(collection["hddMaterialId"].ToString()));
                var modelInventory = _context.Inventories.FirstOrDefault(i => i.Location.Id == Newlocation.Id && i.Material.Id == material.Id);
                inventory = model;
                inventory.QuantityAvailable -= QtyToMove;
                inventory.Modified = DateTime.Now;
                inventory.ModifiedBy = null;//Comms:Modificar a que sea variable
                if (modelInventory != null)
                {
                    modelInventory.QuantityAvailable += QtyToMove;
                    modelInventory.Modified = DateTime.Now;
                    modelInventory.ModifiedBy = null;//Comms:Modificar a que sea variable
                    _context.Update(modelInventory);
                }
                else
                {
                    modelInventory = new Models.Inventory.Inventory();
                    modelInventory.Location = Newlocation;
                    modelInventory.Material = material;
                    modelInventory.QuantityAvailable = QtyToMove;
                    modelInventory.Created = DateTime.Now;
                    modelInventory.CreatedBy = null;//Comms:Modificar a que sea variable
                    modelInventory.Modified = DateTime.Now;
                    modelInventory.ModifiedBy = null;//Comms:Modificar a que sea variable
                    _context.Add(modelInventory);
                }
                _context.Update(inventory);
                _context.SaveChanges();
                var transactionType = _context.TransactionTypes.FirstOrDefault(t => t.Code == "ML");
                AddTransactionLog(inventory.Material, inventory.Location, QtyToMove, transactionType);
                return RedirectToAction(nameof(Index));
            }
            return View(inventory);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetHallways(int id)
        {
            var list = _context.Hallways.Where(i => i.Warehouse.Id == id).ToList();
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetLocations(int id, int idActual)
        {
            var list = _context.Locations.Where(i => i.Hallway.Id == id && i.Id != idActual).ToList();
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetMaterialInfo(string serial)
        {
            var item = _context.Materials.FirstOrDefault(i => i.Serial == serial);
            return JsonConvert.SerializeObject(item);
        }
        /// <summary>
        /// Ajax call: save new qty
        /// </summary>
        /// <param name="id"></param>
        /// <param name="actualQty"></param>
        /// <param name="newQty"></param>
        /// <returns></returns>
        public string SaveNewQty(int id, double actualQty, double newQty)
        {
            var response = new Response();
            try
            {
                var transactionTypeCode = actualQty < newQty ? "II" : "DI";
                var transactionType = _context.TransactionTypes.FirstOrDefault(t => t.Code == transactionTypeCode);
                var model = _context.Inventories.Include(m=> m.Material).Include(l=> l.Location).FirstOrDefault(i => i.Id == id);
                model.QuantityAvailable = newQty;
                _context.Update(model);
                AddTransactionLog(model.Material, model.Location, newQty, transactionType);
                response.Information = "Nueva cantidad guardada con éxito";
                return JsonConvert.SerializeObject(response);
            }
            catch (Exception e)
            {
                response.AddError(e.Message);
                return JsonConvert.SerializeObject(response);
            }
        }
        /// <summary>
        /// Add transaction log
        /// </summary>
        /// <param name="material"></param>
        /// <param name="location"></param>
        /// <param name="qty"></param>
        /// <param name="transactionType"></param>        
        public void AddTransactionLog(Material material, Location location, double qty, TransactionType transactionType)
        {
            var log = new InventoryLog()
            {                
                material = material,
                Location = location,
                TransactionType = transactionType,
                Created = DateTime.Now,
                CreatedBy = null,//Comms:Modificar a que sea variable
                Modified = DateTime.Now,
                ModifiedBy = null//Comms:Modificar a que sea variable
            };
            _context.Add(log);
            _context.SaveChanges();
        }
    }
}
