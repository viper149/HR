using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_FS_UP_MASTER : IBaseService<F_FS_UP_MASTER>
    {
        Task<FFsFabricUPViewModel> GetInitObjByAsync(FFsFabricUPViewModel fFsFabricUPViewModel);
    }
}