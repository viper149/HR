using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_GS_GATEPASS_INFORMATION_M : IBaseService<F_GS_GATEPASS_INFORMATION_M>
    {
        Task<int> InsertAndGetId(F_GS_GATEPASS_INFORMATION_M fGsGatepassInformationM);

        Task<IEnumerable<F_GS_GATEPASS_INFORMATION_M>> GetAllGsGatePassAsync();
        Task<FGsGatePassViewModel> GetInitObjByAsync(FGsGatePassViewModel fGsGatePassViewModel);
        Task<FGsGatePassViewModel> GetInitDetailsObjByAsync(FGsGatePassViewModel fGsGatePassViewModel);
        Task<FGsGatePassViewModel> FindByIdIncludeAllAsync(int gpId);
        Task<IEnumerable<FGsGatePassViewModel>> GetProductListByGpIdAsync(int gpId);
    }
}
