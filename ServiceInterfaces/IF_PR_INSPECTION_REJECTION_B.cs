using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_INSPECTION_REJECTION_B : IBaseService<F_PR_INSPECTION_REJECTION_B>
    {
        Task<IEnumerable<F_PR_INSPECTION_REJECTION_B>> GetAllFPrInspectionRejectionBAsync();
        Task<FPrInspectionRejectionBViewModel> GetInitObjByAsync(FPrInspectionRejectionBViewModel fPrInspectionRejectionBViewModel);
        Task<F_PR_WEAVING_PROCESS_DETAILS_B> GetAllBywdIdAsync(int wdId);
        Task<FPrInspectionRejectionBViewModel> GetDoffByInspectionDate(FPrInspectionRejectionBViewModel fPrInspectionRejectionBViewModel);
    }
}
