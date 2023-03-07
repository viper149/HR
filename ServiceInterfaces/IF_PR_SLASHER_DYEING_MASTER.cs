using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_SLASHER_DYEING_MASTER: IBaseService<F_PR_SLASHER_DYEING_MASTER>
    {
        Task<IEnumerable<F_PR_SLASHER_DYEING_MASTER>> GetAllAsync();
        Task<FDyeingProcessSlasherViewModel> GetInitObjects(FDyeingProcessSlasherViewModel fDyeingProcessSlasherViewModel);
        Task<dynamic> GetSetDetails(int setId);
        Task<string> GetSetWarpLength(int setId);
        Task<FDyeingProcessSlasherViewModel> FindAllByIdAsync(int sId);
    }
}
