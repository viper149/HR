using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
   public  interface IF_FS_WASTAGE_PARTY : IBaseService<F_FS_WASTAGE_PARTY>
    {
        Task<IEnumerable<F_FS_WASTAGE_PARTY>> GetAllFFsWastagePartyAsync();
        Task<F_FS_WASTAGE_PARTY> GetInitObjByAsync(F_FS_WASTAGE_PARTY f_FS_WASTAGE_PARTY);

        Task<bool> FindByProductName(string pName);


    }
}
