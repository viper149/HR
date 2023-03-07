using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces.CompanyInfo
{
    public interface ICOMPANY_INFO : IBaseService<COMPANY_INFO>
    {
        Task<bool> FindByCompanyName(string companyName);
        Task<IEnumerable<COMPANY_INFO>> GetAllCompanyInfoAsync();
    }
}