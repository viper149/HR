using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_CHEM_STORE_RECEIVE_DETAILS : IBaseService<F_CHEM_STORE_RECEIVE_DETAILS>
    {
        IEnumerable<F_CHEM_STORE_RECEIVE_DETAILS> FindAllChemByReceiveIdAsync(int id);
        Task<double?> GetRemainingBalanceByBatchId(int? productId,string batchNo);
        Task<FChemStoreReceiveViewModel> GetInitObjForDetails(FChemStoreReceiveViewModel fChemStoreReceiveViewModel);
    }
}
