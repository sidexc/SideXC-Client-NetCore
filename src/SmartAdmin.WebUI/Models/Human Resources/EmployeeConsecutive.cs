using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SideXC.WebUI.Data;
using SideXC.WebUI.Models.Security;

namespace SideXC.WebUI.Models.Human_Resources
{
    public class EmployeeConsecutive
    {
        [Key]
        public int Id { get; set; }
        public int Folio { get; set; }
        public DateTime Created { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public virtual ApplicationUser ModifiedBy { get; set; }
    }
}
