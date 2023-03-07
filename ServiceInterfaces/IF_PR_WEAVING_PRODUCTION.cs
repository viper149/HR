using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Home;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_WEAVING_PRODUCTION : IBaseService<F_PR_WEAVING_PRODUCTION>
    {
        Task<IEnumerable<F_PR_WEAVING_PRODUCTION>> GetAllFPrWeavingProductionAsync();

        Task<FPrWeavingProductionViewModel> GetInitObjByAsync(FPrWeavingProductionViewModel fPrWeavingProductionViewModel);
        Task<RND_PRODUCTION_ORDER> GetStyleInfoBySo(int id);
        Task<FPrWeavingProductionViewModel> GetProductionDetailsAsync(FPrWeavingProductionViewModel fPrWeavingProductionViewModel);
        Task<DashboardViewModel> GetWeavingDateWiseLengthGraph();
        Task<FPrWeavingProductionViewModel> GetWeavingProductionData();
        Task<List<FPrWeavingProductionViewModel>> GetWeavingProductionList();
        Task<List<PL_PRODUCTION_SETDISTRIBUTION>> GetWeavingPendingList();
    }
}
