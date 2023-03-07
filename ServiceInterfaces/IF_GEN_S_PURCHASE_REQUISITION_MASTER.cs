using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_GEN_S_PURCHASE_REQUISITION_MASTER : IBaseService<F_GEN_S_PURCHASE_REQUISITION_MASTER>
    {
        Task<FGenSRequisitionViewModel> FindByIdIncludeAllAsync(int indslId, bool edit = false);
        Task<FGenSRequisitionViewModel> GetInitObjByAsync(FGenSRequisitionViewModel fGenSRequisitionViewModel);
        Task<IEnumerable<F_GEN_S_PURCHASE_REQUISITION_MASTER>> GetAllGenSPurchaseRequisitionAsync();
        Task<F_GEN_S_PURCHASE_REQUISITION_MASTER> GetFGenSPurReqMasterById(int id);
    }
}
