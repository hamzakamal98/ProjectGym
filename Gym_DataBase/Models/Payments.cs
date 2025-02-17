using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym_DataBase.Models
{
    public class Payments
    {
        [Key]
        public int PaymentID { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? PaymentType { get; set; }

        // navigation property 
        public int SubscriptionId { get; set; }
        [ForeignKey("SubscriptionId")]
        public Subscriptions Subscription { get; set; }
    }
}
