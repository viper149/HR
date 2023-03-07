using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Home;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_YS_YARN_ISSUE_DETAILS : IBaseService<F_YS_YARN_ISSUE_DETAILS>
    {
        Task<int> InsertAndGetIdAsync(F_YS_YARN_ISSUE_DETAILS fYsYarnIssueDetails);
        Task<FYsYarnIssueViewModel> GetInitObjectsByAsync(FYsYarnIssueViewModel fYsYarnIssueViewModel);
        Task<int> GetReqId(int COUNTID, double REQ_QTY);
        Task<DashboardViewModel> GetIssuedCountData();
    }
}
