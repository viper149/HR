using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_INSPECTION_CUTPCS_TRANSFER : IBaseService<F_PR_INSPECTION_CUTPCS_TRANSFER>
    {

        //Task<IEnumerable<IF_PR_INSPECTION_CUTPCS_TRANSFER>> GetAllInspectionCutPcsTransferAsync();

        Task<IEnumerable<F_PR_INSPECTION_CUTPCS_TRANSFER>> GetAllFprInspectionCutPcsTransferAsync();
        Task<FprInspectionCutPcsTransferViewModel> GetInitObjByAsync(FprInspectionCutPcsTransferViewModel fprInspectionCutPcsTransferViewModel);
        Task<FprInspectionCutPcsTransferViewModel> FindByIdIncludeAllAsync(int cpid);
    }
}
