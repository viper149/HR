using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_YS_YARN_RECEIVE_MASTER_S : IBaseService<F_YS_YARN_RECEIVE_MASTER_S>
    {
        Task<IEnumerable<F_YS_YARN_RECEIVE_MASTER_S>> GetAllYarnReceiveAsync();
        Task<FYsYarnReceiveSViewModel> GetInitObjectsByAsync(FYsYarnReceiveSViewModel fYsYarnReceiveSViewModel);
        Task<FYsYarnReceiveSViewModel> FindByIdIncludeAllAsync(int yrcvId);
    }
}
