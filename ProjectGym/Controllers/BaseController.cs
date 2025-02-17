using Gym_DataBase.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace ProjectGym.Controllers
{
    public class BaseController : Controller
    {

        protected AppDbContext _dbContext;
        private readonly ILogger<HomeController> _logger;
        public BaseController(AppDbContext context , ILogger<HomeController> logger)
        {
            _dbContext = context;
            _logger = logger;
        }
        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folderName, string existingFilePath = null)
        {
            if (file == null || file.Length == 0)
                return existingFilePath;

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/{folderName}");
            Directory.CreateDirectory(uploadsFolder);
            var filePath = Path.Combine(uploadsFolder, file.FileName);

            if (!System.IO.File.Exists(filePath))
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return $"/{folderName}/{file.FileName}";
        }
    }
}
