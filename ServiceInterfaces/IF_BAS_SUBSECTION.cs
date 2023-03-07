using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_BAS_SUBSECTION : IBaseService<F_BAS_SUBSECTION>
    {
        Task<bool> FindBySubSectionNameAsync(string subSectionName);
        Task<IEnumerable<F_BAS_SUBSECTION>> GetSubSectionsBySectionIdAsync(int sectionId);
    }
}
