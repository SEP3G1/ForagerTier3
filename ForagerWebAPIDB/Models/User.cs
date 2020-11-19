using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ForagerWebAPIDB.Models
{
    public class User
    {
        [Key, MaxLength(16)]
        public int UserId { get; set; }
        [Required, MaxLength(32)]
        public string Name { get; set; }
        [Required]
        public int SecurityLevel { get; set; }
        [Required, MaxLength(64)]
        public string Password { get; set; }
        [Required, MaxLength(64)]
        public string Email { get; set; }
        public int CompanyId { get; set; }
    }
}
