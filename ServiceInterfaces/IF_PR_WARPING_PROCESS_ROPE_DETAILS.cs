using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Factory.Production;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_WARPING_PROCESS_ROPE_DETAILS:IBaseService<F_PR_WARPING_PROCESS_ROPE_DETAILS>
    {
        Task<PrWarpingProcessRopeViewModel> GetInitSoData(PrWarpingProcessRopeViewModel prWarpingProcessRopeViewModel);
        Task<int> InsertAndGetIdAsync(F_PR_WARPING_PROCESS_ROPE_DETAILS prWarpingProcessRopeDetails);

        Task<PL_PRODUCTION_PLAN_DETAILS> GetSetList(int subGroupId);
        Task<IEnumerable<F_PR_WARPING_PROCESS_ROPE_DETAILS>> GetWarpSetList(int warpId);
    }
}
