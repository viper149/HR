using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_RECONE_MASTER : IBaseService<F_PR_RECONE_MASTER>
    {
        Task<List<F_PR_RECONE_MASTER>> GetFPrReconeMasterInfoAsync();
        Task<FPrReconeMasterViewModel> GetInitObjByAsync(FPrReconeMasterViewModel fPrReconeMasterViewModel);
    }
}
