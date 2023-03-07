using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_FS_WASTAGE_RECEIVE_M : IBaseService<F_FS_WASTAGE_RECEIVE_M>
    {
        Task<List<F_FS_WASTAGE_RECEIVE_M>> GetAllFFsWastageReceiveAsync();
        Task<FFsWastageReceiveViewModel> GetInitObjByAsync(FFsWastageReceiveViewModel fFsWastageReceiveViewModel);
        Task<FFsWastageReceiveViewModel> FindByIdIncludeAllAsync(int fwrId);
    }
}
