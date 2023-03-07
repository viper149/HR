using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface ICOM_IMP_WORK_ORDER_MASTER : IBaseService<COM_IMP_WORK_ORDER_MASTER>
    {
        Task<IEnumerable<COM_IMP_WORK_ORDER_MASTER>> GetAllComImpWorkOrderAsync();
        Task<ComImpWorkOrderViewModel> GetInitObjByAsync(ComImpWorkOrderViewModel comImpWorkOrderViewModel);
        Task<int> GetLastWOID();
        Task<ComImpWorkOrderViewModel> GetInitDetailsObjByAsync(ComImpWorkOrderViewModel comImpWorkOrderViewModel);
        Task<ComImpWorkOrderViewModel> FindByIdIncludeAllAsync(int parse);
        Task<string> GetLastContNoAsync();
        Task<int> GetIndslIdByInd(int indid);
        Task<COM_IMP_WORK_ORDER_MASTER> FindPreviousWorkOrder(int? indid);
    }
}
