using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces.HR
{
    public interface IF_HRD_EDUCATION : IBaseService<F_HRD_EDUCATION>
    {
        Task<List<F_HRD_EDUCATION>> GetAllEducationByEmpIdAsync(int id);
    }
}
