using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public  interface IF_GS_WASTAGE_ISSUE_D : IBaseService<F_GS_WASTAGE_ISSUE_D>
    {
        Task<F_BAS_UNITS> GetAllBywpIdAsync(int wpId);
        Task<FGsWastageIssueViewModel> GetInitObjForDetailsByAsync(FGsWastageIssueViewModel fGsWastageIssueViewModel);
    }
}
