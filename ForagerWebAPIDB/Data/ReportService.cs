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
    }
}
