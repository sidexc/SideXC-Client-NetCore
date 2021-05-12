using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SideXC.WebUI.Models.Security;

namespace SideXC.WebUI.Models.Inventory
{
    public class Material
    {
        [Key]
        public int Id { get; set; }
        [StringLength(20, ErrorMessage = "Please type a valid Barcode.")]
        [MaxLength(20, ErrorMessage = "Maximum lenth is 20 characters for this field.")]
        public string Serial { get; set; }
        [StringLength(100, ErrorMessage = "Please type a valid Name.")]
        [MaxLength(100, ErrorMessage = "Maximum lenth is 100 characters for this field.")]
        [Required(ErrorMessage = "Required.")]
        public string Name { get; set; }
        [StringLength(500, ErrorMessage = "Please type a valid Description.")]
        [MaxLength(500, ErrorMessage = "Maximum lenth is 500 characters for this field.")]
        [Required(ErrorMessage = "Required.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Required.")]
        [DataType("decimal(18,5)")]
        public decimal StandardCost { get; set; }
        [Required(ErrorMessage = "Required.")]
        [DataType("decimal(18,5)")]
        public decimal SalePrice { get; set; }
        public virtual UnitMeassure UnitMeassure { get; set; }
        public virtual MaterialType MaterialType { get; set; }
        public virtual MaterialTypeCost MaterialTypeCost { get; set; }
        [Required(ErrorMessage = "Required.")]
        public double MOQ { get; set; }
        [Required(ErrorMessage = "Required.")]
        public int LeadTime { get; set; }
        public virtual Supplier Supplier { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public virtual ClientUser CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public virtual ClientUser ModifiedBy { get; set; }
    }
}
