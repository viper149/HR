using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Factory.Production;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_WEAVING_BEAM_RECEIVING:IBaseService<F_PR_WEAVING_BEAM_RECEIVING>
    {
        Task<IEnumerable<F_PR_WEAVING_BEAM_RECEIVING>> GetAllAsync();
        Task<PrWeavingProcessViewModel> GetInitObjects(PrWeavingProcessViewModel prWeavingProcessViewModel);
    }
}
