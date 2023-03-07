using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Factory.Dyeing;
using DenimERP.ViewModels.Rnd;
using DenimERP.ViewModels.Rnd.Weaving;

namespace DenimERP.ServiceInterfaces
{
    public interface IRND_SAMPLE_INFO_WEAVING : IBaseService<RND_SAMPLE_INFO_WEAVING>
    {
        Task<RndSampleInfoWeavingViewModel> GetInitObjects(RndSampleInfoWeavingViewModel createRndSampleInfoWeavingView);
        Task<RndSampleInfoWeavingViewModel> GetInitObjectsWithDetails(RndSampleInfoWeavingViewModel createRndSampleInfoWeavingViewModel);

        Task<DataTableObject<RND_SAMPLE_INFO_WEAVING>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection,
            string searchValue, string draw, int skip, int pageSize);
        Task<RND_SAMPLE_INFO_DYEING> FindBySdIdAsync(int sdId);
        Task<CreateRndSampleInfoDyeingAndDetailsViewModel> FindBySdIdWithSetAsync(int sdId);
        Task<RndSampleInfoWeavingWithDetailsViewModel> FindByWvIdAsync(int wvId);
        Task<IEnumerable<RND_SAMPLE_INFO_WEAVING_DETAILS>> GetRndSampleInfoWeavingDetailsesBySdIdAsync(int sdId);
        Task<int> GetParentWvIdInsertByAsync(RND_SAMPLE_INFO_WEAVING rndSampleInfoWeaving);
        Task<PL_PRODUCTION_SETDISTRIBUTION> FindSetWithSetIdAsync(int setId);
    }
}
