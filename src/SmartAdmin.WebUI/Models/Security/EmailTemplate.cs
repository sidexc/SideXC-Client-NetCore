using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SideXC.WebUI.Models.Enumerator;

namespace SideXC.WebUI.Models.Security
{
    public class EmailTemplate
    {
        [Key]
        public int Id { get; set; }
        public eTypeEmail TypeEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool Active { get; set; }
    }
}
