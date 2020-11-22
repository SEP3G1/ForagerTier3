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

        public async Task<string> UpdateCompany(Company company)
        {
            Company companyToUpdate = ctx.Companies.First(c => c.CompanyId == company.CompanyId);
            companyToUpdate.Cvr = company.Cvr;
            companyToUpdate.PostCode = company.PostCode;
            companyToUpdate.Name = company.Name;
            companyToUpdate.ConnectionAddress = company.ConnectionAddress;
            companyToUpdate.Address = company.Address;

            ctx.Companies.Update(companyToUpdate);
            await ctx.SaveChangesAsync();
            return companyToUpdate.CompanyId + "";
        }
    }
}
