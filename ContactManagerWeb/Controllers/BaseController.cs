using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using ContactManagerWeb.Models;

namespace ContactManagerWeb.Controllers
{
    public class BaseController : Controller
    {
        private readonly UserManager<User> _userManager;

        public BaseController(UserManager<User> userManager) => _userManager = userManager;

        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);
        }
    }
}
