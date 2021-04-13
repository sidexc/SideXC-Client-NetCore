using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SideXC.WebUI.Areas.Authorization.Pages
{
    [Authorize]
    public class UserModel : PageModel
    {
    }
}
