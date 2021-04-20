using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SideXC.WebUI.Models.Human_Resources;
using SideXC.WebUI.Models.Security;

namespace SideXC.WebUI.Models.Inventory
{
    public class SupplierContact
    {
        [Key]
        public int Id { get; set; }
        [StringLength(500, ErrorMessage = "Please type a valid Contact Name.")]
        [MaxLength(500, ErrorMessage = "Maximum lenth is 500 characters for this field.")]
        [Required(ErrorMessage = "Required.")]
        public string ContactName { get; set; }
        public virtual ContactType ContactType { get; set; }
        [StringLength(100, ErrorMessage = "Please type a valid description.")]
        [MaxLength(100, ErrorMessage = "Maximum lenth is 100 characters for this field.")]
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public virtual ClientUser CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public virtual ClientUser ModifiedBy { get; set; }
    }
}
