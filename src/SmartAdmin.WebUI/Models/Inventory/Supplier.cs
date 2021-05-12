using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SideXC.WebUI.Models.Map;
using SideXC.WebUI.Models.Security;

namespace SideXC.WebUI.Models.Inventory
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(500, ErrorMessage = "Maximum lenth is 500 characters for this field.")]
        [Required(ErrorMessage = "Required.")]
        public string Name { get; set; }
        public virtual Address Address { get; set; }
        [MaxLength(20, ErrorMessage = "Maximum lenth is 20 characters for this field.")]
        [Required(ErrorMessage = "Required.")]
        public string RFC { get; set; }
        public int PaymentDay { get; set; }
        [MaxLength(100, ErrorMessage = "Maximum lenth is 500 characters for this field.")]
        [EmailAddress(ErrorMessage ="Please type a correct email address.")]
        [Required(ErrorMessage = "Required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Required.")]
        public virtual PaymentMethod PaymentMethod { get; set; }
        [Required(ErrorMessage = "Required.")]
        public virtual Currency Currency { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public virtual ClientUser CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public virtual ClientUser ModifiedBy { get; set; }

        List<SupplierContact> _list = new List<SupplierContact>();
        public IEnumerable<SupplierContact> Contacts { get { return _list; } }
        public void AddContact(SupplierContact item) { _list.Add(item); }
    }
}
