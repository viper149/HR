using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS: IBaseService<F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS>
    {
        Task<F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS> FindByIdAndCountAsync(int rcvId, int? countId);
    }
}
