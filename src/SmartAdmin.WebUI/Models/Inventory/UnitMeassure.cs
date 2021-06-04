using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SideXC.WebUI.Models.Security;
using SideXC.WebUI.Data;

namespace SideXC.WebUI.Models.Inventory
{
    public class UnitMeassure
    {
        [Key]
        public int Id { get; set; }
        [StringLength(3, ErrorMessage = "Please type a valid description.")]
        [MaxLength(3, ErrorMessage = "Maximum lenth is 3 characters for this field.")]
        public string Code { get; set; }
        [StringLength(100, ErrorMessage = "Please type a valid description.")]
        [MaxLength(100, ErrorMessage = "Maximum lenth is 100 characters for this field.")]
        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public virtual ApplicationUser ModifiedBy { get; set; }
    }
}
