using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Factory.Production;
using DenimERP.ViewModels.Rnd;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_LCB_PRODUCTION_ROPE_MASTER:IBaseService<F_LCB_PRODUCTION_ROPE_MASTER>
    {
        Task<IEnumerable<F_LCB_PRODUCTION_ROPE_MASTER>> GetAllAsync();
        Task<FLcbProductionRopeViewModel> GetInitObjects(FLcbProductionRopeViewModel fLcbProductionRopeViewModel);
        Task<int> InsertAndGetIdAsync(F_LCB_PRODUCTION_ROPE_MASTER fLcbProductionRopeMaster);
        Task<RndProductionOrderDetailViewModel> GetSetDetails(int setId);
        Task<dynamic> GetSubGroupDetails(int subGroupId);
        Task<FLcbProductionRopeViewModel> FindAllByIdAsync(int lcbId);
        Task<LCBChartDataViewModel> GetLCBDateWiseLengthGraph();

        Task<LCBChartDataViewModel> GetLCBProductionData();
        Task<List<LCBChartDataViewModel>> GetLCBProductionList();
        Task<LCBChartDataViewModel> GetMonthlyLCBPendingAndCompleteSets();
        Task<List<PL_PRODUCTION_SETDISTRIBUTION>> GeLcbPendingSetList();
    }
}
