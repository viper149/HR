using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_HR_EMP_EDUCATION : IBaseService<F_HR_EMP_EDUCATION>
    {
        Task<F_HR_EMP_EDUCATION> FindByEmpIdAsync(int empId);
        Task<IEnumerable<F_HR_EMP_EDUCATION>> FindAllEduListByIdAsync(int empId);
    }
}
