using ForagerWebAPIDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForagerWebAPIDB.Data
{
    public interface ICompanyService
    {
        Task<string> CreateCompany(Company company);
        Task<Company> GetCompany(string id);
        Task<string> UpdateCompany(Company company);
    }
}
