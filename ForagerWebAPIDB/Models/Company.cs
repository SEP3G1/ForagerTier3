using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        public string PostCode { get; set; } // Udfyld
        public string Logo { get; set; }

        public bool WishDeletion { get; set; }

        public List<Employee> Employees { get; set; }
        public string ConnectionAddress { get; set; } //Udfyld // Skal kun indeholde selve ip-adressen ikke http/https.

    }
}