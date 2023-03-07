using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_GEN_S_MRR : IBaseService<F_GEN_S_MRR>
    {
        Task<int> GetLastMrrNo();
        Task<F_GEN_S_MRR> GetMrrDetails();
    }
}
