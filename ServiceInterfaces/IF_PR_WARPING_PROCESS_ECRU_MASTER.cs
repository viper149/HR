using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_WARPING_PROCESS_ECRU_MASTER : IBaseService<F_PR_WARPING_PROCESS_ECRU_MASTER>
    {
        Task<List<F_PR_WARPING_PROCESS_ECRU_MASTER>> GetAllAsync();
        Task<FPrWarpingProcessEkruMasterViewModel> GetInitObjects(FPrWarpingProcessEkruMasterViewModel fPrWarpingProcessEkruMasterViewModel,bool e);
        Task<FPrWarpingProcessEkruMasterViewModel> FindAllByIdAsync(int id);
        Task<WarpingChartDataViewModel> GetEcruWarpingProductionData();
        Task<List<WarpingChartDataViewModel>> GetEcruWarpingProductionList();
    }
}
