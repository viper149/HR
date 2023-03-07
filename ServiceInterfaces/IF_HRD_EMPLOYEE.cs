using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Factory;
using DenimERP.ViewModels.HR;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_HRD_EMPLOYEE : IBaseService<F_HRD_EMPLOYEE>
    {
        Task<IEnumerable<GetFHrEmployeeViewModel>> GetAllEmployeesAsync();
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
