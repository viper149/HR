using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IRND_FABTEST_SAMPLE_BULK : IBaseService<RND_FABTEST_SAMPLE_BULK>
    {
        Task<IEnumerable<RND_FABTEST_SAMPLE_BULK>> GetAllRndFabTestSampleBulkAsync();
        Task<RndFabTestSampleBulkViewModel> GetInitObjByAsync(RndFabTestSampleBulkViewModel rndFabTestSampleBulkViewModel);
        Task<RND_SAMPLEINFO_FINISHING> FindObjectsByIdAsync(int sfinId);
        Task<bool> FindByLabNo(string labNo);
    }
}
