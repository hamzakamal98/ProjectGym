using Gym_DataBase.Data;
using Gym_DataBase.Models;
using Gym_DataBase.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.DependencyResolver;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectGym.Controllers
{
    public class AdminController : BaseController
    {
        public AdminController(AppDbContext context, ILogger<HomeController> logger) : base(context, logger)
        {
        }
        // Use ViewModel (TotalMembers , ActiveSubscriptions ,TotalRevenue)  The Best 
        public IActionResult Dashboard()
        {
            if (User.IsInRole("Admin"))
            {
                var statistics = new 
                {
                    TotalMembers = _dbContext.Users.Count(),
                    ActiveSubscriptions = _dbContext.Subscriptions.Count(s => s.PaymentStatus == true),
                    TotalRevenue = _dbContext.Payments.Sum(p => p.Amount ?? 0)
                };
                return View(statistics);
            }

            return View();  
        }

        public async Task<IActionResult> ManageSubscriptions(DateTime? startDate, DateTime? endDate)
        {

            var subscriptions = _dbContext.Subscriptions.Include(x => x.Users).AsQueryable();

            if (startDate.HasValue && endDate.HasValue)
            {
                subscriptions = subscriptions.Where(s => s.StartDate >= startDate && s.EndDate <= endDate);
            }

            return View(await subscriptions.ToListAsync());
        }
        #region EditSubscription
        public IActionResult EditSubscription(int SubID)
        {
            try
            {
                var data = _dbContext.Subscriptions.Where(x => x.SubscriptionId == SubID).FirstOrDefault();

                if (data == null)
                {
                    return NotFound();
                }
                var Subscription = new SubscriptionViewModel
                {
                    SubscriptionId = data.SubscriptionId,
                    UserID = data.UserID,
                    PlanName = data.PlanName,
                    StartDate = data.StartDate,
                    EndDate = data.EndDate,
                    PaymentStatus = (bool)data.PaymentStatus
                };

                return View(Subscription);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditSubscription(SubscriptionViewModel subscriptions)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _dbContext.Subscriptions.Where(x => x.SubscriptionId == subscriptions.SubscriptionId).FirstOrDefault();

                    if (data == null)
                    {
                        return NotFound();
                    }

                    data.PlanName = subscriptions.PlanName;
                    data.StartDate = subscriptions.StartDate;
                    data.EndDate = subscriptions.EndDate;
                    data.PaymentStatus = subscriptions.PaymentStatus;

                    _dbContext.Subscriptions.Update(data);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(ManageSubscriptions));
                }

                return View(subscriptions);
            }

            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region ProfileAdmin
        public async Task<IActionResult> Profile()
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var admin = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserID.ToString() == adminId);

            var UserViewModel = new UserViewModel
            {
                UserID = admin.UserID,
                Username = admin.UserName,
                Email = admin.Email,
                MobileNo = admin.MobileNo,
                ProfilePicturePath = admin.ProfilePicture,
                RegistrationDate = (DateTime)admin.RegistrationDate,
            };

            ViewBag.UserId = adminId;
            if (admin == null)
            {
                return NotFound();
            }

            return View(UserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(UserViewModel model)
        {
            try
            {
                if (model.ProfilePicture == null)
                {
                    ModelState.Remove("ProfilePicture");

                }

                if (ModelState.IsValid)
                {
                    var user = _dbContext.Users.Where(u => u.UserID == model.UserID).FirstOrDefault();
                    user.UserName = model.Username;
                    user.Email = model.Email;

                    if (model.ProfilePicture != null)
                    {
                        var newProfilePicturePath = await SaveFileAsync(model.ProfilePicture, "uploads");

                        if (!string.IsNullOrEmpty(user.ProfilePicture))
                        {
                            user.ProfilePicture = newProfilePicturePath;
                        }
                        user.ProfilePicture = newProfilePicturePath;
                    }

                    _dbContext.Users.Update(user);
                    _dbContext.SaveChanges();
                    return RedirectToAction(nameof(Dashboard));
                }

                return View(model);
            }

            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        // Testimonials
        public IActionResult ApproveTestimonials()
        {
            // Eger Looding 
            var data = _dbContext.Testimonials.Include(x => x.User).ToList();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveTestimonial(int TestimonialID, string value)
        {
            try
            {
                var testimonial = await _dbContext.Testimonials.FirstOrDefaultAsync(t => t.TestimonialID == TestimonialID);

                if (testimonial != null)
                {
                    if (value == "Approve")
                    {
                        testimonial.ApprovalStatus = 1;
                    }
                    else if (value == "Reject")
                    {
                        testimonial.ApprovalStatus = 2; 
                    }

                    await _dbContext.SaveChangesAsync();
                    return Ok();
                }

                return BadRequest("Testimonial not found");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
