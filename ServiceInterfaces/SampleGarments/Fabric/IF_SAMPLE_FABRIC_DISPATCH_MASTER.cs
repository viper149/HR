using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.SampleGarments.Fabric;

namespace DenimERP.ServiceInterfaces.SampleGarments.Fabric
{
    public interface IF_SAMPLE_FABRIC_DISPATCH_MASTER : IBaseService<F_SAMPLE_FABRIC_DISPATCH_MASTER>
    {
        Task<int> GetGetPassNo();
        Task<CreateFSampleFabricDispatchMasterViewModel> GetInitObjectsByAsync(CreateFSampleFabricDispatchMasterViewModel createFSampleFabricDispatchMasterViewModel);
        Task<CreateFSampleFabricDispatchMasterViewModel> GetInitObjectsForDetailsTableByAsync(CreateFSampleFabricDispatchMasterViewModel createFSampleFabricDispatchMasterViewModel);
        Task<CreateFSampleFabricDispatchMasterViewModel> FindByIdIncludeAllAsync(int dpId);
        Task<dynamic> GetQtyByAsync(CreateFSampleFabricDispatchMasterViewModel createFSampleFabricDispatchMasterView);
        Task<DataTableObject<F_SAMPLE_FABRIC_DISPATCH_MASTER>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize);
        Task<CreateFSampleFabricDispatchMasterViewModel> GetGatePassFor(string search, int page = 1);
        Task<CreateFSampleFabricDispatchMasterViewModel> GetGatePassType(string search, int page = 1);
        Task<CreateFSampleFabricDispatchMasterViewModel> GetDriverInfo(string search, int page = 1);
        Task<CreateFSampleFabricDispatchMasterViewModel> GetVehicleInfo(string search, int page = 1);
        Task<F_SAMPLE_FABRIC_DISPATCH_MASTER> FindByIdForDeleteAsync(int dpId);
    }
}
