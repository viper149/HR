using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces.ProcWorkOrder
{
   public interface IPROC_WORKORDER_MASTER : IBaseService<PROC_WORKORDER_MASTER>
    {
        Task<IEnumerable<PROC_WORKORDER_MASTER>> GetAllProcWorkOrder();
        Task<ProcWorkOrderViewModel> GetInitObjByAsync(ProcWorkOrderViewModel procWorkOrderViewModel);
        Task<ProcWorkOrderViewModel> GetIndentProductInfoByAsync(ProcWorkOrderViewModel procWorkOrderViewModel);
    }
}
