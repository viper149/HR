using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
   public  interface IF_FS_FABRIC_RETURN_RECEIVE : IBaseService<F_FS_FABRIC_RETURN_RECEIVE>
    {
        Task<IEnumerable<F_FS_FABRIC_RETURN_RECEIVE>> GetFFsFabricReturnReceive();
        Task<FFsFabricReturnReceiveViewModel> GetInitObjByAsync(FFsFabricReturnReceiveViewModel ffsFabricReturnReceiveViewModel);

        Task<IEnumerable<COM_EX_PIMASTER>> GetStyleByPi(int pi);

    }
}
