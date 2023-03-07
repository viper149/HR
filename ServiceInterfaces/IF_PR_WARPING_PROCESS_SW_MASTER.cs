using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_WARPING_PROCESS_SW_MASTER : IBaseService<F_PR_WARPING_PROCESS_SW_MASTER>
    {
        Task<List<F_PR_WARPING_PROCESS_SW_MASTER>> GetAllAsync();
        Task<FPrWarpingProcessSwMasterViewModel> GetInitObjects(FPrWarpingProcessSwMasterViewModel prWarpingProcessSwMasterViewModel);
        Task<string> GetWarpLength(int? setId);
        Task<List<WarpingChartDataViewModel>> GetSectionalWarpingProductionList();
    }
}
