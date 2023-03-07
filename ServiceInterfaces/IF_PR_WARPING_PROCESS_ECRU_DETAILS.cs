using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_WARPING_PROCESS_ECRU_DETAILS : IBaseService<F_PR_WARPING_PROCESS_ECRU_DETAILS>
    {
        Task<FPrWarpingProcessEkruMasterViewModel> GetInitSoData(FPrWarpingProcessEkruMasterViewModel fPrWarpingProcessEkruMasterViewModel);
    }
}
