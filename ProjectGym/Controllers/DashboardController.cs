using Microsoft.AspNetCore.Mvc;

namespace ProjectGym.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
