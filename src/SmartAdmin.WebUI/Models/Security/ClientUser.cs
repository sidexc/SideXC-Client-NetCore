using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SideXC.WebUI.Models.Security
{
    public class ClientUser
    {
        [Key]
        public int Id { get; set; }
        public Guid UID { get; set; }
        public int CompanyId { get; set; }
        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid email address.")]
        public string Email { get; set; }
        [StringLength(10, ErrorMessage = "The Max legnth is 10 characters for this field.")]
        public string Password { get; set; }
        public virtual Profile Profile { get; set; }
        public DateTime LastAccess { get; set; }
        public int FailNumberAccess { get; set; }
        public virtual StatusClientUser Status { get; set; }
        public DateTime Created { get; set; }
        public int CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
    }
}
