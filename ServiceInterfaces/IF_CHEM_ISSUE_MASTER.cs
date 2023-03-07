using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_CHEM_ISSUE_MASTER : IBaseService<F_CHEM_ISSUE_MASTER>
    {
        Task<IEnumerable<F_CHEM_ISSUE_MASTER>> GetAllChemIssueMasterList();
        Task<FChemIssueViewModel> GetInitObjByAsync(FChemIssueViewModel fChemIssueViewModel);
        Task<FChemIssueViewModel> GetInitObjForDetailsByAsync(FChemIssueViewModel fChemIssueViewModel);
        Task<FChemIssueViewModel> FindByIdIncludeAllAsync(int cIssueId, bool edit = false);
    }
}
