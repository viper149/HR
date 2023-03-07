using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_YS_YARN_ISSUE_MASTER : IBaseService<F_YS_YARN_ISSUE_MASTER>
    {
        Task<dynamic> GetYarnReqMaster(int ysrId);
        Task<IEnumerable<F_YS_YARN_ISSUE_MASTER>> GetAllIssueMasterList();
        Task<FYsYarnIssueViewModel> FindByIdIncludeAllAsync(int yIssueId);
        Task<FYsYarnIssueViewModel> GetInitObjectsAsync(FYsYarnIssueViewModel yarnIssueViewModel);
    }
}
