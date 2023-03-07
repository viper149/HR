using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_YS_YARN_ISSUE_MASTER_S : IBaseService<F_YS_YARN_ISSUE_MASTER_S>
    {
        Task<dynamic> GetYarnReqMaster(int ysrId);
        Task<IEnumerable<F_YS_YARN_ISSUE_MASTER_S>> GetAllIssueMasterList();
        Task<FYsYarnIssueSViewModel> FindByIdIncludeAllAsync(int yIssueId);
        Task<FYsYarnIssueSViewModel> GetInitObjectsAsync(FYsYarnIssueSViewModel fYsYarnIssueSViewModel);
    }
}
