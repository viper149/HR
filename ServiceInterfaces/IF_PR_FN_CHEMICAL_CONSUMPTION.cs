using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_FN_CHEMICAL_CONSUMPTION:IBaseService<F_PR_FN_CHEMICAL_CONSUMPTION>
    {
        Task<IEnumerable<F_PR_FN_CHEMICAL_CONSUMPTION>> GetInitChemData(List<F_PR_FN_CHEMICAL_CONSUMPTION> fPrFnChemicalConsumptions);
        //Task<IEnumerable<F_PR_FN_CHEMICAL_CONSUMPTION>> GetChemList(int? setId);

    }
}
