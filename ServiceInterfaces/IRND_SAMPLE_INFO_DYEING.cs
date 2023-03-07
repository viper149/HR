using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Factory.Dyeing;
using DenimERP.ViewModels.Rnd.Dyeing;

namespace DenimERP.ServiceInterfaces
{
    public interface IRND_SAMPLE_INFO_DYEING : IBaseService<RND_SAMPLE_INFO_DYEING>
    {
        Task<CreateRndSampleInfoDyeingAndDetailsViewModel> GetInitObjects(CreateRndSampleInfoDyeingAndDetailsViewModel createRndSampleInfoDyeingAndDetailsViewModel);
        Task<CreateRndSampleInfoDyeingAndDetailsViewModel> GetInitObjectsWithDetails(CreateRndSampleInfoDyeingAndDetailsViewModel createRndSampleInfoDyeingAndDetailsViewModel);
        Task<DataTableObject<RND_SAMPLE_INFO_DYEING>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize);
        Task<DetailsRndSampleInfoDyeingViewModel> FindByIdIncludeAllAsync(int id);
        Task<RND_SAMPLE_INFO_DYEING> GetTeamInfo(int sdrfId);
        string GetLastRSNo(string rsCode);
        Task<RND_SAMPLE_INFO_DYEING> FindByIdIncludeAssociatesAsync(int id);
    }
}
