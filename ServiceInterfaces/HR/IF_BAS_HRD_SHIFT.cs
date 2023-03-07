using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces.HR
{
    public interface IF_BAS_HRD_SHIFT : IBaseService<F_BAS_HRD_SHIFT>
    {
        Task<bool> FindByShiftAsync(string shift);
        Task<IEnumerable<F_BAS_HRD_SHIFT>> GetAllFBasHrdShiftAsync();
    }
}
