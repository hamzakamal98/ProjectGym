using Gym_DataBase.Data;
using Gym_DataBase.Dto;
using Gym_DataBase.Models;
using Gym_DataBase.Repository;
using Gym_DataBase.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace WebProjectGym_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MemberController : ControllerBase
    {

        private readonly IGenericRepository<Users> _usersRepository;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public MemberController(IGenericRepository<Users> usersRepository, AppDbContext context, IConfiguration configuration)
        {
            _usersRepository = usersRepository;
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            try
            {
                var member = await _context.Users.Where(x=>x.RoleId == 2).FirstOrDefaultAsync(m => m.UserName == model.UserName);
                if (member == null || !VerifyPassword(model.Password, member.Password))
                    return Unauthorized(new { message = "Incorrect email or password" });

                var jwtSettings = _configuration.GetSection("JwtToken").Get<JwtTokenSettings>();

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
            new Claim(ClaimTypes.NameIdentifier, member.UserName.ToString()),
            new Claim(ClaimTypes.Email, member.Email),
            new Claim(ClaimTypes.Role, "Member") 
        }),
                    Expires = DateTime.UtcNow.AddMinutes(jwtSettings.TokenExpiry),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);


                return Ok(new { message = "You have successfully logged in!" ,token = jwtToken });
            }
            catch (Exception)
            {

                throw;
            }

        }

        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            return HashPassword(enteredPassword) == storedPassword;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            try
            {
                if (await _context.Users.AnyAsync(m => m.Email == model.Email))
                    return BadRequest(new { message = "Email already exists" });

                var hashedPassword = HashPassword(model.Password);
                var Data = new Users
                {
                    UserName = model.UserName,
                    Password = HashPassword(model.Password),
                    Email = model.Email,
                    MobileNo = model.MobileNo,
                    RegistrationDate = DateTime.Now,
                    RoleId = 2,
                };

                await _usersRepository.SaveAsync(Data);
                return Ok(new { message = "Registration successful!" });
            }

            catch (Exception)
            {

                throw;
            }
        }

       
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var member = await _usersRepository.ListAsync();
            var filteredMembers = member.Where(x=>x.RoleId == 2).Select(x=> new MemberDto
            {
                UserID = x.UserID,
                UserName = x.UserName,
                Email = x.Email,
                MobileNo = x.MobileNo,
                RegistrationDate = x.RegistrationDate,

            }).ToList();
            if (filteredMembers == null || !filteredMembers.Any()) return NotFound();
            return Ok(filteredMembers);
            
        }



        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] MemberDto model)
        {

            var member = await _usersRepository.GetByIdAsync(id);
            if (member == null) return NotFound();

            member.UserName = model.UserName;
            member.Email = model.Email;
            member.MobileNo = model.MobileNo;
            member.RegistrationDate = model.RegistrationDate;

            _usersRepository.UpdateAsync(member);
            return Ok(new { message = "Profile updated successfully!" });
        }

        [HttpGet("{id}")]
      //  [Authorize]
        public async Task<IActionResult> GetMemberships(int id)
        {
            var memberships = await _context.Subscriptions.Where(x=>x.UserID == id)
                .Select(m => new
                {
                    m.Users.UserName,
                    m.PlanName,
                    m.StartDate,
                    m.EndDate,
                    m.PaymentStatus
                })
                .ToListAsync();

            return Ok(memberships);
        }


        private string HashPassword(string password)
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
    }
}
