using Gym_DataBase.Data;
using Gym_DataBase.Models;
using Microsoft.AspNetCore.Mvc;

namespace ProjectGym.Controllers
{
    public class PaymentsController : BaseController
    {
        public PaymentsController(AppDbContext context, ILogger<HomeController> logger) : base(context, logger)
        {
        }

        public IActionResult CreatePayments(int Id)
        {
            Payments payments = _dbContext.Payments.Where(x => x.SubscriptionId == Id).FirstOrDefault();
            ViewBag.Id = Id;
            return View(payments);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayments([FromBody] Payments payments)
        {
            try
            {

                _dbContext.Payments.Add(payments);
                await _dbContext.SaveChangesAsync();
                return Json(new { success = true, message = "Payment created successfully", data = payments });

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
