using Gym_DataBase.Data;
using Gym_DataBase.Models;
using Microsoft.AspNetCore.Mvc;
using ProjectGym.Models;
using System.Diagnostics;

namespace ProjectGym.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(AppDbContext context, ILogger<HomeController> logger) : base(context, logger)
        {
        }

        public IActionResult Index()
        {

            List<Subscriptions> subscriptions = _dbContext.Subscriptions.ToList();
            return View(subscriptions);

        }

        public IActionResult IndexMember()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
            {
                List<Subscriptions> subscriptions = _dbContext.Subscriptions.ToList();
                return View(subscriptions);
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
