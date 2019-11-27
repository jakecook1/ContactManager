using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ContactManagerWeb.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public ContactsController(ILogger<HomeController> logger) => _logger = logger;

        public IActionResult Index()
        {
            return View();
        }
    }
}
