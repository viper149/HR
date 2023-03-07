using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface ICOS_PRECOSTING_DETAILS:IBaseService<COS_PRECOSTING_DETAILS>
    {
        Task<IEnumerable<COS_PRECOSTING_DETAILS>> FindPreCostDetailsListByFabCodeAndCountIdAsync(int fabCode, int countId);
        Task<IEnumerable<COS_PRECOSTING_DETAILS>> GetAllDetailsAsync(int csId);
        Task<CosPreCostingMasterViewModel> GetCountList(CosPreCostingMasterViewModel cosPreCostingMasterViewModel);
    }
}
