using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.SampleFabric;

namespace DenimERP.ServiceInterfaces.SampleGarments.Fabric
{
    public interface IH_SAMPLE_FABRIC_DISPATCH_MASTER : IBaseService<H_SAMPLE_FABRIC_DISPATCH_MASTER>
    {
        Task<string> GetGatePassNoByAsync();
        Task<dynamic> GetBuyersByAsync(string search, int page);
        Task<dynamic> GetBrandsByAsync(string search, int page);
        Task<dynamic> GetMerchandisersByAsync(string search, int page);
        Task<dynamic> GetAvailableItemsByAsync(string search, int page);
        Task<dynamic> GetOtherInfoByAsync(CreateHSampleFabricDispatchMaster createHSampleFabricDispatchMaster);
        Task<DataTableObject<H_SAMPLE_FABRIC_DISPATCH_MASTER>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize);
        Task<H_SAMPLE_FABRIC_DISPATCH_MASTER> GetForSafeDeleteByAsync(int sfdId);
        Task<CreateHSampleFabricDispatchMaster> FindByIdIncludeAllAsync(int sfdId);
        Task<CreateHSampleFabricDispatchMaster> GetInitObjByAsync(CreateHSampleFabricDispatchMaster createHSampleFabricDispatchMaster);
        Task<CreateHSampleFabricDispatchMaster> GetInitObjForDetailsTableByAsync(CreateHSampleFabricDispatchMaster createHSampleFabricDispatchMaster);
    }
}
