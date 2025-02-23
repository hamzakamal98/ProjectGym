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


        public class Builder
        {
            private readonly MemberDto _user = new MemberDto();

            public Builder SetUsername(string username)
            {
                _user.UserName  = username;
                return this;
            }

            public Builder SetEmail(string email)
            {
                _user.Email = email;
                return this;
            }

            public Builder SetAge(string age)
            {
                _user.MobileNo = age;
                return this;
            }

            public MemberDto Build()
            {
                return _user;
            }
        }


    }
}
