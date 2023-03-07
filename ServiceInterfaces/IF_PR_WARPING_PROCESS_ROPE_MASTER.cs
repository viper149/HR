using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Factory.Production;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_WARPING_PROCESS_ROPE_MASTER: IBaseService<F_PR_WARPING_PROCESS_ROPE_MASTER>
    {
        Task<PrWarpingProcessRopeViewModel> GetInitObjects(PrWarpingProcessRopeViewModel prWarpingProcessRopeViewModel);
        Task<int> InsertAndGetIdAsync(F_PR_WARPING_PROCESS_ROPE_MASTER prWarpingProcessRopeMaster);
        Task<IEnumerable<F_PR_WARPING_PROCESS_ROPE_MASTER>> GetAllWithNameAsync();
        Task<IEnumerable<F_PR_WARPING_PROCESS_ROPE_MASTER>> GetAllPendingWithNameAsync();
        Task<PrWarpingProcessRopeDataViewModel> GetDataBySubGroupIdAsync(string subGroup);
        Task<PrWarpingProcessRopeDataViewModel> GetDataBySetIdAsync(string setId);
        Task<PrWarpingProcessRopeViewModel> FindAllByIdAsync(int id);
        Task<List<WarpingChartDataViewModel>> GetWarpingDateWiseLengthGraph();
        Task<WarpingChartDataViewModel> GetWarpingDataDayMonthAsync();
        Task<WarpingChartDataViewModel> GetWarpingPendingSets();
        Task<List<PL_PRODUCTION_SETDISTRIBUTION>> GetWarpingPendingSetList();
        Task<WarpingChartDataViewModel> GetBudgetConsumedYarn();
        Task<WarpingChartDataViewModel> GetRopeWarpingProductionData();
        Task<List<WarpingChartDataViewModel>> GetRopeWarpingProductionList();
        Task<List<WarpingChartDataViewModel>> GetWarpingProductionList();
        Task<WarpingChartDataViewModel> MonthlyWarpingPendingsAndCompleteSets();
    }
}
