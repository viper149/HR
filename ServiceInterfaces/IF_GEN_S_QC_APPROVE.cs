using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_GEN_S_QC_APPROVE : IBaseService<F_GEN_S_QC_APPROVE>
    {
        Task<int> GetLastQCANo();
        Task<F_GEN_S_QC_APPROVE> GetQcDetails();
    }
}
