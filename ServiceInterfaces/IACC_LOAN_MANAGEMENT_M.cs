using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IACC_LOAN_MANAGEMENT_M : IBaseService<ACC_LOAN_MANAGEMENT_M>
    {
        Task<IEnumerable<ACC_LOAN_MANAGEMENT_M>> GetForDataTableByAsync();
    }
}
