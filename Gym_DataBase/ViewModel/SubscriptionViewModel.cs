using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym_DataBase.ViewModel
{
    public class SubscriptionViewModel
    {
        [Key]
        public int SubscriptionId { get; set; }
        public string? PlanName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool PaymentStatus { get; set; }
        public int UserID { get; set; }

    }
}
