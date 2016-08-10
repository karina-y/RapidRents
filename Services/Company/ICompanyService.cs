using System.Collections.Generic;
using RapidRents.Web.Models.Requests.Company;

namespace RapidRents.Web.Services.Company
{
    public interface ICompanyService
    {
        List<Domain.Company.Company> GetAll();
        Domain.Company.Company GetCompanyById(int Id);
        Domain.Company.Company DeleteCompanyById(int Id);
        int Insert(CompanyAddRequests model, string userId);
        void Update(CompanyUpdateRequests model);
    }
}
