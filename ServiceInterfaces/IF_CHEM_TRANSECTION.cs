using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_CHEM_TRANSECTION : IBaseService<F_CHEM_TRANSECTION>
    {
        Task<double> GetLastBalanceByProductIdAsync(int? id,int cRcvId);
        Task<IEnumerable<F_CHEM_TRANSECTION>> GetAllTransactions();
    }
}
