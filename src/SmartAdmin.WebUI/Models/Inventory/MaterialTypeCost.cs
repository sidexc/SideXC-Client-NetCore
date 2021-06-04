using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SideXC.WebUI.Models.Security;
using SideXC.WebUI.Data;

namespace SideXC.WebUI.Models.Inventory
{
    public class MaterialTypeCost
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100, ErrorMessage = "Please type a valid description.")]
        [MaxLength(100, ErrorMessage = "Maximum lenth is 100 characters for this field.")]
        [Required(ErrorMessage = "Required.")]
        public string Description { get; set; }
        [StringLength(3, ErrorMessage = "Please type a valid Code.")]
        [MaxLength(3, ErrorMessage = "Maximum lenth is 3 characters for this field.")]
        [Required(ErrorMessage = "Required.")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Required.")]
        [DataType("decimal(18,5)")]
        public decimal MinimunRange { get; set; }
        [Required(ErrorMessage = "Required.")]
        [DataType("decimal(18,5)")]
        public decimal MaximunRange { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public virtual ApplicationUser ModifiedBy { get; set; }
    }
}
