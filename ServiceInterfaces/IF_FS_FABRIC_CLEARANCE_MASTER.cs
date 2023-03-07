using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_FS_FABRIC_CLEARANCE_MASTER:IBaseService<F_FS_FABRIC_CLEARANCE_MASTER>
    {
        Task<IEnumerable<F_FS_FABRIC_CLEARANCE_MASTER>> GetAllAsync();
        Task<FFsFabricClearanceViewModel> GetInitObjects(FFsFabricClearanceViewModel fFsFabricClearanceViewModel);
        Task<bool> WashcodeAnyAsync(FFsFabricClearanceViewModel fFsFabricClearanceViewModel);
        Task<FFsFabricClearanceViewModel> FindAllByIdAsync(int clId);
        Task<dynamic> GetOrderDetaiils(int id);
    }
}
