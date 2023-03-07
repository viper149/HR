using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface ICOS_PRECOSTING_MASTER: IBaseService<COS_PRECOSTING_MASTER>
    {
        Task<int> InsertByAndReturnIdAsync(COS_PRECOSTING_MASTER cosPreCostingMaster);
        Task<COS_PRECOSTING_MASTER> FindByIdAllAsync(int csId);
        Task<CosPreCostingMasterViewModel> GetInitObjects(CosPreCostingMasterViewModel cosPreCostingMasterViewModel);
        Task<IEnumerable<COS_PRECOSTING_MASTER>> GetAllAsync();
    }
}
