using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SideXC.WebUI.Models.Enumerator;
using SideXC.WebUI.Models.Security;

namespace SideXC.WebUI.Models.Inventory
{
    public class TransactionType
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
        public virtual eSign Sign { get; set; }
        public bool IsAdjustment { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public virtual ClientUser CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public virtual ClientUser ModifiedBy { get; set; }
    }
}
