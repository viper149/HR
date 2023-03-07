using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_CHEM_REQ_DETAILS : IBaseService<F_CHEM_REQ_DETAILS>
    {
        Task<dynamic> GetSingleChemReqDetails(int csrId, int productId);
        Task<IEnumerable<F_CHEM_STORE_RECEIVE_DETAILS>> GetSingleChemReqDetailsAsync(int csrId, int productId);
    }
}
