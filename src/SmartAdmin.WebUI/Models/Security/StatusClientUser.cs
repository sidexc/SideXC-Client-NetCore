using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SideXC.WebUI.Models.Security
{
    public class StatusClientUser
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50, ErrorMessage = "Please type a valid description.")]
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}
