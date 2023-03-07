using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Com.Export;
using DenimERP.ViewModels.Home;

namespace DenimERP.ServiceInterfaces
{
    public interface ICOM_EX_PIMASTER : IBaseService<COM_EX_PIMASTER>
    {
        Task<IEnumerable<COM_EX_PIMASTER>> GetForDataTableByAsync();
        Task<bool> FindByPINoInUseAsync(string piNo);
        Task<IEnumerable<COM_EX_FABSTYLE>> FindFabStyleByPiIdAsync(int piId);
        Task<COM_EX_PIMASTER> FindByPiIdAsync(int piId);
        Task<COM_EX_PIMASTER> FindByIdPIInfoAsync(int? piId);
        Task<int> InsertAndGetIdByAsync(COM_EX_PIMASTER comExPiMaster);
        Task<int> TotalPercentageOfComExPiMaster(int days = 7);
        Task<ComExPIMasterViewModel> GetInitObjects(ComExPIMasterViewModel comExPiMasterViewModel);
        Task<ComExPIMasterViewModel> GetCostRefNo(int styleId);
        Task<string> GetLastPINoAsync();
        Task<ComExPIMasterViewModel> GetInitObjForDetailsTable(ComExPIMasterViewModel comExPiMasterViewModel);
        Task<ComExPIMasterViewModel> FindByPiIdIncludeAllAsync(int piId);
        Task<CreateComExLcInfoViewModel> GetPiInformationByAsync(CreateComExLcInfoViewModel comExLcInfoViewModel, string search, int page = 1);
        Task<IEnumerable<COM_EX_PIMASTER>> GetPiByBuyerAsync(int buyerId);
        Task<DashboardViewModel> GetPIChartData();
    }
}
