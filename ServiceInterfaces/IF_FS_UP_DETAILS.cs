using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_FS_UP_DETAILS : IBaseService<F_FS_UP_DETAILS>
    {
        Task<COM_EX_LCINFO> GetLcInfo(int lcId);
        Task<FFsFabricUPViewModel> GetInitObjectsOfSelectedItems(FFsFabricUPViewModel fFsFabricUPViewModel);
    }
}
