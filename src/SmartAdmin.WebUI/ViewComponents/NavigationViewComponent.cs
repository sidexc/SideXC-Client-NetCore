using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SideXC.WebUI.Models;

namespace SideXC.WebUI.ViewComponents
{
    public class NavigationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var items = NavigationModel.Seed;

            return View(items);
        }
    }
}
