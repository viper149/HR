using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
   public interface IF_YS_YARN_RECEIVE_MASTER2 : IBaseService<F_YS_YARN_RECEIVE_MASTER2>
    {
        Task<IEnumerable<F_YS_YARN_RECEIVE_MASTER2>> GetAllYarnReceiveAsync();
        Task<YarnReceiveForOthersViewModel> GetInitObjectsByAsync(YarnReceiveForOthersViewModel yarnReceiveForOthersViewModel);

        Task<YarnReceiveForOthersViewModel> GetInitDetailsObjByAsync(YarnReceiveForOthersViewModel yarnReceiveForOthersViewModel);

        Task<YarnReceiveForOthersViewModel> FindByIdIncludeAllAsync(int id);

    }
}
