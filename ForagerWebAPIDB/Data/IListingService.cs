using System.Collections.Generic;
using System.Threading.Tasks;
using ForagerWebAPIDB.Models;

namespace ForagerWebAPIDB.Data
{
    public interface IListingService
    {
        Task<string> CreateListing(Listing listing);
        Task<string> UpdateListing(Listing listing);
        Task<Listing> GetListing(string id);
        Task<List<Listing>> GetAllListings(string parameter);
        Task<string> GetNumberOfResults(string parameter);
        Task<List<Listing>> GetListings(string parameter, string filter, int sequenceNumber);
        Task<List<string>> GetListingPostCodes();
        Task<Dictionary<string, string>> GetListingNamesAndCovers();
        Task<Product> GetProduct(string id);
        Task<List<string>> GetProductCategories();
        Task<List<Product>> GetProducts();
    }
}