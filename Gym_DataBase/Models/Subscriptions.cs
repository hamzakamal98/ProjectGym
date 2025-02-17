using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym_DataBase.Models
{
    public class Subscriptions
    {
        [Key]
        public int SubscriptionId { get; set; }
        public string? PlanName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? PaymentStatus { get; set; }

        // navigation property 

        public ICollection<Payments> Payments { get; set; }
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public Users Users { get; set; }
       
    }
}
