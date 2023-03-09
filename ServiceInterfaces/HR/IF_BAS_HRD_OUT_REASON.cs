using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Models;
using HRMS.ServiceInterfaces.BaseInterfaces;

namespace HRMS.ServiceInterfaces.HR
{
    public interface IF_BAS_HRD_OUT_REASON : IBaseService<F_BAS_HRD_OUT_REASON>
    {
        Task<bool> FindByOutReasonAsync(string resason);
        Task<IEnumerable<F_BAS_HRD_OUT_REASON>> GetAllFBasHrdOutReasonAsync();
    }
}
