using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface ICOM_IMP_WORK_ORDER_DETAILS : IBaseService<COM_IMP_WORK_ORDER_DETAILS>
    {
        Task<IEnumerable<F_YS_INDENT_DETAILS>> GetCountInfoByIndentIdAsync(int indId);
        Task<F_YS_INDENT_DETAILS> GetAllByCountIdAsync(int transId);
    }
}
