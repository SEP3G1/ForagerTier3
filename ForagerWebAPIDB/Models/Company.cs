using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ForagerWebAPIDB.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }
        public string Cvr { get; set; } // Udfyld
        public double TrustScore { get; set; }
        public int NumberOfVotes { get; set; }
        public string Name { get; set; } // Udfyld
        public string Address { get; set; } // Udfyld
        public string Postcode { get; set; } // Udfyld
        public string Logo { get; set; }
        public List<Employee> Employees { get; set; }
        public string ConnectionAddress { get; set; } //Udfyld

    }
}