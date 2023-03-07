using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
   public interface IF_PR_RECONE_YARN_DETAILS : IBaseService<F_PR_RECONE_YARN_DETAILS>
    {
        Task<FPrReconeMasterViewModel> GetInitObjForDetailsByAsync(FPrReconeMasterViewModel fPrReconeMasterViewModel);
    }
}
