using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_GEN_S_ISSUE_MASTER :IBaseService<F_GEN_S_ISSUE_MASTER>
    {
        Task<FGenSIssueViewModel> FindByIdIncludeAllAsync(int gsIssueId, bool edit = false);
        Task<FGenSIssueViewModel> GetInitObjByAsync(FGenSIssueViewModel fGenSIssueViewModel);
        Task<IEnumerable<F_GEN_S_ISSUE_MASTER>> GetAllFGenSIssueMasterList();
        Task<FGenSIssueViewModel> GetInitObjForDetailsByAsync(FGenSIssueViewModel fGenSIssueViewModel);
    }
}
