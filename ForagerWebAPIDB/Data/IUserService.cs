using System.Threading.Tasks;
using ForagerWebAPIDB.Models;

namespace ForagerWebAPIDB.Data
{
    public interface IUserService
    {
        Task<User> ValidateUserAsync(string Email, string Password);

    }
}
