using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_GEN_S_REQ_DETAILS : IBaseService<F_GEN_S_REQ_DETAILS>
    {
        Task<F_GEN_S_REQ_DETAILS> GetSingleGenSReqDetails(int gsrId, int productId);
    }
}
