using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.SampleFabric;

namespace DenimERP.ServiceInterfaces.SampleGarments.Fabric
{
    public interface IH_SAMPLE_FABRIC_RECEIVING_M : IBaseService<H_SAMPLE_FABRIC_RECEIVING_M>
    {
        Task<CreateHSampleFabricReceivingM> GetInitObjsByAsync(CreateHSampleFabricReceivingM createHSampleFabricReceivingM);
        Task<DataTableObject<H_SAMPLE_FABRIC_RECEIVING_M>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize);
        Task<CreateHSampleFabricReceivingM> GetHSampleFabricReceiveDetailsByAsync(CreateHSampleFabricReceivingM createHSampleFabricReceivingM);
        Task<CreateHSampleFabricReceivingM> GetInitObjsForDetailsTableByAsync(CreateHSampleFabricReceivingM createHSampleFabricReceivingM);
        Task<CreateHSampleFabricReceivingM> FindByIdIncludeAllAsync(int rcvId);
    }
}
