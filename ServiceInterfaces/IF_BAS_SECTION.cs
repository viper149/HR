using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_BAS_SECTION : IBaseService<F_BAS_SECTION>
    {
        Task<IEnumerable<F_BAS_SECTION>> GetSectionsByDeptIdAsync(int id);
        Task<F_BAS_SECTION> GetSubSectionBySectionId(int id);
        Task<bool> FindBySectionNameAsync(string sectionName);
    }
}
