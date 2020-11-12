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
    public class ProductController : ControllerBase
    {
        private IListingService listingService;

        public ProductController(IListingService listingService)
        {
            this.listingService = listingService;
        }

        [HttpGet]
        public async Task<ActionResult<Product>> GetAllProducts()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                List<Product> products = await listingService.GetProducts();
                return Ok(products);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
        
    }
}
