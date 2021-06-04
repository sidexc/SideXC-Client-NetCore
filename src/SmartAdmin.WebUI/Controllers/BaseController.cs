using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SideXC.WebUI.Data;
using System.Linq;

namespace SideXC.WebUI.Controllers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class UserAuthentication : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly ApplicationDbContext _context;
        ApplicationUser _UserLogged;

        public ApplicationUser UserLogged { get { return _UserLogged; } }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            var userEmail = filterContext.HttpContext.User.Identity.Name;

            if (!user.Identity.IsAuthenticated)
            {
                //Necesito investigar como redireccionar a pagina de error en logeo
                return;
            }
            else
            {
                var userLogged = _context.ApplicationUsers.FirstOrDefault(u => u.Email == filterContext.HttpContext.User.Identity.Name);
                _UserLogged = userLogged;
            }
        }

    }

    public class BaseController : Controller
    {


    }
}
