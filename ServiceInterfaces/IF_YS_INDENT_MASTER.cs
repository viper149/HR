using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Rnd;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_YS_INDENT_MASTER : IBaseService<F_YS_INDENT_MASTER>
    {
        Task<IEnumerable<F_YS_INDENT_MASTER>> GetAllIndentMasterAsync();
        Task<int> GetLastIndNo();
        Task<F_YS_INDENT_MASTER> GetIndentByINDSLID(int indslid);
        Task<int> InsertAndGetIdAsync(F_YS_INDENT_MASTER fYsIndentMaster);
        Task<RndYarnRequisitionViewModel> GetInitObjects(RndYarnRequisitionViewModel requisitionViewModel);
        Task<RndYarnRequisitionViewModel> GetInitObjectsForYarnDetails(RndYarnRequisitionViewModel requisitionViewModel);
        Task<RndYarnRequisitionViewModel> GetIndentCountListByINDSLID(RndYarnRequisitionViewModel requisitionViewModel);
        Task<RndYarnRequisitionViewModel> GetOtherDetails(string id);
        Task<RndYarnRequisitionViewModel> FindByIdIncludeAllAsync(int indId);
    }
}
