using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Home;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_CHEM_ISSUE_DETAILS : IBaseService<F_CHEM_ISSUE_DETAILS>
    {
        Task<int> InsertAndGetIdAsync(F_CHEM_ISSUE_DETAILS fChemIssueDetails);
        Task<DashboardViewModel> GetIssuedChemicalData();
    }
}
