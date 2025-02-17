using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym_DataBase.Models
{
    public class Reports
    {
        [Key]
        public int ReportID { get; set; }
        public bool? ReportType { get; set; }
        public DateTime? ReportDate { get; set; }
        public string? Description { get; set; }

        // navigation property 
        public int AdminID { get; set; }
        [ForeignKey("AdminID")]
        public Users Users { get; set; }
    }
}
