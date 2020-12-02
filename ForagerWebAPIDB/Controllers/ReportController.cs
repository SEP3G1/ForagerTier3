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
    public class ReportController : ControllerBase
    {
        private IReportService reportService;

        public ReportController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Report>>> GetAllReports()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                List<Report> reports = await reportService.GetReports();
                return Ok(reports);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [Route("numberofreports")]
        public async Task<ActionResult<List<Report>>> GetNumberOfReports([FromQuery] string userid, string since)
        {
            long sinceLong = 0;

            if (since == "lasthour")
            {
                sinceLong = DateTime.Now.AddHours(-1).Ticks;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                int numberOfReports = await reportService.GetNumberOfReports(userid, sinceLong); //#patrick husk også timestamp
                return Ok(numberOfReports);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{Report}")]
        public async Task<ActionResult<string>> CreateListingReport(string Report)
        {
            Report report = JsonSerializer.Deserialize<Report>(Report);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string id = await reportService.CreateListingReport(report);
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
