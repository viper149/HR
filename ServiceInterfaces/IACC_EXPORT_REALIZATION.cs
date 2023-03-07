using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Home;

namespace DenimERP.ServiceInterfaces
{
    public interface IACC_EXPORT_REALIZATION : IBaseService<ACC_EXPORT_REALIZATION>
    {
        Task<IEnumerable<ACC_EXPORT_REALIZATION>> GetAccExRealizationAllAsync();
        Task<AccExRealizationViewModel> FindByIdIncludeAllAsync(int trnsId);
        Task<DashboardViewModel> GetRealizatioData();
    }
}