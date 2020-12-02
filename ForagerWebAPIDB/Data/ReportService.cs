using ForagerWebAPIDB.DataAccess;
using ForagerWebAPIDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForagerWebAPIDB.Data
{
    public class ReportService : IReportService
    {
        private ForagerDBContext ctx;

        public ReportService(ForagerDBContext ctx)
        {
            this.ctx = ctx;
        }
        public async Task<string> CreateListingReport(Report report)
        {
            EntityEntry<Report> newlyAdded = await ctx.Reports.AddAsync(report);
            await ctx.SaveChangesAsync();
            return newlyAdded.Entity.ReportId + "";
        }

        public async Task<List<Report>> GetReports()
        {
            return await ctx.Reports.ToListAsync();
        }

        public async Task<int> GetNumberOfReports(string userid, long since) //+ parameter since ticks.. #patrick
        {
            int userIdInt = -1;
            try { 
            userIdInt = Int32.Parse(userid);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in GetNumberOfReports");
                Console.WriteLine(e.StackTrace);
            }
            Console.WriteLine("userIdInt: " + userIdInt);

            IQueryable<Report> q = ctx.Reports.Where(r => r.UserId == userIdInt);
            List<Report> reports = q.ToList();
            int numberOfReports = 0;


            if (since > 0) { 
                foreach(Report report in reports)
                {
                    Console.WriteLine("foreach(Report report in reports)");
                    long timeOfReport = (long)Convert.ToDouble(report.Time);
                    if(timeOfReport > since)
                    {
                        numberOfReports += 1;
                    }
                }
                // q = q.Where(r => (long)Convert.ToDouble(r.Time) >= since); virker ikke... "Translation of method 'System.Convert.ToDouble' failed." #patrick
                // https://forums.asp.net/t/2077863.aspx?Convert+ToDouble+Conversions+in+Linq

            }

            //numberOfReports = q.ToList().Count();
            Console.WriteLine("GetNumberOfReports: " + numberOfReports);
            return numberOfReports;
        }
    }
}
