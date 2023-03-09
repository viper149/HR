using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Models;
using HRMS.ServiceInterfaces.BaseInterfaces;
using HRMS.ViewModels.HR;

namespace HRMS.ServiceInterfaces.HR
{
    public interface IF_BAS_HRD_DESIGNATION : IBaseService<F_BAS_HRD_DESIGNATION>
    {
        Task<IEnumerable<F_BAS_HRD_DESIGNATION>> GetAllFBasHrdDesignationAsync();
        Task<bool> FindByDesNameAsync(string desName, bool isBn = false);
        Task<FBasHrdDesignationViewModel> GetInitObjByAsync(FBasHrdDesignationViewModel fBasHrdDesignationViewModel);
        Task<List<F_BAS_HRD_DESIGNATION>> GetAllDesignationsAsync();
    }
}