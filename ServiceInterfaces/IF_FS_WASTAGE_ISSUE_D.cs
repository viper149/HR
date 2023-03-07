using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_FS_WASTAGE_ISSUE_D : IBaseService<F_FS_WASTAGE_ISSUE_D>
    {
        Task<F_BAS_UNITS> GetAllByfwpIdAsync(int fwpId);
        Task<FFsWastageIssueViewModel> GetInitObjForDetailsByAsync(FFsWastageIssueViewModel fFsWastageIssueViewModel);
    }
}
