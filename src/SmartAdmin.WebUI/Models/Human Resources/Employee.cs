using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SideXC.WebUI.Models.Security;

namespace SideXC.WebUI.Models.Human_Resources
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50, ErrorMessage = "Please type a valid Name.")]
        [MaxLength(50, ErrorMessage = "Maximum lenth is 50 characters for this field.")]
        public string Name { get; set; } 
        [StringLength(50, ErrorMessage = "Please type a valid Last Name 1.")]
        [MaxLength(50, ErrorMessage = "Maximum lenth is 50 characters for this field.")]
        public string LastName1 { get; set; }
        [StringLength(50, ErrorMessage = "Please type a valid Last name 2.")]
        [MaxLength(50, ErrorMessage = "Maximum lenth is 50 characters for this field.")]
        public string LastName2 { get; set; }
        [StringLength(1000, ErrorMessage = "The Max legnth is 1000 characters for this field.")]
        public string PhotUrl { get; set; }
        public virtual Position Position { get; set; }
        public double GrossSalary { get; set; }
        public double NetSalary { get; set; }
        public double DailySalary { get; set; }
        public double IntegratedDailySalary { get; set; }
        //public Address Address { get; set; }
        public virtual ClientUser ClientUser { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public virtual ClientUser CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public virtual ClientUser ModifiedBy { get; set; }
    }
}
