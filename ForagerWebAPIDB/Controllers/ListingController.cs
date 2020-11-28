﻿using System;
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
      // [HttpGet]
      //  public async Task<ActionResult<List<Listing>>> GetAllListings([FromQuery] string parameter)
      //  {
      //      if (!ModelState.IsValid)
      //      {
      //          return BadRequest(ModelState);
      //      }
      //
      //      try
      //      {
      //          List<Listing> listings = await listingService.GetAllListings(parameter);
      //          foreach(Listing l in listings)
      //          {
      //              l.Product = await listingService.GetProduct(l.ProductId + "");
      //          }
      //          return Ok(listings);
      //      }
      //      catch (Exception e)
      //      {
      //          Console.WriteLine(e);
      //          return StatusCode(500, e.Message);
      //      }
      //  }

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
                if (filter == null && sequencenumber == 0)
                {
                    Console.WriteLine("(filter == null && sequencenumber == 0)       IN         ListingController");
                    listings = await listingService.GetAllListings(parameter);
                }
                else
                {
                    Console.WriteLine("NOT (filter == null && sequencenumber == 0)       IN         ListingController. filter: " + filter + " + seqnumber: " + sequencenumber);
                    listings = await listingService.GetAllListings(parameter, filter, sequencenumber);
                }
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