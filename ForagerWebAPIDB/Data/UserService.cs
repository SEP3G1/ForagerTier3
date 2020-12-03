using ForagerWebAPIDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForagerWebAPIDB.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ForagerWebAPIDB.Data
{
    public class UserService : IUserService
    {
        private User user;
        private List<User> users;
        private ForagerDBContext ctx;
        

        public UserService(ForagerDBContext ctx)
        {
            this.ctx = ctx;
            //Roles:
            //"User" = Can controll own ToDos 
            //"Admin" = Can controll all ToDos

            //SecurityLevel:
            //1 = Can view ToDos determined by Role
            //2 = Cam view, create and delete ToDos determined by Role
        }

        public async Task<string> CreateUserAsync(User user)
        {
            EntityEntry<User> newlyAdded = await ctx.Users.AddAsync(user);
            await ctx.SaveChangesAsync();
            return newlyAdded.Entity.UserId + "";
        }

        public async Task<User> GetUserAsync(int Id)
        {
            return await ctx.Users.FirstOrDefaultAsync(u => u.UserId == Id);
        }

        public async Task<User> ValidateUserAsync(string Email, string password)
        {
            User first = await ctx.Users.FirstAsync(user => user.Email.Equals(Email));
            if (first == null)
            {
                throw new Exception("User not found");
            }

            if (!first.Password.Equals(password))
            {
                throw new Exception("Incorrect password");
            }
            user = first;
            return first;
        }

    }
}
