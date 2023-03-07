using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_WEAVING_WORKLOAD_EFFICIENCELOSS : IBaseService<F_PR_WEAVING_WORKLOAD_EFFICIENCELOSS>
    {
        Task<IEnumerable<F_PR_WEAVING_WORKLOAD_EFFICIENCELOSS>> GetAllFPrWeavingWorkLoadEfficiencyLossAsync();
        Task<FPrWeavingWorkLoadEfficiencyLossViewModel> GetInitObjByAsync(FPrWeavingWorkLoadEfficiencyLossViewModel fPrWeavingWorkLoadEfficiencyLossViewModel);
    }
}
