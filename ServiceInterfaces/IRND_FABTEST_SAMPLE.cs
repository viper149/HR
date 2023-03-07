using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IRND_FABTEST_SAMPLE: IBaseService<RND_FABTEST_SAMPLE>
    {
        Task<RndFabTestSampleViewModel> GetInitObjects(RndFabTestSampleViewModel rndFabTestSampleViewModel);
        Task<bool> FindByLabNo(string labNo);
        Task<IEnumerable<RND_FABTEST_SAMPLE>> GetAllRndFabTestSampleAsync();
        //Task<dynamic> FindObjectsByWvIdAsync(int sfinId);
        //Task<DataTableObject<RND_FABTEST_SAMPLE>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize);
        //Task<EditRndFabTestSampleViewModel> FindObjectsByLtsIdAsync(int ltsId);
        //Task<RND_FABTEST_SAMPLE> FindObjectsByLtsIdIncludeAllAsync(int ltsId);
    }
}
