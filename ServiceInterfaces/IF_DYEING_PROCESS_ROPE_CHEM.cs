using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_DYEING_PROCESS_ROPE_CHEM:IBaseService<F_DYEING_PROCESS_ROPE_CHEM>
    {
        Task<IEnumerable<F_DYEING_PROCESS_ROPE_CHEM>> GetInitChemData(List<F_DYEING_PROCESS_ROPE_CHEM> fDyeingProcessRopeChems);
    }
}
