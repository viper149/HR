using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_YS_GP_MASTER : IBaseService<F_YS_GP_MASTER>
    {
        Task<IEnumerable<F_YS_GP_MASTER>> GetAllAsync();

        Task<FYsGpViewModel> GetInitObjByAsync(FYsGpViewModel fysgpViewModel);

        Task<FYsGpViewModel> GetInitDetailsObjByAsync(FYsGpViewModel fysgpViewModel);

        Task<FYsGpViewModel> FindByIdIncludeAllAsync(int id);
        Task<F_YS_GP_MASTER> FindByIdForDeleteAsync(int dsId);

        Task<COM_IMP_LCINFORMATION> GetLcInfoAsync(int id);

        Task<IEnumerable<F_YS_YARN_RECEIVE_DETAILS>> GetYarnIndentDetails(int countId);

        Task<IEnumerable<F_YS_YARN_RECEIVE_DETAILS>> GetLotId(int lotid);

    }
}
