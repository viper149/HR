using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_FS_FABRIC_LOADING_BILL : IBaseService<F_FS_FABRIC_LOADING_BILL>
    {
        Task<IEnumerable<F_FS_FABRIC_LOADING_BILL>> GetAllFFsFabricLoadingBillAsync();
        Task<FFsFabricLoadingBillViewModel> GetInitObjByAsync(FFsFabricLoadingBillViewModel fFsFabricLoadingBillViewModel);
      
    }
}
