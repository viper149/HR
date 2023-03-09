using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Models;
using HRMS.ServiceInterfaces.BaseInterfaces;

namespace HRMS.ServiceInterfaces.HR
{
    public interface IF_BAS_HRD_LOCATION : IBaseService<F_BAS_HRD_LOCATION>
    {
        Task<IEnumerable<F_BAS_HRD_LOCATION>> GetAllFBasHrdLocationAsync();
        Task<bool> FindByLocNameAsync(string locName);
        Task<List<F_BAS_HRD_LOCATION>> GetAllLocationsAsync();
    }
}
