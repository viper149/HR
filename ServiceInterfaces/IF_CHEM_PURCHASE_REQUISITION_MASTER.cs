using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_CHEM_PURCHASE_REQUISITION_MASTER : IBaseService<F_CHEM_PURCHASE_REQUISITION_MASTER>
    {
        Task<IEnumerable<F_CHEM_PURCHASE_REQUISITION_MASTER>> GetAllChemicalPurchaseRequisitionAsync();
        Task<F_CHEM_PURCHASE_REQUISITION_MASTER> GetChemPurReqMasterById(int id);
        Task<FChemicalRequisitionViewModel> GetInitObjByAsync(FChemicalRequisitionViewModel requisitionViewModel);
        Task<FChemicalRequisitionViewModel> FindByIdIncludeAllAsync(int indslId, bool edit = false);
    }
}
