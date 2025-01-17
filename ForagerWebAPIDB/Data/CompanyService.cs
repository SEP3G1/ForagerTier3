﻿using ForagerWebAPIDB.DataAccess;
using ForagerWebAPIDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

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
            bool alreadyExists = ctx.Companies.Any(c => c.Cvr == company.Cvr);
            if (alreadyExists){
                return "alreadyExists";
            }
            else
            {
                company.Name = WebUtility.HtmlDecode(company.Name).Replace("+", " ");
                company.Address = WebUtility.HtmlDecode(company.Address).Replace("+", " ");
                EntityEntry<Company> newlyAdded = await ctx.Companies.AddAsync(company);
                await ctx.SaveChangesAsync();
                return newlyAdded.Entity.CompanyId + "";
            }
        }

        public async Task<Company> GetCompany(string id)
        {
            int idInt = int.Parse(id);
            Company c = await ctx.Companies.FirstAsync(c => c.CompanyId == idInt);
            c.Name = WebUtility.HtmlEncode(c.Name);
            c.Address = WebUtility.HtmlEncode(c.Address);
            return c;
        }

        public async Task<string> UpdateCompany(Company company)
        {
            company.Name = WebUtility.HtmlDecode(company.Name).Replace("+", " ");
            company.Address = WebUtility.HtmlDecode(company.Address).Replace("+", " ");
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

        public async Task<string> DeleteCompanyWish(int id)
        {
            Company companyToUpdate = ctx.Companies.First(c => c.CompanyId == id);
            companyToUpdate.WishDeletion = true;

            ctx.Companies.Update(companyToUpdate);
            await ctx.SaveChangesAsync();
            return companyToUpdate.CompanyId + "";
        }
        public async Task<string> DeleteCompany(int id)
        {
            Company toRemove = ctx.Companies.First(c => c.CompanyId == id);
            ctx.Remove(toRemove);

            List<Listing> listingsToArchive = await ctx.listings.Where(l => l.CompanyId == id && !l.IsArchived).ToListAsync();
            foreach (Listing listing in listingsToArchive)
            {
                listing.IsArchived = true;
                ctx.listings.Update(listing);
            }
            await ctx.SaveChangesAsync();
            return "Success";
        }
        
        public async Task<List<Company>> GetCompaniesToDelete()
        {
            List<Company> allCompanies = await ctx.Companies.ToListAsync();
            List<Company> companiesToDelete = new List<Company>();
            foreach (var c in allCompanies)
            {
                if (c.WishDeletion)
                {
                    c.Name = WebUtility.HtmlEncode(c.Name);
                    c.Address = WebUtility.HtmlEncode(c.Address);
                    companiesToDelete.Add(c);
                }
            }

            return companiesToDelete;
        }
    }
}
