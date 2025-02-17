using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym_DataBase.Models
{
    public class Users
    {
        [Key]
        public int UserID { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ProfilePicture { get; set; }
        public string? MobileNo { get; set; }
        public DateTime? RegistrationDate { get; set; }

        // navigation property 
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public ICollection<Subscriptions> Subscriptions { get; set; }
        public ICollection<WorkoutPlans> MemberWorkoutPlans { get; set; } 
        public ICollection<WorkoutPlans> TrainerWorkoutPlans { get; set; } 
        public ICollection<Testimonials> Testimonials { get; set; }
        public ICollection<Reports> Reports { get; set; }
       
    }
}
