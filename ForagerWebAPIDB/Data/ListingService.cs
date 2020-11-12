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

        public async Task<List<Listing>> GetAllListings()
        {
            List<Listing> listings = await ctx.listings.ToListAsync();

            return listings.OrderByDescending(o => o.ListingId).ToList();
        }

        public async Task<Listing> GetListing(string id)
        {
            int idInt = int.Parse(id);
            return await ctx.listings.FirstAsync(c => c.ListingId == idInt);
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