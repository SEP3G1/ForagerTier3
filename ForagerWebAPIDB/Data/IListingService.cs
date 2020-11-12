﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ForagerWebAPIDB.Models;

namespace ForagerWebAPIDB.Data
{
    public interface IListingService
    {
        Task<string> CreateListing(Listing listing);
        Task<Listing> GetListing(string id);
        Task<List<Listing>> GetAllListings();
        Task<Product> GetProduct(string id);
        Task<List<string>> GetProductCategories();
        Task<List<Product>> GetProducts();
    }
}