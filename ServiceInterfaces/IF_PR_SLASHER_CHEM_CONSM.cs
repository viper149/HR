using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_SLASHER_CHEM_CONSM : IBaseService<F_PR_SLASHER_CHEM_CONSM>
    {
        Task<IEnumerable<F_PR_SLASHER_CHEM_CONSM>> GetInitChemData(List<F_PR_SLASHER_CHEM_CONSM> fPrSlasherChemConsms);
    }
}
