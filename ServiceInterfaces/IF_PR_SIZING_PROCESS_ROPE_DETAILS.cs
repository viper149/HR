using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Home;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_SIZING_PROCESS_ROPE_DETAILS:IBaseService<F_PR_SIZING_PROCESS_ROPE_DETAILS>
    {
        Task<DashboardViewModel> GetSizingDateWiseLengthGraph();
    }
}
