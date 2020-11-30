using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForagerWebAPIDB.DataAccess;
using ForagerWebAPIDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ForagerWebAPIDB.Data
{
    public class ListingService : IListingService
    {
        private ForagerDBContext ctx;

        public ListingService(ForagerDBContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<string> CreateListing(Listing listing)
        {
            //If product is not set to null the product will be added to db twice
            listing.Product = null;
            EntityEntry<Listing> newlyAdded = await ctx.listings.AddAsync(listing);
            await ctx.SaveChangesAsync();
            return newlyAdded.Entity.ListingId + "";
        }

        public async Task<string> UpdateListing(Listing listing)
        {
            //If product is not set to null the product will be added to db twice
            listing.Product = null; 
            ctx.listings.Attach(listing);
            ctx.Entry(listing).State = EntityState.Modified;
            await ctx.SaveChangesAsync();

            return listing.ListingId + "";
        }
        public async Task<List<Listing>> GetAllListings(string parameter)
        {
            IQueryable<Listing> q = ctx.listings;
            List<Listing> listings = new List<Listing>();

            if (parameter == null || parameter.Length == 0)
            {
                listings = await q.ToListAsync();
            }
            else
            {
                q.Include(l => l.Product);
                q.Include(l => l.Product.ProductCategory);             
                q.Include(l => l.Product.Name);             
                
                try
                {
                    listings = await q.Where(l =>
            l.Product.ProductCategory.Equals(parameter) ||
            l.Product.Name.Equals(parameter) ||
            l.Postcode.Equals(parameter)
            ).ToListAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error in GetAllListings");
                    Console.WriteLine(e.StackTrace);
                }
            }
            return listings.OrderByDescending(o => o.ListingId).ToList();
        }

        public async Task<Listing> GetListing(string id)
        {
            int idInt = int.Parse(id);
            Listing listing = await ctx.listings.FirstAsync(c => c.ListingId == idInt);
            listing.NumberOfViews++;
            ctx.Update(listing);
            await ctx.SaveChangesAsync();
            return listing;
        }
        public async Task<Product> GetProduct(string id)
        {
            int idInt = int.Parse(id);
            return await ctx.Products.FirstAsync(c => c.ProductId == idInt);
        }

        public async Task<List<string>> GetProductCategories()
        {
            List<Product> products = await ctx.Products.ToListAsync();
            List<string> productcategories = new List<string>();
            foreach (var p in products)
            { 
                if (!(productcategories.Contains(p.ProductCategory))) 
                    productcategories.Add(p.ProductCategory);
            }

            return productcategories;
        }

        public async Task<List<Product>> GetProducts()
        {
            return await ctx.Products.ToListAsync();
        }
    }
}