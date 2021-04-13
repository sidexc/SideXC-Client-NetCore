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
        [StringLength(100, ErrorMessage = "Please type a valid Name.")]
        [MaxLength(100, ErrorMessage = "Maximum lenth is 100 characters for this field.")]
        public string Name { get; set; }
        [StringLength(500, ErrorMessage = "Please type a valid Description.")]
        [MaxLength(500, ErrorMessage = "Maximum lenth is 500 characters for this field.")]
        public string Description { get; set; }
        public double StandardCost { get; set; }
        public virtual UnitMeassure UnitMeassure { get; set; }
        public virtual MaterialType MaterialType { get; set; }
        public double MOQ { get; set; }
        public int LeadTime { get; set; }
        public virtual Supplier Supplier { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public virtual ClientUser CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public virtual ClientUser ModifiedBy { get; set; }
    }
}
