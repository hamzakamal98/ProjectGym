using Gym_DataBase.Data;
using Gym_DataBase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ProjectGym.Controllers
{
    public class WorkoutPlansController : BaseController
    {
        public WorkoutPlansController(AppDbContext context, ILogger<HomeController> logger) : base(context, logger)
        {
        }

        public IActionResult List()
        {
            var data = _dbContext.WorkoutPlans.Include(x => x.Trainer).Include(x => x.Member).ToList();

            ViewBag.Member = _dbContext.Users.Where(x => x.RoleId == 2).Select(x => new SelectListItem
            {
                Value = x.UserID.ToString(),
                Text = x.UserName

            }).ToList();

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] List<WorkoutPlans> workoutPlansList)
        {
            try
            {

                if (workoutPlansList.Count > 0 || workoutPlansList == null)
                {
                    var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    int UserID = Convert.ToInt32(adminId);

                    foreach (var item in workoutPlansList)
                    {
                        item.TrainerId = UserID;
                    };

                    _dbContext.WorkoutPlans.AddRange(workoutPlansList);
                    await _dbContext.SaveChangesAsync();
                    return Json(new { success = true, message = "تمت إضافة الخطة بنجاح!" });
                }

                return Json(new { success = false, message = "حدث خطأ أثناء الإضافة." });
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
