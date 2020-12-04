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
            List<Report> reports = await ctx.Reports.ToListAsync();
            List<Listing> listingsToGet = await ctx.listings.ToListAsync();
            List<Report> reportsToShow = new List<Report>();
            foreach(Report r in reports)
            {
                foreach(Listing l in listingsToGet) 
                {
                    if (r.ListingId == l.ListingId && !l.IsArchived)
                        reportsToShow.Add(r);
                }
            }
            return reportsToShow;
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
                    long timeOfReport = (long)Convert.ToDouble(report.Time);
                    if(timeOfReport > since)
                        numberOfReports += 1;
                }
                // q = q.Where(r => (long)Convert.ToDouble(r.Time) >= since); virker ikke... "Translation of method 'System.Convert.ToDouble' failed." #patrick
                // https://forums.asp.net/t/2077863.aspx?Convert+ToDouble+Conversions+in+Linq

            }

            return numberOfReports;
        }
    }
}
