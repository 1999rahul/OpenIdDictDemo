using Microsoft.AspNetCore.Mvc;

namespace OpenIdDictDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
