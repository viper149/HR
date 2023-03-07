using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.HR;

namespace DenimERP.ServiceInterfaces.HR
{
    public interface IF_BAS_HRD_DEPARTMENT : IBaseService<F_BAS_HRD_DEPARTMENT>
    {
        Task<bool> FindByDeptNameAsync(string deptName, bool isBn = false);
        Task<IEnumerable<F_BAS_HRD_DEPARTMENT>> GetAllFBasHrdDepartmentAsync();
        Task<FBasHrdDepartmentViewModel> GetInitObjByAsync(FBasHrdDepartmentViewModel fBasHrdDepartmentViewModel);
        Task<List<F_BAS_HRD_DEPARTMENT>> GetAllDepartmentsAsync();
    }
}