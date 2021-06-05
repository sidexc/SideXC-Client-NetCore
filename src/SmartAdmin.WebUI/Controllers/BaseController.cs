using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SideXC.WebUI.Data;
using System.Linq;

namespace SideXC.WebUI.Controllers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class Authorization : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            //var user = filterContext.HttpContext.User;
            //var userEmail = filterContext.HttpContext.User.Identity.Name;

            //if (!user.Identity.IsAuthenticated)
            //{
            //    //Necesito investigar como redireccionar a pagina de error en logeo
            //    return;
            //}
        }
    }

    public class BaseController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ApplicationUser UserLogged { get {
                var email = "soporte@sidexc.com";// HttpContext.User.Identity.Name;
                var userLogged = _dbContext.ApplicationUsers.FirstOrDefault(u => u.Email == email);
                return userLogged;
            }
        }
    }
}
