using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_INSPECTION_WASTAGE_TRANSFER : IBaseService<F_PR_INSPECTION_WASTAGE_TRANSFER>
    {
        Task<IEnumerable<F_PR_INSPECTION_WASTAGE_TRANSFER>> GetAllInspectionWastageTransferAsync();
    }
}
