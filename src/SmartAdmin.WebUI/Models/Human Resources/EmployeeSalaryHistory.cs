using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SideXC.WebUI.Models.Security;

namespace SideXC.WebUI.Models.Human_Resources
{
    public class EmployeeSalaryHistory
    {
        [Key]
        public int Id { get; set; }
        public virtual Employee Employee { get; set; }
        public double GrossSalary { get; set; }
        public double NetSalary { get; set; }
        public double DailySalary { get; set; }
        public double IntegratedDailySalary { get; set; }
        [MaxLength(5000, ErrorMessage = "Maximum lenth is 5000 characters for this field.")]
        public string Comments { get; set; }
        public DateTime Created { get; set; }
        public virtual ClientUser CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public virtual ClientUser ModifiedBy { get; set; }
    }
}
