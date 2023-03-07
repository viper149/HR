using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_WARPING_PROCESS_SW_DETAILS : IBaseService<F_PR_WARPING_PROCESS_SW_DETAILS>
    {
        Task<FPrWarpingProcessSwMasterViewModel> GetInitSoData(FPrWarpingProcessSwMasterViewModel prWarpingProcessSwMasterViewModel);
    }
}
