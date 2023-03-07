using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_DYEING_PROCESS_ROPE_DETAILS: IBaseService<F_DYEING_PROCESS_ROPE_DETAILS>
    {
        Task<IEnumerable<F_DYEING_PROCESS_ROPE_DETAILS>> GetInitBallData(
            List<F_DYEING_PROCESS_ROPE_DETAILS> fDyeingProcessRopeDetailsList);

        Task<DyeingChartDataViewModel> GetDyeingPendingSets();
        Task<DyeingChartDataViewModel> GetMonthlyDyeingPendingAndCompleteSets();
        Task<DyeingChartDataViewModel> GetDyeingChemicalConsumed();
        Task<List<DyeingChartDataViewModel>>GetDyeingProductionList();
        Task<DyeingChartDataViewModel> GetDyeingProductionData();
        Task<List<PL_PRODUCTION_SETDISTRIBUTION>> GetDyeingPendingSetList();
    }
}
