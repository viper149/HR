using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Models;
using HRMS.ServiceInterfaces.BaseInterfaces;
using HRMS.ViewModels.HR;

namespace HRMS.ServiceInterfaces
{
    public interface IF_HRD_EMPLOYEE : IBaseService<F_HRD_EMPLOYEE>
    {
        Task<IEnumerable<F_HRD_EMPLOYEE>> GetAllFHrdEmployeeAsync();
        Task<FHrdEmployeeViewModel> GetInitObjByAsync(FHrdEmployeeViewModel fHrdEmployeeViewModel);
        Task<IEnumerable<F_BAS_HRD_DIVISION>> GetDivByNationIdAsync(int nationId);
        Task<IEnumerable<F_BAS_HRD_DISTRICT>> GetDistByDivIdAsync(int divId);
        Task<IEnumerable<F_BAS_HRD_THANA>> GetThanaByDistIdAsync(int distId);
        Task<IEnumerable<F_BAS_HRD_UNION>> GetUnionByThanaIdAsync(int thanaId);
        Task<FHrdEmployeeViewModel> GetInitDetailsObjEduByAsync(FHrdEmployeeViewModel fHrEmployeeViewModel);
        Task<bool> FindByValueAsync(string value, string type);
    }
}
