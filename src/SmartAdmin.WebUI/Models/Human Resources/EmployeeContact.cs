using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SideXC.WebUI.Data;
using SideXC.WebUI.Models.Security;

namespace SideXC.WebUI.Models.Human_Resources
{
    public class EmployeeContact
    {
        [Key]
        public int Id { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ContactType ContactType { get; set; }
        [StringLength(100, ErrorMessage = "Please type a valid description.")]
        [MaxLength(100, ErrorMessage = "Maximum lenth is 100 characters for this field.")]
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public virtual ApplicationUser ModifiedBy { get; set; }
    }
}
