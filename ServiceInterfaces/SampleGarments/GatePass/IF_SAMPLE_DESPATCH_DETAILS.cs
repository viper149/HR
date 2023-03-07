using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces.SampleGarments.GatePass
{
    public interface IF_SAMPLE_DESPATCH_DETAILS : IBaseService<F_SAMPLE_DESPATCH_DETAILS>
    {
        Task<IEnumerable<F_SAMPLE_DESPATCH_DETAILS>> FindByDispatchIdAsync(int dispatchId);
    }
}
