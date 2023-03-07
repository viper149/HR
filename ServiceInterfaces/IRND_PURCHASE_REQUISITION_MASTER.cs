using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Rnd;

namespace DenimERP.ServiceInterfaces
{
    public interface IRND_PURCHASE_REQUISITION_MASTER : IBaseService<RND_PURCHASE_REQUISITION_MASTER>
    {
        Task<IEnumerable<RND_PURCHASE_REQUISITION_MASTER>> GetAllPurchaseRequisationAsync();
        Task<IEnumerable<RND_PURCHASE_REQUISITION_MASTER>> GetIndslidListWithStatusZero();
        Task<RND_PURCHASE_REQUISITION_MASTER> GetSinglePurchaseRequisitionByIdAsync(int id);
        Task<RndYarnRequisitionViewModel> GetPurchaseRequisitionById(int id);
        Task<RndYarnRequisitionViewModel> GetInitObjectsByAsync(RndYarnRequisitionViewModel rndYarnRequisitionViewModel);
        Task<RndYarnRequisitionViewModel> FindByIdIncludeAllAsync(int indslId);
        Task<object> GetCountNameByOrderNoAsync(RndYarnRequisitionViewModel rndYarnRequisition);
        Task<string> GetLastIndentNoAsync(string yarnFor);
    }
}