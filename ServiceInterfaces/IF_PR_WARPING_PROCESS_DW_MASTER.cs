using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_WARPING_PROCESS_DW_MASTER:IBaseService<F_PR_WARPING_PROCESS_DW_MASTER>
    {
        Task<IEnumerable<F_PR_WARPING_PROCESS_DW_MASTER>> GetAllAsync();
        Task<PrWarpingProcessSlasherViewModel> GetInitObjects(PrWarpingProcessSlasherViewModel prWarpingProcessSlasherViewModel);
        Task<string> GetWarpLength(int? setId);
        Task<PrWarpingProcessSlasherViewModel> FindAllByIdAsync(int id);
        Task<WarpingChartDataViewModel> GetDirectWarpingProductionData();
        Task<List<WarpingChartDataViewModel>>GetDirectWarpingProductionList();
    }
}
