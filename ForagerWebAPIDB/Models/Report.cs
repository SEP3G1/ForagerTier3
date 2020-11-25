using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForagerWebAPIDB.Models
{
    public class Report
    {
        [Key]
        public int ReportId { get; set; }
        public int ListingId { get; set; }
        public int UserId { get; set; }
        [Required]
        public string ReportType { get; set; }
        [Required]
        public string Time { get; set; }
    }
}
