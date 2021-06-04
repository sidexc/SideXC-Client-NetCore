using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SideXC.WebUI.Data;
using SideXC.WebUI.Models.Map;
using SideXC.WebUI.Models.Security;

namespace SideXC.WebUI.Models.Human_Resources
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50, ErrorMessage = "Please type a valid Name.")]
        [MaxLength(50, ErrorMessage = "Maximum lenth is 50 characters for this field.")]
        [Required(ErrorMessage = "* El nombre es requerido")]
        public string Name { get; set; } 
        [StringLength(50, ErrorMessage = "Please type a valid Last Name 1.")]
        [MaxLength(50, ErrorMessage = "Maximum lenth is 50 characters for this field.")]
        [Required(ErrorMessage = "* El Apellido Paterno es requerido")]
        public string LastName1 { get; set; }
        [StringLength(50, ErrorMessage = "Please type a valid Last name 2.")]
        [MaxLength(50, ErrorMessage = "Maximum lenth is 50 characters for this field.")]
        [Required(ErrorMessage = "* El apellido Materno es requerido")]
        public string LastName2 { get; set; }
        [StringLength(50, ErrorMessage = "Please type a valid Employee Number.")]
        public string EmployeeNumber { get; set; }
        [StringLength(1000, ErrorMessage = "The Max legnth is 1000 characters for this field.")]
        public string PhotUrl { get; set; }
        public virtual Position Position { get; set; }
        public double GrossSalary { get; set; }
        public double NetSalary { get; set; }
        public double DailySalary { get; set; }
        public double IntegratedDailySalary { get; set; }
        public virtual Address Address { get; set; }
        public virtual ApplicationUser ClientUser { get; set; }
        public bool Active { get; set; }

        List<EmployeeContact> _list = new List<EmployeeContact>();
        public IEnumerable<EmployeeContact> Contacts { get { return _list; } }
        public void AddContact(EmployeeContact item) { _list.Add(item); }

        public DateTime Created { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public virtual ApplicationUser ModifiedBy { get; set; }
    }
}
