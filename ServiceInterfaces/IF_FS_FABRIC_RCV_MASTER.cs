using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Factory.Fabric_Store;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_FS_FABRIC_RCV_MASTER:IBaseService<F_FS_FABRIC_RCV_MASTER>
    {
        Task<IEnumerable<F_FS_FABRIC_RCV_MASTER>> GetAllAsync();
        Task<FFsRollReceiveViewModel> GetInitObjects(FFsRollReceiveViewModel fFsRollReceiveViewModel);
        Task<int> InsertAndGetIdAsync(F_FS_FABRIC_RCV_MASTER fFsFabricRcvMaster);
        Task<F_FS_FABRIC_RCV_MASTER> GetRollDetailsByDate(DateTime? rcvDate);
        Task<FFsRollReceiveViewModel> FindByRollRcvIdAsync(int rollRcvId);
    }
}
