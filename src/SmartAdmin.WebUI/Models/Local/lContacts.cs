using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SideXC.WebUI.Models.Local
{
    public class lContacts
    {
        public int Id { get; set; }
        public string ContactName { get; set; }
        public string Description { get; set; }
        public int ContactTypeId { get; set; }
    }
}
