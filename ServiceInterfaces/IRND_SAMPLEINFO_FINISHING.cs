using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Rnd.Finish;

namespace DenimERP.ServiceInterfaces
{
    public interface IRND_SAMPLEINFO_FINISHING : IBaseService<RND_SAMPLEINFO_FINISHING>
    {
        Task<CreateRndSampleInfoFinishViewModel> GetInitObjects(CreateRndSampleInfoFinishViewModel createRndSampleInfoFinishViewModel);
        Task<CreateRndSampleInfoFinishViewModel> GetPreviousData(int ltgId);
        Task<DataTableObject<RND_SAMPLEINFO_FINISHING>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize);
        Task<CreateRndSampleInfoFinishViewModel> FindByFnIdAsync(int fnId);
        Task<RND_SAMPLEINFO_FINISHING> FindByLtgIdAsync(int fnId);
        Task<RND_SAMPLEINFO_FINISHING> GetProgNoBySfnIdAsync(int sfnId);
    }
}
