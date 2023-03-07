using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_YARN_REQ_MASTER_S : IBaseService<F_YARN_REQ_MASTER_S>
    {
        Task<IEnumerable<F_YARN_REQ_MASTER_S>> GetAllYarnRequirementAsync();
        Task<IEnumerable<F_YARN_REQ_MASTER_S>> GetYsrIdList();
        Task<FYarnReqSViewModel> GetInitObjects(FYarnReqSViewModel fYarnReqSViewModel);
        Task<FYarnReqSViewModel> FindByIdIncludeAllAsync(int ysrId);
        Task<dynamic> GetRequiredKgsWithLotdAsync(int poId, int countId);
    }
}
