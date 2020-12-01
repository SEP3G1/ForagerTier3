using ForagerWebAPIDB.Data;
using ForagerWebAPIDB.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ForagerWebAPIDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private ICompanyService companyService;

        public CompanyController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Company>>> GetCompaniesToDelete()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                List<Company> companiesToDelete = await companyService.GetCompaniesToDelete();
                return Ok(companiesToDelete);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Company>> GetCompany(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Company company = await companyService.GetCompany(id);
                //listing.Product = await listingService.GetProduct(listing.ProductId + ""); //TODO ?
                return Ok(company);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateCompany([FromQuery] string companyAsString)
        {
            Company company = JsonSerializer.Deserialize<Company>(companyAsString);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string id = await companyService.CreateCompany(company);
                return Ok(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<string>> UpdateCompany([FromQuery] string companyAsString)
        {
            Company company = JsonSerializer.Deserialize<Company>(companyAsString);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string id = await companyService.UpdateCompany(company);
                return Ok(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<string>> DeleteCompanyWish(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                string toReturn = await companyService.DeleteCompanyWish(Int32.Parse(id));
                return Ok(toReturn);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{companyid}")]
        public async Task<ActionResult<string>> DeleteListing(int companyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string message = await companyService.DeleteCompany(companyId);
                return Ok(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}
