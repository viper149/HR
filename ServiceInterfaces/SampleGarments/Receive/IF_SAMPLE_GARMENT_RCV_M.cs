using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.SampleGarments.Receive;

namespace DenimERP.ServiceInterfaces.SampleGarments.Receive
{
    public interface IF_SAMPLE_GARMENT_RCV_M : IBaseService<F_SAMPLE_GARMENT_RCV_M>
    {
        Task<CreateFSampleGarmentRcvMViewModel> GetInitObjectsByAsync(CreateFSampleGarmentRcvMViewModel createFSampleGarmentRcvMViewModel);
        Task<CreateFSampleGarmentRcvMViewModel> GetInitObjectsOfSelectedItems(CreateFSampleGarmentRcvMViewModel createFSampleGarmentRcvMViewModel);
        Task<DataTableObject<F_SAMPLE_GARMENT_RCV_M>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize);
        Task<CreateFSampleGarmentRcvMViewModel> FindBySrIdIncludeAllAsync(int srId);
        Task<RND_FABRICINFO> FindByFabCodeAsync(int fabCode);
        Task<CreateFSampleGarmentRcvMViewModel> GetEmployeesByAsync(string search, int page = 1);
        Task<CreateFSampleGarmentRcvMViewModel> GetSampleItemsByAsync(string search, int page = 1);
        Task<CreateFSampleGarmentRcvMViewModel> GetRndFabricsByAsync(string search, int page = 1);
        Task<CreateFSampleGarmentRcvMViewModel> GetSectionsByAsync(string search, int page = 1);
        Task<CreateFSampleGarmentRcvMViewModel> GetDetailsFormInspectionByAsync(CreateFSampleGarmentRcvMViewModel createFSampleGarmentRcvMViewModel);
    }
}
