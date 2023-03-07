using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_GS_GATEPASS_INFORMATION_D : IBaseService<F_GS_GATEPASS_INFORMATION_D>
    {
        Task<IEnumerable<F_GS_GATEPASS_INFORMATION_D>> GetGsGatePassInfoByGpId(int gpId);
    }
}
