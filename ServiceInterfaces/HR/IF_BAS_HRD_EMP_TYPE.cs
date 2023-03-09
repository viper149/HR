using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Models;
using HRMS.ServiceInterfaces.BaseInterfaces;

namespace HRMS.ServiceInterfaces.HR
{
    public interface IF_BAS_HRD_EMP_TYPE : IBaseService<F_BAS_HRD_EMP_TYPE>
    {
        Task<bool> FindByEmpTypeAsync(string typeName);
        Task<IEnumerable<F_BAS_HRD_EMP_TYPE>> GetAllFBasHrdEmpTypeAsync();
        Task<List<F_BAS_HRD_EMP_TYPE>> GetAllEmpTypesAsync();
    }
}
