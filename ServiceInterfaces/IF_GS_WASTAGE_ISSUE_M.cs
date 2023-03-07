using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_GS_WASTAGE_ISSUE_M : IBaseService<F_GS_WASTAGE_ISSUE_M>
    {
        Task<IEnumerable<F_GS_WASTAGE_ISSUE_M>> GetAllFGsWastageIssueAsync();
        Task<FGsWastageIssueViewModel> GetInitObjByAsync(FGsWastageIssueViewModel fGsWastageIssueViewModel);
        Task<FGsWastageIssueViewModel> FindByIdIncludeAllAsync(int wiId);
    }
}
