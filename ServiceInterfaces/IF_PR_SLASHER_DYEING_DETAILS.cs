using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_SLASHER_DYEING_DETAILS:IBaseService<F_PR_SLASHER_DYEING_DETAILS>
    {
        Task<IEnumerable<F_PR_SLASHER_DYEING_DETAILS>> GetInitBeamData(List<F_PR_SLASHER_DYEING_DETAILS> fPrSlasherDyeingDetailses);
    }
}
