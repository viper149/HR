using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces.HR
{
    public interface IF_BAS_HRD_SECTION : IBaseService<F_BAS_HRD_SECTION>
    {
        Task<IEnumerable<F_BAS_HRD_SECTION>> GetAllFBasHrdSectionAsync();
        Task<bool> FindBySecNameAsync(string deptName,bool isBn = false);
        Task<List<F_BAS_HRD_SECTION>> GetAllSectionsAsync();
    }
}