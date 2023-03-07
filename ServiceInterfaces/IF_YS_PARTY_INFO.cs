using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public   interface IF_YS_PARTY_INFO : IBaseService<F_YS_PARTY_INFO>
    {
        Task<IEnumerable<F_YS_PARTY_INFO>> GetAllFYsPartyInfoAsync();
        Task<F_YS_PARTY_INFO> GetInitObjByAsync(F_YS_PARTY_INFO f_YS_PARTY_INFO);

    }
}
