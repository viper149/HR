using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Planning;

namespace DenimERP.ServiceInterfaces
{
    public interface IPL_PRODUCTION_PLAN_DETAILS: IBaseService<PL_PRODUCTION_PLAN_DETAILS>
    {
        Task<PlProductionGroupViewModel> GetInitData(PlProductionGroupViewModel plProductionGroupViewModel);
        Task<IEnumerable<PL_PRODUCTION_SO_DETAILS>> GetInitSoData(List<PL_PRODUCTION_SO_DETAILS> plProductionSoDetails);
        Task<int> InsertAndGetIdAsync(PL_PRODUCTION_PLAN_DETAILS plProductionPlanDetails);
    }
}
