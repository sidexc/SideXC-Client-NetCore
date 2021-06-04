using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SideXC.WebUI.Controllers.Inventory
{
    public class InventorySummary
    {
        public int Id { get; set; }
        public string Material { get; set; }
        public string Description { get; set; }
        public double MOQ { get; set; }
        public double QuantityAvailable { get; set; }
        public decimal StandarCost { get; set; }
        public decimal SalePrice { get; set; }
        public double Difference { get { return QuantityAvailable - MOQ; } }
        public DateTime Modified { get; set; }
        public string ModifiedBy { get; set; }
    }
}
