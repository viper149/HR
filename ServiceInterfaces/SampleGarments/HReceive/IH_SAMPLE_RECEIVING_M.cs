using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.SampleGarments.HReceive;

namespace DenimERP.ServiceInterfaces.SampleGarments.HReceive
{
    public interface IH_SAMPLE_RECEIVING_M : IBaseService<H_SAMPLE_RECEIVING_M>
    {
        Task<CreateHSampleReceivingMViewModel> GetInitObjects(CreateHSampleReceivingMViewModel createHSampleReceivingMViewModel);
        Task<CreateHSampleReceivingMViewModel> GetFactoryGatePassReceiveDetails(int dpId);
        Task<DataTableObject<H_SAMPLE_RECEIVING_M>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize);
        Task<CreateHSampleReceivingMViewModel> FindByHsrIdIncludeAllAsync(int hsrId);
    }
}
