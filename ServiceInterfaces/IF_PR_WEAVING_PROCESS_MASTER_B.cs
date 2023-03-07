using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Factory.Production;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_WEAVING_PROCESS_MASTER_B:IBaseService<F_PR_WEAVING_PROCESS_MASTER_B>
    {
        Task<IEnumerable<F_PR_WEAVING_PROCESS_MASTER_B>> GetAllAsync();
        Task<PrWeavingProcessBulkViewModel> GetInitObjects(PrWeavingProcessBulkViewModel prWeavingProcessBulkViewModel);
        Task<int> InsertAndGetIdAsync(F_PR_WEAVING_PROCESS_MASTER_B fPrWeavingProcessMasterB);

        Task<PrWeavingProcessBulkViewModel> FindAllByIdAsync(int wvId);
        Task<dynamic> GetSetDetails(int setId);

        Task<PrWeavingProcessBulkViewModel> GetConsumpDetails(PrWeavingProcessBulkViewModel prWeavingProcessBulkViewModel);
    }
}
