using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_GEN_S_RECEIVE_DETAILS : IBaseService<F_GEN_S_RECEIVE_DETAILS>
    {
        IEnumerable<F_GEN_S_RECEIVE_DETAILS> FindAllGenSByReceiveIdAsync(int id);
        Task<FGenSReceiveViewModel> GetInitObjForDetails(FGenSReceiveViewModel fGenSReceiveViewModel);
    }
}
