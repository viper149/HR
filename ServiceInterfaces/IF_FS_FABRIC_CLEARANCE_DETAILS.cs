using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_FS_FABRIC_CLEARANCE_DETAILS:IBaseService<F_FS_FABRIC_CLEARANCE_DETAILS>
    {
        Task<FFsFabricClearanceViewModel> GetRollDetailsAsync(FFsFabricClearanceViewModel fFsFabricClearanceViewModel);
        Task<FFsFabricClearanceViewModel> SetRollStatus(FFsFabricClearanceViewModel fFsFabricClearanceViewModel);
        Task<FFsFabricClearanceViewModel> GetRollDetailsEditAsync(FFsFabricClearanceViewModel fFsFabricClearanceViewModel);
        Task<FFsFabricClearanceViewModel> GetDetailsAsync(FFsFabricClearanceViewModel fFsFabricClearanceViewModel);
    }
}
