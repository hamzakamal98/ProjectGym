using Gym_DataBase.Data;
using Gym_DataBase.Models;
using Gym_DataBase.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectGym.Controllers
{
    public class MemberController : BaseController
    {
        public MemberController(AppDbContext context, ILogger<HomeController> logger) : base(context, logger)
        {
        }

        public IActionResult List()
        {
            var Data = _dbContext.Users.Where(x => x.RoleId == 2).AsNoTracking().ToList();

            return View(Data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MemberViewModel member)
        {

            try
            {
                if (ModelState.IsValid)
                {

                    var profilePicturePath = await SaveFileAsync(member.ProfilePicture, "uploads");
                    var data = new Users
                    {
                        UserName = member.Username,
                        Email = member.Email,
                        Password = member.Password,
                        MobileNo = member.MobileNo,
                        RegistrationDate = member.RegistrationDate,
                        ProfilePicture = profilePicturePath,
                        RoleId = 2
                    };

                    await _dbContext.Users.AddAsync(data);
                    _dbContext.SaveChanges();
                    return RedirectToAction(nameof(List));
                }

                return View();
            }

            catch (Exception)
            {

                throw;
            }
        }
        #region EditMember
        public IActionResult Edit(int UserID)
        {
            try
            {
                var data = _dbContext.Users.Where(x => x.UserID == UserID).FirstOrDefault();
                if (data == null)
                {
                    return NotFound();
                }

                var memberViewModel = new MemberViewModel
                {
                    UserID = data.UserID,
                    Username = data.UserName,
                    Email = data.Email,
                    Password = data.Password,
                    MobileNo = data.MobileNo,
                    ProfilePicturePath = data.ProfilePicture,
                    RegistrationDate = (DateTime)data.RegistrationDate,
                };

                return View(memberViewModel);
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MemberViewModel member)
        {
            try
            {

                if (member.ProfilePicture == null)
                {
                    ModelState.Remove("ProfilePicture");
                }

                if (ModelState.IsValid)
                {

                    var data = _dbContext.Users.Where(x => x.UserID == member.UserID).FirstOrDefault();

                    if (data == null)
                    {
                        return NotFound();
                    }

                    if (member.ProfilePicture != null)
                    {
                        var newProfilePicturePath = await SaveFileAsync(member.ProfilePicture, "uploads");

                        if (!string.IsNullOrEmpty(data.ProfilePicture))
                        {
                            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", data.ProfilePicture.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        data.ProfilePicture = newProfilePicturePath;
                    }

                    data.Email = member.Email;
                    data.MobileNo = member.MobileNo;
                    data.RegistrationDate = (DateTime)member.RegistrationDate;

                    _dbContext.Users.Update(data);
                    await _dbContext.SaveChangesAsync();

                    return RedirectToAction(nameof(List));
                }

                return View(member);
            }

            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        public IActionResult Details(int UserID)
        {
            try
            {
                var data = _dbContext.Users.Find(UserID);
                if (data == null)
                {
                    return NotFound();
                }

                var memberViewModel = new MemberViewModel
                {
                    Username = data.UserName,
                    Email = data.Email,
                    MobileNo = data.MobileNo,
                    ProfilePicturePath = data.ProfilePicture,
                    RegistrationDate = (DateTime)data.RegistrationDate,
                };

                return View(memberViewModel);
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        public IActionResult Delete(int UserID)
        {
            try
            {
                var data = _dbContext.Users.Find(UserID);
                ViewBag.UserName = data.UserName;
                return View(data);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task <IActionResult> Delete(Users users)
        {
            try
            {
                _dbContext.Users.Remove(users);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }

            catch (Exception)
            {
                throw;
            }
        }

        private async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/{folderName}");
            Directory.CreateDirectory(uploadsFolder);
            var filePath = Path.Combine(uploadsFolder, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/{folderName}/{file.FileName}";
        }
    }
}
