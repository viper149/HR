using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_FS_WASTAGE_ISSUE_M : IBaseService<F_FS_WASTAGE_ISSUE_M>
    {
        Task<IEnumerable<F_FS_WASTAGE_ISSUE_M>> GetAllFFsWastageIssueAsync();
        Task<FFsWastageIssueViewModel> GetInitObjByAsync(FFsWastageIssueViewModel fFsWastageIssueViewModel);
        Task<FFsWastageIssueViewModel> FindByIdIncludeAllAsync(int fwiId);
    }
}
