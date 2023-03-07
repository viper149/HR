using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_GS_GATEPASS_RETURN_RCV_MASTER : IBaseService<F_GS_GATEPASS_RETURN_RCV_MASTER>
    {
        Task<IEnumerable<F_GS_GATEPASS_RETURN_RCV_MASTER>> GetAllFGenSRequirementAsync();
        Task<FGsGatepassReturnRcvViewModel> GetInitObjByAsync(FGsGatepassReturnRcvViewModel fGsGatepassReturnRcvViewModel);
        Task<FGsGatepassReturnRcvViewModel> GetInitDetailsObjByAsync(FGsGatepassReturnRcvViewModel fGsGatepassReturnRcvViewModel);
        Task<FGsGatepassReturnRcvViewModel> FindByIdIncludeAllAsync(int rcvId);
    }
}
