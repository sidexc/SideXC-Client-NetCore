using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SideXC.WebUI.Data
{
    public class ApplicationUser : IdentityUser<int>
    {
        public Guid UID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
