using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_FS_CLEARANCE_WASTAGE_TRANSFER : IBaseService<F_FS_CLEARANCE_WASTAGE_TRANSFER>
    {
        Task<FFsClearanceWastageTransferViewModel> GetInitObjByAsync(FFsClearanceWastageTransferViewModel fFsClearanceWastageTransferViewModel);
        Task<IEnumerable<F_FS_CLEARANCE_WASTAGE_TRANSFER>> GetAllFFsClearanceWastageTransferAsync();
    }
}
