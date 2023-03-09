using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Models;
using HRMS.ServiceInterfaces.BaseInterfaces;

namespace HRMS.ServiceInterfaces.HR
{
    public interface IF_BAS_HRD_SHIFT : IBaseService<F_BAS_HRD_SHIFT>
    {
        Task<bool> FindByShiftAsync(string shift);
        Task<IEnumerable<F_BAS_HRD_SHIFT>> GetAllFBasHrdShiftAsync();
    }
}
