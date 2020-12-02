using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ForagerWebAPIDB.Data;
using ForagerWebAPIDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace ForagerWebAPIDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingController : ControllerBase
    {
        private IListingService listingService;

        public ListingController(IListingService listingService)
        {
            this.listingService = listingService;
        }

       [HttpGet]
       public async Task<ActionResult<List<Listing>>> GetLazyFilteredListings([FromQuery] string parameter, string filter, int sequencenumber)
       {
           if (!ModelState.IsValid)
           {
               return BadRequest(ModelState);
           }
      
           try
           {
                List<Listing> listings = new List<Listing>();
                listings = await listingService.GetListings(parameter, filter, sequencenumber);
                foreach (Listing l in listings)
                   {
                       l.Product = await listingService.GetProduct(l.ProductId + "");
                   }
               return Ok(listings);
           }
           catch (Exception e)
           {
               Console.WriteLine(e);
               return StatusCode(500, e.Message);
           }
       }

        [HttpGet]
        [Route("companyListings/{id:int}")]
        public async Task<ActionResult<List<Listing>>> GetListingsFromCompany(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                List<Listing> listings = await listingService.GetListingsFromCompany(id);
                foreach (Listing l in listings)
                {
                    l.Product = await listingService.GetProduct(l.ProductId + "");
                }
                return Ok(listings);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [Route("postcode")]
        public async Task<ActionResult<List<string>>> GetListingPostCodes()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                List<string> listingPostCodes = await listingService.GetListingPostCodes();

                return Ok(listingPostCodes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [Route("namescovers")]
        public async Task<ActionResult<List<string>>> GetListingNamesAndCovers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Dictionary<string, string> listingNamesAndCovers = await listingService.GetListingNamesAndCovers();

                return Ok(listingNamesAndCovers);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [Route("count")]
        public async Task<ActionResult<List<string>>> GetNumberOfResults([FromQuery] string parameter) //Jeg er i tvivl om dennes return value bør være "List<string>"? #patrick
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string numberOfResults = await listingService.GetNumberOfResults(parameter);
                return Ok(numberOfResults);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<Listing>> GetListing(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Listing listing = await listingService.GetListing(id);
                if (listing != null)
                    listing.Product = await listingService.GetProduct(listing.ProductId + "");
                return Ok(listing);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateListing([FromQuery] string listingAsString)
        {
            Listing listing = JsonSerializer.Deserialize<Listing>(listingAsString);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string id = await listingService.CreateListing(listing);
                return Ok(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
        [HttpPost("{Listing}")]
        public async Task<ActionResult<string>> UpdateListing(string Listing)
        {
            Listing listing = JsonSerializer.Deserialize<Listing>(Listing);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string id = await listingService.UpdateListing(listing);
                return Ok(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}