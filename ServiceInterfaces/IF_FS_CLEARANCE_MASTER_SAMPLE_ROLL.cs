using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_FS_CLEARANCE_MASTER_SAMPLE_ROLL : IBaseService<F_FS_CLEARANCE_MASTER_SAMPLE_ROLL>
    {
        Task<FFsClearanceMasterSampleRollViewModel> GetInitObjByAsync(FFsClearanceMasterSampleRollViewModel fFsClearanceMasterSampleRollViewModel);
        Task<IEnumerable<F_FS_CLEARANCE_MASTER_SAMPLE_ROLL>> GetAllClearanceMasterSampleRollAsync();
        Task<F_PR_INSPECTION_PROCESS_DETAILS> GetRollDetailsAsync(int rollId);
        Task<PL_PRODUCTION_SETDISTRIBUTION> GetSetDetailsAsync(int setId);
    }
}
