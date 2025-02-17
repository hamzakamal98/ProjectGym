using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym_DataBase.Models
{
    public class WorkoutPlans
    {
        [Key]
        public int PlanID { get; set; }
        public string? PlanDetails { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // navigation property 

        public int MemberId { get; set; }
        public Users Member { get; set; }
        public int TrainerId { get; set; }
        public Users Trainer { get; set; }

    }
}
