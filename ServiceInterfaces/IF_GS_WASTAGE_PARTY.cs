using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_GS_WASTAGE_PARTY : IBaseService<F_GS_WASTAGE_PARTY>
    {
        Task<IEnumerable<F_GS_WASTAGE_PARTY>> GetAllFGsWastagePartyAsync();
        Task<F_GS_WASTAGE_PARTY> GetInitObjByAsync(F_GS_WASTAGE_PARTY fGsWastageParty);
        Task<bool> FindByProductName(string pName);
    }
}
