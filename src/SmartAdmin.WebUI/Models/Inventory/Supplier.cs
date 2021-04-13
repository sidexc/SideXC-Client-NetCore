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
        [StringLength(500, ErrorMessage = "Please type a valid Contact Name.")]
        [MaxLength(500, ErrorMessage = "Maximum lenth is 500 characters for this field.")]
        public string ContactName { get; set; }
        public virtual Address Address { get; set; }
        [StringLength(20, ErrorMessage = "Please type a valid RFC.")]
        [MaxLength(20, ErrorMessage = "Maximum lenth is 20 characters for this field.")]
        public string RFC { get; set; }
        public int PaymentDay { get; set; }
        [StringLength(100, ErrorMessage = "Please type a valid Contact Name.")]
        [MaxLength(100, ErrorMessage = "Maximum lenth is 500 characters for this field.")]
        [EmailAddress(ErrorMessage ="Please type a correct email address.")]
        public string Email { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public Currency Currrency { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public virtual ClientUser CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public virtual ClientUser ModifiedBy { get; set; }
    }
}
