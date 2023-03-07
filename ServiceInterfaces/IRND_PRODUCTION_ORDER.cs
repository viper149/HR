using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Rnd;

namespace DenimERP.ServiceInterfaces
{
    public interface IRND_PRODUCTION_ORDER: IBaseService<RND_PRODUCTION_ORDER>
    {
        Task<RndProductionOrderViewModel> GetInitObjects(RndProductionOrderViewModel rndProductionOrderViewModel);
        Task<int> InsertAndGetIdAsync(RND_PRODUCTION_ORDER rndProductionOrder);
        RndProductionOrderDetailViewModel GetPoDetailsAsync(string orderNo);
        double GetPoDetailsForCountBudgetAsync(string orderNo, string countId, string warpLength);
        RndProductionOrderDetailViewModel GetPoDetailsByPoIdAsync(string orderNo);
        RndProductionOrderDetailViewModel GetRndPoDetailsByPoIdAsync(string orderNo);
        RndRsProductionOrderDetailsViewModel GetRsDetailsAsync(string orderNo);
        Task<IEnumerable<RND_PRODUCTION_ORDER>> GetAllAsync();
        Task<IEnumerable<TypeTableViewModel>> GetOrderNoDataAsync(string orderType);
        Task<RND_PRODUCTION_ORDER> GetSoNoByPoIdAsync(string orderNo);
    }
}
