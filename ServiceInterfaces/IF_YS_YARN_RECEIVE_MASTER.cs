using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_YS_YARN_RECEIVE_MASTER : IBaseService<F_YS_YARN_RECEIVE_MASTER>
    {
        Task<IEnumerable<F_YS_YARN_RECEIVE_MASTER>> GetAllYarnReceiveAsync();
        Task<YarnReceiveViewModel> GetInitObjectsByAsync(YarnReceiveViewModel yarnReceiveViewModel);
        Task<YarnReceiveViewModel> FindByIdIncludeAllAsync(int yrcvId);
    }
}
