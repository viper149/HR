using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IRND_FABTEST_BULK:IBaseService<RND_FABTEST_BULK>
    {
        Task<IEnumerable<RND_FABTEST_BULK>> GetAllRndFabTestBulkAsync();
        Task<RndFabTestBulkViewModel> GetInitObjByAsync(RndFabTestBulkViewModel rndFabTestBulkViewModel);
        Task<bool> FindByLabNo(string labNo);
        Task<PL_PRODUCTION_SETDISTRIBUTION> GetSetDetailsAsync(int setId);
        Task<F_PR_FINISHING_FNPROCESS> GetFnProcessDetailsAsync(int id);
    }
}
