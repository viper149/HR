using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_WEAVING_OS : IBaseService<F_PR_WEAVING_OS>
    {

        Task<IEnumerable<F_PR_WEAVING_OS>> GetAllFPrWeavingOs();
        Task<FPrWeavingOsViewModel> GetInitObjByAsync(FPrWeavingOsViewModel fPrWeavingOsViewModel);
    }
}
