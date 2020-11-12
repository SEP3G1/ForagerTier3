using ForagerWebAPIDB.Models;
using Microsoft.EntityFrameworkCore;

namespace ForagerWebAPIDB.DataAccess
{
    public class ForagerDBContext : DbContext
    {
        public DbSet<Listing> listings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Company> Companies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data source = forager.db");
        }
    }
}