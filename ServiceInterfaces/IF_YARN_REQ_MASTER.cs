using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_YARN_REQ_MASTER : IBaseService<F_YARN_REQ_MASTER>
    {
        Task<IEnumerable<F_YARN_REQ_MASTER>> GetAllYarnRequirementAsync();
        Task<IEnumerable<F_YARN_REQ_MASTER>> GetYsrIdList();
        Task<YarnRequirementViewModel> GetInitObjects(YarnRequirementViewModel yarnRequirementViewModel);
        Task<YarnRequirementViewModel> FindByIdIncludeAllAsync(int ysrId);
        Task<dynamic> GetRequiredKgsWithLotdAsync(int poId, int countId);
    }
}
