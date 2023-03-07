using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.SampleGarments.HDispatch;

namespace DenimERP.ServiceInterfaces.SampleGarments.HDispatch
{
    public interface IH_SAMPLE_DESPATCH_M : IBaseService<H_SAMPLE_DESPATCH_M>
    {
        Task<CreateHSampleDespatchMViewModel> GetInitObjects(CreateHSampleDespatchMViewModel createHSampleDespatchMViewModel);
        Task<int> GetGatePassNumber();
        Task<bool> IsAddableToHSampleDispatchAsync(CreateHSampleDespatchMViewModel createHSampleDespatchMViewModel);
        Task<DataTableObject<H_SAMPLE_DESPATCH_M>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize);
        Task<CreateHSampleDespatchMViewModel> FindByHsdIdIncludeAllAsync(int hsdId);
    }
}
