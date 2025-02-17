using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym_DataBase.Dto
{
    public class MemberDto
    {

        public int UserID { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? MobileNo { get; set; }
        public DateTime? RegistrationDate { get; set; }
    }
}
