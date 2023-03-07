using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_FINISHING_FNPROCESS:IBaseService<F_PR_FINISHING_FNPROCESS>
    {
        Task<IEnumerable<F_PR_FINISHING_FNPROCESS>> GetInitFinishData(List<F_PR_FINISHING_FNPROCESS> fPrFinishingFnProcesses);
        Task<IEnumerable<F_PR_FINISHING_FNPROCESS>> GetFinishList(int finId);

        Task<List<FinishingChartDataViewModel>> GetFinishingDateWiseLengthGraph();
        Task<FinishingChartDataViewModel> GetFinishingDataDayMonthAsync();
        Task<FinishingChartDataViewModel> GetFinishingProductionData();
    }
}
