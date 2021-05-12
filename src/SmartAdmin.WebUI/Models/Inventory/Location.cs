using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SideXC.WebUI.Models.Security;

namespace SideXC.WebUI.Models.Inventory
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        [StringLength(5, ErrorMessage = "Please type a valid Short description.")]
        [MaxLength(5, ErrorMessage = "Maximum lenth is 5 characters for this field.")]
        [Required(ErrorMessage = "Required.")]
        public string ShortDescription { get; set; }
        [StringLength(100, ErrorMessage = "Please type a valid description.")]
        [MaxLength(100, ErrorMessage = "Maximum lenth is 100 characters for this field.")]
        [Required(ErrorMessage = "Required.")]
        public string Description { get; set; }
        public virtual Hallway Hallway { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public virtual ClientUser CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public virtual ClientUser ModifiedBy { get; set; }
    }
}
