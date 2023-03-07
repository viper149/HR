using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Factory.Fabric_Store;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_FS_FABRIC_RCV_DETAILS:IBaseService<F_FS_FABRIC_RCV_DETAILS>
    {
        Task<FFsRollReceiveViewModel> GetRollsAsync(DateTime rcvDate);
        Task<IEnumerable<F_FS_FABRIC_RCV_DETAILS>> GetRollListAsync();
        Task<F_FS_FABRIC_RCV_DETAILS> GetRollIDetails(int rollId);
        Task<bool> GetRollBalance(int rollId,double fullLength);
        Task<FFsRollReceiveViewModel> GetRollsByScanAsync(FFsRollReceiveViewModel fFsRollReceiveViewModel);
        Task<F_FS_FABRIC_RCV_DETAILS> FindRollDetails(int rollId, DateTime rcvDate);
        Task<F_FS_FABRIC_RCV_DETAILS> GetRollIdByRollNo(string rollNo);
        Task<FFsRollReceiveViewModel> GetRollDetailsList(FFsRollReceiveViewModel fFsRollReceiveViewModel);

        Task<F_FS_FABRIC_RCV_DETAILS> GetRcvRollIdByRollNo(string rollNo);
    }
}
