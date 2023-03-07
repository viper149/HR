using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Factory.Production;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_WEAVING_PROCESS_BEAM_DETAILS_B : IBaseService<F_PR_WEAVING_PROCESS_BEAM_DETAILS_B>
    {
        Task<IEnumerable<F_LOOM_MACHINE_NO>> GetLoomMachines(int loomId);
        Task<PrWeavingProcessBulkViewModel> GetInitData(PrWeavingProcessBulkViewModel prWeavingProcessBulkViewModel);
        Task<int> InsertAndGetIdAsync(F_PR_WEAVING_PROCESS_BEAM_DETAILS_B fPrWeavingProcessBeamDetailsB);
    }
}
