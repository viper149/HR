using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.SampleGarments.GatePass;
using DenimERP.ViewModels.SampleGarments.Receive;

namespace DenimERP.ServiceInterfaces.SampleGarments.GatePass
{
    public interface IF_SAMPLE_DESPATCH_MASTER : IBaseService<F_SAMPLE_DESPATCH_MASTER>
    {
        Task<CreateFSampleDesPatchMasterViewModel> GetInitObjects(CreateFSampleDesPatchMasterViewModel createFSampleDesPatchMasterViewModel);
        Task<CreateFSampleDesPatchMasterViewModel> GetInitObjectsOfSelectedItems(CreateFSampleDesPatchMasterViewModel createFSampleDesPatchMasterViewModel);
        Task<DataTableObject<ExtendFSampleDespatchMasterViewModel>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize);
        Task<CreateFSampleDesPatchMasterViewModel> FindByDispatchIdIncludeAllAsync(int dispatchId);
        Task<int> GetGatePassNumber();
        Task<ContainsFsampleGarmentRcvd> GetNumberOfTotalItems(int trnsId);
        Task<F_SAMPLE_GARMENT_RCV_D> FindByBarcodeAsync(string barcode);
    }
}
