using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym_DataBase.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50)]
        public string? UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required]
        public string? MobileNo { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
       
    }
}
