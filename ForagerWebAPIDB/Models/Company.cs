using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ForagerWebAPIDB.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }
        public string Cvr { get; set; }
        public double TrustScore { get; set; }
        public int NumberOfVotes { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Logo { get; set; }
        public List<Employee> Employees { get; set; }
    }
}