using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SideXC.WebUI.Models.Human_Resources;
using SideXC.WebUI.Models.Inventory;
using SideXC.WebUI.Models.Map;
using SideXC.WebUI.Models.Security;

namespace SideXC.WebUI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        //public class ApplicationDbContext : DbContext
        //{
        //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        //{
        //}


        //Human Resource
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeContact> EmployeeContacts { get; set; }
        public virtual DbSet<EmployeePositionHistory> EmployeePositionsHistory { get; set; }
        public virtual DbSet<EmployeeSalaryHistory> EmployeeSalarysHistory { get; set; }
        public virtual DbSet<EmployeeType> EmployeeTypes { get; set; }
        public virtual DbSet<Position> Positions { get; set; }        
        //Inventory
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<Employee> Hallways { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<InventoryLog> InventoriesLogs { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Material> Materials { get; set; }
        public virtual DbSet<MaterialType> MaterialTypes { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<SupplierContact> SupplierContacts { get; set; }
        public virtual DbSet<TransactionType> TransactionTypes { get; set; }
        public virtual DbSet<UnitMeassure> UnitMeassures { get; set; }
        public virtual DbSet<Warehouse> Warehouses { get; set; }
        //Map
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Neighborhood> Neighborhoods { get; set; }
        public virtual DbSet<State> States { get; set; }
        //Security
        public virtual DbSet<ClientUser> ClientUsers { get; set; }
        public virtual DbSet<ContactType> ContactTypes { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<Option> Options { get; set; }
        public virtual DbSet<Profile> Profiles { get; set; }
        public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }
        public virtual DbSet<StatusClientUser> StatusClientUsers { get; set; }
    }
}
