﻿using System;
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
                Console.WriteLine("parameter == null || parameter.Length == 0 - IN GetAllListings(string parameter)");
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

        public async Task<List<Listing>> GetAllListings(string parameter, string filter, int sequenceNumber)
        {
            if(filter == null && sequenceNumber == 0)
            {
                Console.WriteLine("filter == null && sequenceNumber == 0");
                return await GetAllListings(parameter);
            }

            IQueryable<Listing> q = ctx.listings;
            Console.WriteLine($"\n\n filter: {filter} \n\n");
            switch (filter)
            {
                case null: q = q.OrderByDescending(l => l.ListingId);
                    break;
                case "priceAscending": q = q.OrderBy(l => l.Price);                    
                    break;
                case "bestBeforeAscending": q = q.OrderBy(l => l.BestBefore);               
                    break;
                case "bestBeforeDescending": q = q.OrderByDescending(l => l.BestBefore);   
                    break;
                case "distanceAscending": q = q.OrderByDescending(l => l.Price); //For debugging #patrick
                    break;
            }

            List<Listing> listings = new List<Listing>();

            if (parameter == null || parameter.Length == 0)
            {
                Console.WriteLine("parameter == null || parameter.Length == 0 - IN GetAllListings(string parameter, string filter, int sequenceNumber)");
                listings = await q.Skip(sequenceNumber).Take(2).ToListAsync(); //TODO 2 er hardcode lige nu. #patrick
            }
            else
            {
                q.Include(l => l.Product);
                q.Include(l => l.Product.ProductCategory);
                q.Include(l => l.Product.Name);

                q = q.Where(l =>
l.Product.ProductCategory.Equals(parameter) ||
l.Product.Name.Equals(parameter) ||
l.Postcode.Equals(parameter)
);

                /*Når jeg har forsøgt at kalde "OrderByDescending" tidligere end dette, så virker sorteringen ikke,
mens jeg sorteringen virker fint, hvis "OrderBy" kaldes tidligere. 
Jeg tror det skyldes at "Linq to Entities does not guarantee to maintain the order established by OrderByDescending()"
som beskrevet her: https://stackoverflow.com/questions/7615237/linq-orderbydescending-first-and-the-nefarious-defaultifempty/7615289#7615289
*/
                switch (filter)
                {
                    case "priceAscending":
                        q = q.OrderBy(l => l.Price);
                        break;
                    case "bestBeforeAscending":
                        q = q.OrderBy(l => l.BestBefore);
                        break;
                    case "bestBeforeDescending":
                        q = q.OrderByDescending(l => l.BestBefore);
                        break;
                    case "distanceAscending":
                        q = q.OrderByDescending(l => l.Price); //For debugging #patrick
                        break;
                    default:
                        q = q.OrderByDescending(l => l.ListingId);
                        break;

                }

                try
                {
                   listings = await q.Skip(sequenceNumber).Take(2).ToListAsync();  ///OBS #patrick klienten skal sige hvor mange den vil have ad gangen. #getridofhardcode
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error in GetAllListings");
                    Console.WriteLine(e.StackTrace);
                }
            }
            return listings;
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