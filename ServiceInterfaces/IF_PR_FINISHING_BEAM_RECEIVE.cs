using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_FINISHING_BEAM_RECEIVE:IBaseService<F_PR_FINISHING_BEAM_RECEIVE>
    {
        Task<FPrFinishingBeamReceiveViewModel> GetInitObjects(FPrFinishingBeamReceiveViewModel fPrFinishingBeamReceiveViewModel);
        Task<IEnumerable<F_PR_FINISHING_BEAM_RECEIVE>> GetAllAsync();
    }
}
