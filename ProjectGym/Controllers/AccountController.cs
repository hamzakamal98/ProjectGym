using Gym_DataBase.Models;
using Gym_DataBase.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Gym_DataBase.Data;
using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace ProjectGym.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(AppDbContext context, ILogger<HomeController> logger) : base(context, logger)
        {
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {

                var User = _dbContext.Users.Include(x=>x.Role).FirstOrDefault(x => x.UserName.Equals(login.UserName) && x.Password.Equals(login.Password));

                if (User != null)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, User.UserID.ToString()),
                new Claim(ClaimTypes.Name, User.UserName),
                new Claim(ClaimTypes.Role, User.Role.RoleName)
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties { IsPersistent = true };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                  new ClaimsPrincipal(claimsIdentity),
                                                  authProperties);

                    if (User.Role.RoleName == "Member")
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    Response.Cookies.Append("RoleName", User.Role.RoleName, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTime.UtcNow.AddHours(2)
                    });

                    HttpContext.Session.SetString("UserName", login.UserName);
                    HttpContext.Session.SetString("RoleName", User.Role.RoleName);

                    return RedirectToAction("Dashboard", "Admin");
                }

                ViewBag.ErrorMessage = "InValid UserName or Password";
            }

            return View();
        }


        public IActionResult Register()
        {
            return View();
        }

        // The Register only (Member)

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var Data = new Users
                    {
                        UserName = register.UserName,
                        Password = HashPassword(register.Password),
                        Email = register.Email,
                        MobileNo = register.MobileNo,
                        RegistrationDate = DateTime.Now,
                        RoleId = 2,
                    };

                    await _dbContext.Users.AddAsync(Data);
                    _dbContext.SaveChanges();
                    return RedirectToAction("Login", "Account");
                }

                return View();
            }

            catch (Exception)
            {
                throw;
            }
        }

       
        public IActionResult Logout()
        {

            HttpContext.Session.Clear();
            Response.Cookies.Delete("RoleName");
            
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            return RedirectToAction("Login", "Account");
        }
    }
}
