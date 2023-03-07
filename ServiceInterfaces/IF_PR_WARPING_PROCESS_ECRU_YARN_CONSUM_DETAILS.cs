using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
   public interface IF_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS : IBaseService<F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS>
    {
        Task<IEnumerable<F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS>> GetInitCountData(List<F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS> fPrWarpingProcessEcruYarnConsumDetailsList);
    }
}
