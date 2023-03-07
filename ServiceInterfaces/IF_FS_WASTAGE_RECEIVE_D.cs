using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_FS_WASTAGE_RECEIVE_D:IBaseService<F_FS_WASTAGE_RECEIVE_D>
    {
        Task<F_BAS_UNITS> GetAllBywpIdAsync(int fwpId);
        Task<FFsWastageReceiveViewModel> GetInitObjForDetailsByAsync(FFsWastageReceiveViewModel fFsWastageReceiveViewModel);
    }
}
