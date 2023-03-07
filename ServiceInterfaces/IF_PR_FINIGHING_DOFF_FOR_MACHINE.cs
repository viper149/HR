using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_FINIGHING_DOFF_FOR_MACHINE:IBaseService<F_PR_FINIGHING_DOFF_FOR_MACHINE>
    {
        Task<IEnumerable<F_PR_FINIGHING_DOFF_FOR_MACHINE>> GetInitDoffData(List<F_PR_FINIGHING_DOFF_FOR_MACHINE> fPrFinighingDoffForMachines, int processType);
    }
}
