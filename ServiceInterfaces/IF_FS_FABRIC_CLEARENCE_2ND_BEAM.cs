using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Rnd;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_FS_FABRIC_CLEARENCE_2ND_BEAM : IBaseService<F_FS_FABRIC_CLEARENCE_2ND_BEAM>
    {
        Task<List<F_FS_FABRIC_CLEARENCE_2ND_BEAM>> GetAllAsync();
        Task<FFsFabricClearance2ndBeamViewModel> GetInitData(FFsFabricClearance2ndBeamViewModel fFsFabricClearance2NdBeamViewModel);
        Task<RndProductionOrderDetailViewModel> GetSetDetails(int setId);
    }
}
