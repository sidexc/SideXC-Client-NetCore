using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SideXC.WebUI.Models.Security;

namespace SideXC.WebUI.Models.Map
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50, ErrorMessage = "Please type a valid Street.")]
        [MaxLength(50, ErrorMessage = "Maximum lenth is 50 characters for this field.")]
        public string Street { get; set; }
        [StringLength(20, ErrorMessage = "Please type a valid Internal Number.")]
        [MaxLength(20, ErrorMessage = "Maximum lenth is 20 characters for this field.")]
        public string InternalNumber { get; set; }
        [StringLength(20, ErrorMessage = "Please type a valid External Number.")]
        [MaxLength(20, ErrorMessage = "Maximum lenth is 20 characters for this field.")]
        public string ExternalNumber { get; set; }
        public virtual Neighborhood Neighborhood { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public virtual ClientUser CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public virtual ClientUser ModifiedBy { get; set; }
    }
}
