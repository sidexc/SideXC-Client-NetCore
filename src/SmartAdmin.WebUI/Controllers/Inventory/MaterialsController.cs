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

namespace SideXC.WebUI.Controllers.Inventory
{
    public class MaterialsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MaterialsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Materials
        public async Task<IActionResult> Index()
        {
            return View(await _context.Materials.Include(u=> u.UnitMeassure).Include(i=> i.MaterialType).Include(i => i.MaterialTypeCost).Include(s=> s.Supplier).ToListAsync());
        }

        public IActionResult Create()
        {
            var listUnitMeassures = _context.UnitMeassures.Where(c => c.Active == true).ToList();
            var listMaterialTypes = _context.MaterialTypes.Where(c => c.Active == true).ToList();
            var listMaterialTypeCosts = _context.MaterialTypeCosts.Where(c => c.Active == true).ToList();
            var listSuppliers = _context.Suppliers.Where(c => c.Active == true).ToList();
            ViewBag.UnitMeassures = new SelectList(listUnitMeassures, "Id", "Description");
            ViewBag.MaterialTypes = new SelectList(listMaterialTypes, "Id", "Description");
            ViewBag.MaterialTypeCosts = new SelectList(listMaterialTypeCosts, "Id", "Description");
            ViewBag.Suppliers = new SelectList(listSuppliers, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Material material, IFormCollection collection)
        {
            if (MaterialExists(material.Name)){
                ModelState.AddModelError("Error", "Ya existe un material con ese nombre");
            }

            if (ModelState.IsValid)
            {
                var unitMeassure = _context.UnitMeassures.FirstOrDefault(u=> u.Id == int.Parse(collection["ddUnitMeassure"].ToString()));
                var materialType = _context.MaterialTypes.FirstOrDefault(u => u.Id == int.Parse(collection["ddMaterialType"].ToString()));
                var materialTypeCost = _context.MaterialTypeCosts.FirstOrDefault(u => u.Id == int.Parse(collection["ddMaterialTypeCost"].ToString()));
                var supplier = _context.Suppliers.FirstOrDefault(u => u.Id == int.Parse(collection["ddSupplier"].ToString()));
                material.UnitMeassure = unitMeassure;
                material.MaterialType = materialType;
                material.MaterialTypeCost = materialTypeCost;
                material.Supplier = supplier;
                material.Serial = CreateBarCode(material);
                material.Active = true;
                material.Created = DateTime.Now;
                material.CreatedBy = null;//Comms:Modificar a que sea variable
                material.Modified = DateTime.Now;
                material.ModifiedBy = null;//Comms:Modificar a que sea variable
                _context.Add(material);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var listUnitMeassures = _context.UnitMeassures.Where(c => c.Active == true).ToList();
            var listMaterialTypes = _context.MaterialTypes.Where(c => c.Active == true).ToList();
            var listMaterialTypeCosts = _context.MaterialTypeCosts.Where(c => c.Active == true).ToList();
            var listSuppliers = _context.Suppliers.Where(c => c.Active == true).ToList();
            ViewBag.UnitMeassures = new SelectList(listUnitMeassures, "Id", "Description");
            ViewBag.MaterialTypes = new SelectList(listMaterialTypes, "Id", "Description");
            ViewBag.MaterialTypeCosts = new SelectList(listMaterialTypeCosts, "Id", "Description");
            ViewBag.Suppliers = new SelectList(listSuppliers, "Id", "Name");
            return View(material);
        }
        /// <summary>
        /// Create barcode for material
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        private string CreateBarCode(Material material)
        {
            var hayConsecutivo = _context.SerialConsecutives.FirstOrDefault(s => s.Created == DateTime.Today);
            SerialConsecutive consecutive;

            if(hayConsecutivo == null)
            {
                consecutive = new SerialConsecutive()
                {
                    Folio = 1,
                    Created = DateTime.Today,
                    CreatedBy = null,
                    Modified = DateTime.Today,
                    ModifiedBy = null
                };
                _context.Add(consecutive);
                _context.SaveChanges();
            }
            else
            {
                consecutive = hayConsecutivo;
                consecutive.Folio += 1;
                _context.SaveChanges();
            }

            return string.Concat(material.Name.Substring(0,2),
                                    material.MaterialTypeCost.Code,
                                    DateTime.Today.Year.ToString().PadLeft(4,'0'),
                                    DateTime.Today.Month.ToString().PadLeft(2, '0'),
                                    DateTime.Today.Day.ToString().PadLeft(2, '0'),
                                    consecutive.Folio.ToString().PadLeft(3,'0')
                                    );
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
            var listUnitMeassures = _context.UnitMeassures.Where(c => c.Active == true).ToList();
            var listMaterialTypes = _context.MaterialTypes.Where(c => c.Active == true).ToList();
            var listMaterialTypeCosts = _context.MaterialTypeCosts.Where(c => c.Active == true).ToList();
            var listSuppliers = _context.Suppliers.Where(c => c.Active == true).ToList();
            ViewBag.UnitMeassures = new SelectList(listUnitMeassures, "Id", "Description");
            ViewBag.MaterialTypes = new SelectList(listMaterialTypes, "Id", "Description");
            ViewBag.MaterialTypeCosts = new SelectList(listMaterialTypeCosts, "Id", "Description");
            ViewBag.Suppliers = new SelectList(listSuppliers, "Id", "Name");
            var material = await _context.Materials.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }
            return View(material);
        }
        /// <summary>
        /// Edit Post
        /// </summary>
        /// <param name="id"></param>
        /// <param name="material"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Material material, IFormCollection collection)
        {
            if (id != material.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var unitMeassure = _context.UnitMeassures.FirstOrDefault(u => u.Id == int.Parse(collection["UnitMeassure"].ToString()));
                    var materialType = _context.MaterialTypes.FirstOrDefault(u => u.Id == int.Parse(collection["MaterialType"].ToString()));
                    var materialTypeCost = _context.MaterialTypeCosts.FirstOrDefault(u => u.Id == int.Parse(collection["MaterialTypeCost"].ToString()));
                    var supplier = _context.Suppliers.FirstOrDefault(u => u.Id == int.Parse(collection["Supplier"].ToString()));
                    material.UnitMeassure = unitMeassure;
                    material.MaterialType = materialType;
                    material.MaterialTypeCost = materialTypeCost;
                    material.Supplier = supplier;
                    material.Active = true;
                    material.Modified = DateTime.Now;
                    material.ModifiedBy = null;//Comms:Modificar a que sea variable
                    _context.Update(material);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialExists(material.Name))
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
            return View(material);
        }
        /// <summary>
        /// Ajax method for material type cost
        /// </summary>
        /// <param name="stdcost"></param>
        /// <returns></returns>
        public string GetMaterialTypeCostId(string stdcost)
        {
            var dblStdCost = decimal.Parse(stdcost);
            var item = _context.MaterialTypeCosts.FirstOrDefault(m => m.MinimunRange <= dblStdCost && m.MaximunRange >= dblStdCost);
            return JsonConvert.SerializeObject(item);
        }
        /// <summary>
        /// Material validation
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool MaterialExists(string name)
        {
            return _context.Materials.Any(e => e.Name == name);
        }
    }
}
