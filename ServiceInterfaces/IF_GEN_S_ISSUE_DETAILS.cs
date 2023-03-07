using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_GEN_S_ISSUE_DETAILS : IBaseService<F_GEN_S_ISSUE_DETAILS>
    {
        Task<FGenSIssueViewModel> GetInitObjForDetails(FGenSIssueViewModel fGenSIssueViewModel);
    }
}
