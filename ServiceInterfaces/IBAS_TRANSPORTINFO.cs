using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Basic;

namespace DenimERP.ServiceInterfaces
{
    public interface IBAS_TRANSPORTINFO : IBaseService<BAS_TRANSPORTINFO>
    {
        Task<IEnumerable<BAS_TRANSPORTINFO>> GetAllForDataTables();
        Task<BasTransportInfoViewModel> FindByIdIncludeAllAsync(int trnspId);
    }
}
