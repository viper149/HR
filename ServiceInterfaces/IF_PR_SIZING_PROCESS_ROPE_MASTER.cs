using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Factory.Production;
using DenimERP.ViewModels.Rnd;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_SIZING_PROCESS_ROPE_MASTER:IBaseService<F_PR_SIZING_PROCESS_ROPE_MASTER>
    {
        Task<IEnumerable<F_PR_SIZING_PROCESS_ROPE_MASTER>> GetAllAsync();

        Task<FSizingProductionRopeViewModel> GetInitObjects(FSizingProductionRopeViewModel fSizingProductionRopeViewModel);
        Task<IEnumerable<F_PR_SIZING_PROCESS_ROPE_CHEM>> GetInitChemData(List<F_PR_SIZING_PROCESS_ROPE_CHEM> fPrSizingProcessRopeChems);
        Task<IEnumerable<F_PR_SIZING_PROCESS_ROPE_DETAILS>> GetInitBeamData(List<F_PR_SIZING_PROCESS_ROPE_DETAILS> fPrSizingProcessRopeDetails);
        Task<int> InsertAndGetIdAsync(F_PR_SIZING_PROCESS_ROPE_MASTER fPrSizingProcessRopeMaster);
        Task<FSizingProductionRopeViewModel> FindAllByIdAsync(int sId);
        Task<RndProductionOrderDetailViewModel> GetSetDetails(int setId);
        Task<List<FSizingProductionRopeViewModel>> GetSizingDateWiseLengthGraph();
        Task<FSizingProductionRopeViewModel> GetSizingDataDayMonthAsync();

        Task<FSizingProductionRopeViewModel> GetSizingProductionData();
        Task<List<FSizingProductionRopeViewModel>> GetSizingProductionList();
        Task<List<PL_PRODUCTION_SETDISTRIBUTION>> GetSizingPendingSetList();
       
    }
}
