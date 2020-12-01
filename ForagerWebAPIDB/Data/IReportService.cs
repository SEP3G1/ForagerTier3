using ForagerWebAPIDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForagerWebAPIDB.Data
{
    public interface IReportService
    {
        Task<string> CreateListingReport(Report report);
        Task<List<Report>> GetReports();
    }
}
