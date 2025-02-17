using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym_DataBase.ViewModel
{
    public class UserViewModel
    {
        public int UserID { get; set; }
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }
        [Required]
        public string MobileNo { get; set; }
        [Required]
        public DateTime RegistrationDate { get; set; }

        public IFormFile ProfilePicture { get; set; }
        public string ProfilePicturePath { get; set; } = "";
    }
}
