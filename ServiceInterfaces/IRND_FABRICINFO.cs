using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Rnd;
using DenimERP.ViewModels.Rnd.Fabric;

namespace DenimERP.ServiceInterfaces
{
    public interface IRND_FABRICINFO : IBaseService<RND_FABRICINFO>
    {
        Task<bool> FindByRndFabricInfoFabCodeByAsync(int fabCode);
        //Task<DataTableObject<RND_FABRICINFO>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize);
        Task<List<RND_FABRICINFO>> GetForDataTableByAsync();
        Task<RND_FABRICINFO> FindByFabCodeIAsync(int fabCode, bool create = false, bool update = false, bool delete = false, bool details = false);
        Task<IEnumerable<RndFabricCountinfoAndRndYarnConsumptionViewModel>> GetAllRndFabricAndYarnConsumptionList(int fabCode);
        Task<int> TotalNumberOfFabricInfoExcludingAllByAsync();
        Task<RndFabricInfoViewModel> GetInitObjects(RndFabricInfoViewModel rndFabricInfoViewModel);
        Task<ExtendRndSampleInfoWeavingViewModel> GetAssociateObjectsByWvIdAsync(int wvId);
        Task<RND_SAMPLEINFO_FINISHING> GetAssociateObjectsBySFinId(int sFinId);
        Task<RND_FABTEST_SAMPLE> GetLabTestResult(int ltsId);
        //Task<DyeingWeavingDetailsListViewModel> GetDyeingWeavingDetailsByWvIdAsync(int wvId);
        Task<RndFabricInfoViewModel> GetDyeingWeavingDetailsByWvIdAsync(RndFabricInfoViewModel rndFabricInfoViewModel);
        Task<int> GetWeavingIdBySFinId(int sFinId);
        Task<RndFabricInfoViewModel> GetEditInfo(RndFabricInfoViewModel rndFabricInfoViewModel);
        Task<int?> GetPrimaryKeyDuringInsertByAsync(RND_FABRICINFO rndFabricinfo);
        Task<RndFabricInfoCountAndYarnConsumptionViewModel> GetSelectedTargetSegmentAsync(RndFabricInfoCountAndYarnConsumptionViewModel rndFabricInfoCountAndYarnConsumptionViewModel);
        
        Task<RND_FABRICINFO> GetFabInfoWithCount(int id);
    }
}
