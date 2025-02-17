using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym_DataBase.Models
{
    public class Testimonials
    {
        [Key]
        public int TestimonialID { get; set; }
        public string? Content { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public Int32? ApprovalStatus { get; set; }

        // navigation property 
        public int UserId { get; set; }
        public Users User { get; set; }
    }
}
