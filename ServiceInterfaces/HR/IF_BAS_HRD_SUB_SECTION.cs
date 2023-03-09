using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Models;
using HRMS.ServiceInterfaces.BaseInterfaces;

namespace HRMS.ServiceInterfaces.HR
{
    public interface IF_BAS_HRD_SUB_SECTION : IBaseService<F_BAS_HRD_SUB_SECTION>
    {
        Task<IEnumerable<F_BAS_HRD_SUB_SECTION>> GetAllFBasHrdSubSectionAsync();
        Task<bool> FindBySubSecNameAsync(string subSecName, bool isBn = false);
        Task<List<F_BAS_HRD_SUB_SECTION>> GetAllSubSectionsAsync();
    }
}