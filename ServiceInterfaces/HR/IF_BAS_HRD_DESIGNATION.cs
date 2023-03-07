using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.HR;

namespace DenimERP.ServiceInterfaces.HR
{
    public interface IF_BAS_HRD_DESIGNATION : IBaseService<F_BAS_HRD_DESIGNATION>
    {
        Task<IEnumerable<F_BAS_HRD_DESIGNATION>> GetAllFBasHrdDesignationAsync();
        Task<bool> FindByDesNameAsync(string desName, bool isBn = false);
        Task<FBasHrdDesignationViewModel> GetInitObjByAsync(FBasHrdDesignationViewModel fBasHrdDesignationViewModel);
        Task<List<F_BAS_HRD_DESIGNATION>> GetAllDesignationsAsync();
    }
}