using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym_DataBase.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        // navigation property 
        public ICollection<Users> Users { get; set; }
    }
}
