using Gym_DataBase.Data;
using Gym_DataBase.Models;
using Gym_DataBase.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProjectGym.Controllers
{
    public class TrainerController : BaseController
    {
        public TrainerController(AppDbContext context, ILogger<HomeController> logger) : base(context, logger)
        {
        }

        public IActionResult List()
        {
            var Data = _dbContext.Users.Where(x => x.RoleId == 3).AsNoTracking().ToList();

            return View(Data);
        }

        public IActionResult ListOfMember()
        {
            var Data = _dbContext.Users.Where(x => x.RoleId == 2).AsNoTracking().ToList();

            return View(Data);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TrainerViewModel trainer)
        {

            try
            {
                if (ModelState.IsValid)
                {

                    var profilePicturePath = await SaveFileAsync(trainer.ProfilePicture, "uploads");

                    var Trainer = new Users
                    {
                        UserName = trainer.Username,
                        Email = trainer.Email,
                        MobileNo = trainer.MobileNo,
                        Password = trainer.Password,
                        RegistrationDate = trainer.RegistrationDate,
                        ProfilePicture = profilePicturePath,
                        RoleId = 3
                    };

                    await _dbContext.Users.AddAsync(Trainer);
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

        public IActionResult Edit(int UserID)
        {
            try
            {
                var data = _dbContext.Users.Where(x => x.UserID == UserID).FirstOrDefault();
                if (data == null)
                {
                    return NotFound();
                }

                var TrainerViewModel = new TrainerViewModel
                {
                    UserID = data.UserID,
                    Username = data.UserName,
                    Email = data.Email,
                    Password = data.Password,
                    MobileNo = data.MobileNo,
                    ProfilePicturePath = data.ProfilePicture,
                    RegistrationDate = (DateTime)data.RegistrationDate,
                };

                return View(TrainerViewModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TrainerViewModel trainer)
        {
            try
            {

                if (trainer.ProfilePicture == null)
                {
                    ModelState.Remove("ProfilePicture");
                }

                if (ModelState.IsValid)
                {

                    var data = _dbContext.Users.Where(x => x.UserID == trainer.UserID).FirstOrDefault();

                    if (data == null)
                    {
                        return NotFound();
                    }

                    if (trainer.ProfilePicture != null)
                    {
                        var newProfilePicturePath = await SaveFileAsync(trainer.ProfilePicture, "uploads");

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

                    data.Email = trainer.Email;
                    data.MobileNo = trainer.MobileNo;
                    data.RegistrationDate = (DateTime)trainer.RegistrationDate;

                    _dbContext.Users.Update(data);
                    await _dbContext.SaveChangesAsync();

                    return RedirectToAction(nameof(List));
                }

                return View(trainer);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public IActionResult Details(int UserID)
        {
            try
            {
                var data = _dbContext.Users.Find(UserID);
                if (data == null)
                {
                    return NotFound();
                }

                var TrainerViewModel = new TrainerViewModel
                {
                    Username = data.UserName,
                    Email = data.Email,
                    MobileNo = data.MobileNo,
                    ProfilePicturePath = data.ProfilePicture,
                    RegistrationDate = (DateTime)data.RegistrationDate,
                };

                return View(TrainerViewModel);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
