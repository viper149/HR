using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_YARN_TRANSACTION_S : IBaseService<F_YARN_TRANSACTION_S>
    {
        Task<double?> GetLastBalanceByCountIdAsync(int? countId, int lotId);
    }
}
