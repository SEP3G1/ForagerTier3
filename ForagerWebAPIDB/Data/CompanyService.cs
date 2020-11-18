using ForagerWebAPIDB.DataAccess;
using ForagerWebAPIDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForagerWebAPIDB.Data
{
    public class CompanyService : ICompanyService
    {

        private ForagerDBContext ctx;

        public CompanyService(ForagerDBContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<string> CreateCompany(Company company)
        {
            EntityEntry<Company> newlyAdded = await ctx.Companies.AddAsync(company);
            await ctx.SaveChangesAsync();
            return newlyAdded.Entity.CompanyId + "";
        }

        public async Task<Company> GetCompany(string id)
        {
            int idInt = int.Parse(id);
            return await ctx.Companies.FirstAsync(c => c.CompanyId == idInt);
        }
    }
}
