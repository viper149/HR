using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_FINISHING_FAB_PROCESS:IBaseService<F_PR_FINISHING_FAB_PROCESS>
    {
        Task<IEnumerable<F_PR_FINISHING_FAB_PROCESS>> GetInitFabricData(List<F_PR_FINISHING_FAB_PROCESS> fPrFinishingFabProcesses);
        Task<IEnumerable<F_PR_FINISHING_FAB_PROCESS>> GetFabricList(int finId);
    }
}
