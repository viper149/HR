using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_GEN_S_RECEIVE_MASTER : IBaseService<F_GEN_S_RECEIVE_MASTER>
    {
        Task<FGenSReceiveViewModel> GetInitObjsByAsync(FGenSReceiveViewModel fGenSReceiveViewModel);
        Task<FGenSReceiveViewModel> FindByIdIncludeAllAsync(int gsRcvId, bool edit = false);
        Task<IEnumerable<F_GEN_S_RECEIVE_MASTER>> GetAllFGenSReceiveAsync();
    }
}
