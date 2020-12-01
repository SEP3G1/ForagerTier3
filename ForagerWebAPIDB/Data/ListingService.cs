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
        public int lazyLoadSequenceValue = 9; //hardcode #patrick

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
            if (parameter == null || parameter == "null" || parameter.Length == 0)
            {
                listings = await q.Take(25).ToListAsync();
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

        public async Task<string> GetNumberOfResults(string parameter)
        {
            return ctx.listings.Where(l =>
            l.Product.ProductCategory.Equals(parameter) ||
            l.Product.Name.Equals(parameter) ||
            l.Postcode.Equals(parameter)
            ).Count() + "";
        }
        public async Task<List<Listing>> GetListings(string parameter, string filter, int sequenceNumber)
        {

            if ((filter == null || filter == "null") && sequenceNumber == 0)
            {
                return await GetAllListings(parameter);
            }

            IQueryable<Listing> q = ctx.listings;
            List<Listing> listings = new List<Listing>();

            if (parameter == null || parameter.Length == 0 || parameter.Equals(null))
            {
                listings = await q.Skip(sequenceNumber).Take(lazyLoadSequenceValue).ToListAsync();
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
                        q = q.OrderByDescending(l => l.Price); //For debugging #patrick, virker ikke endnu
                        break;
                    default:
                        q = q.OrderByDescending(l => l.ListingId);
                        break;

                }

                try
                {
                    listings = await q.Skip(sequenceNumber).Take(lazyLoadSequenceValue).ToListAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error in GetListings(string parameter, string filter, int sequenceNumber)");
                    Console.WriteLine(e.StackTrace);
                }
            }
            return listings;
        }

        public async Task<List<string>> GetListingPostCodes()
        {
            List<string> listingPostCodes = new List<string>();
            List<Listing> listings = new List<Listing>();

            try
            {
                listings = await ctx.listings.ToListAsync();
                listings.ForEach(l => listingPostCodes.Add(l.Postcode));
                listingPostCodes = listingPostCodes.Distinct().ToList();
            }

            catch (Exception e)
            {
                Console.WriteLine("Error in GetListingPostCodes");
                Console.WriteLine(e.StackTrace);
            }
            return listingPostCodes.OrderBy(p => p).ToList();
        }

        public async Task<Dictionary<string, string>> GetListingNamesAndCovers()
        {
            Dictionary<string, string> listingNamesAndCovers = new Dictionary<string, string>();
            List<Listing> listings = new List<Listing>();

            try
            {
                listings = await ctx.listings.Include(l => l.Product).ToListAsync();

                int count = numberOfListingsWithDistinctProductId();


                foreach (Listing l in listings)
                {

                    //       if (l.Product.ImagesString == null)
                    //   {
                    //       l.Product.ImagesString = "no image set";
                    //   }

                    string imagesString = l.Product.ImagesString;

                    try
                    {
                        listingNamesAndCovers.Add(l.Product.Name, imagesString);

                        if (listingNamesAndCovers.Count >= count)
                        {
                            break;
                        }

                    }
                    catch (ArgumentException e) // Catches exception when 'An item with the same key has already been added.'
                    {
                        continue;
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("Error in GetListingNamesAndCovers");
                Console.WriteLine(e.StackTrace);
            }
            Dictionary<string, string> listingNamesAndCoversFinal = new Dictionary<string, string>();

            // ToList() for at kunne kalde OrderBy(). ToList() igen for at kunne kalde ForEach()
            //listingNamesAndCovers.ToList().OrderBy(l => l.Key).ToList().ForEach(l => listingNamesAndCoversFinal.Add(l.Key, l.Value));
            listingNamesAndCovers.ToList().ForEach(l => listingNamesAndCoversFinal.Add(l.Key, l.Value));

            return listingNamesAndCovers;
        }

        private int numberOfListingsWithDistinctProductId()
        {
            int count = ctx.listings.GroupBy(l => l.ProductId).Distinct().Count();
            return count;
        }

        public async Task<string> DeleteListing(int listingId)
        {
            Listing toRemove = ctx.listings.First(l => l.ListingId == listingId);
            ctx.Remove(toRemove);
            await ctx.SaveChangesAsync();
            return "Success";
        }
    }
}