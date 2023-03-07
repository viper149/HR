using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_CHEM_STORE_RECEIVE_MASTER : IBaseService<F_CHEM_STORE_RECEIVE_MASTER>
    {
        Task<IEnumerable<F_CHEM_STORE_RECEIVE_MASTER>> GetAllChemicalReceiveAsync();
        Task<FChemStoreReceiveViewModel> GetInitObjsByAsync(FChemStoreReceiveViewModel fChemStoreReceiveViewModel);
        Task<FChemStoreReceiveViewModel> FindByIdIncludeAllAsync(int chemRcvId);
        Task<DataTableObject<F_CHEM_STORE_RECEIVE_MASTER>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize);
    }
}
