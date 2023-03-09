using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Models;
using HRMS.ServiceInterfaces.BaseInterfaces;

namespace HRMS.ServiceInterfaces.CompanyInfo
{
    public interface ICOMPANY_INFO : IBaseService<COMPANY_INFO>
    {
        Task<bool> FindByCompanyName(string companyName);
        Task<IEnumerable<COMPANY_INFO>> GetAllCompanyInfoAsync();
    }
}