using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_YS_YARN_ISSUE_DETAILS_S : IBaseService<F_YS_YARN_ISSUE_DETAILS_S>
    {
        Task<int> InsertAndGetIdAsync(F_YS_YARN_ISSUE_DETAILS_S fYsYarnIssueDetailsS);
        Task<FYsYarnIssueSViewModel> GetInitObjectsByAsync(FYsYarnIssueSViewModel fYsYarnIssueSViewModel);
        Task<int> GetReqId(int COUNTID, double REQ_QTY);
    }
}
