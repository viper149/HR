using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.SampleGarments.Fabric;

namespace DenimERP.ServiceInterfaces.SampleGarments.Fabric
{
    public interface IF_SAMPLE_FABRIC_RCV_M : IBaseService<F_SAMPLE_FABRIC_RCV_M>
    {
        Task<CreateFSampleFabricRcvMViewModel> GetInitObjsByAsync(CreateFSampleFabricRcvMViewModel createFSampleFabricRcvMViewModel);
        Task<CreateFSampleFabricRcvMViewModel> GetInitObjectsByAsync(CreateFSampleFabricRcvMViewModel createFSampleFabricRcvMViewModel);
        Task<CreateFSampleFabricRcvMViewModel> GetDetailsFormInspectionByAsync(CreateFSampleFabricRcvMViewModel createFSampleFabricRcvMViewModel);
        Task<DataTableObject<F_SAMPLE_FABRIC_RCV_M>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize);
        Task<CreateFSampleFabricRcvMViewModel> FindBySfrIdIncludeAllAsync(int sfrId);
        Task<CreateFSampleFabricRcvMViewModel> FindBySfrIdForDeleteAsync(int sfrId);
        Task<CreateFSampleFabricRcvMViewModel> GetProgramsByAsync(string search, int page = 1);
        Task<CreateFSampleFabricRcvMViewModel> GetDetailsFormClearanceByAsync(CreateFSampleFabricRcvMViewModel createFSampleFabricRcvMViewModel);
    }
}
