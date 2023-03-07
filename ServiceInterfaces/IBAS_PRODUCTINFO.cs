using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Basic;

namespace DenimERP.ServiceInterfaces
{
    public interface IBAS_PRODUCTINFO : IBaseService<BAS_PRODUCTINFO>
    {
        Task<IEnumerable<BAS_PRODUCTINFO>> GetProductInfoList();
        Task<IEnumerable<BAS_PRODUCTINFO>> GetProductInfoListAllAsync();
        Task<int> TotalNumberOfProducts();
        Task<BAS_PRODUCTINFO> FindProductInfoByAsync(int id);
        Task<BasProductInfoViewModel> GetInfo(BasProductInfoViewModel basProductInfoViewModel);
    }
}
