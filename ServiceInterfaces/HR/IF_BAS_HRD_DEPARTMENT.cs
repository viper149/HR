using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Models;
using HRMS.ServiceInterfaces.BaseInterfaces;
using HRMS.ViewModels.HR;

namespace HRMS.ServiceInterfaces.HR
{
    public interface IF_BAS_HRD_DEPARTMENT : IBaseService<F_BAS_HRD_DEPARTMENT>
    {
        Task<bool> FindByDeptNameAsync(string deptName, bool isBn = false);
        Task<IEnumerable<F_BAS_HRD_DEPARTMENT>> GetAllFBasHrdDepartmentAsync();
        Task<FBasHrdDepartmentViewModel> GetInitObjByAsync(FBasHrdDepartmentViewModel fBasHrdDepartmentViewModel);
        Task<List<F_BAS_HRD_DEPARTMENT>> GetAllDepartmentsAsync();
    }
}