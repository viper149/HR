using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_HR_EMP_FAMILYDETAILS : IBaseService<F_HR_EMP_FAMILYDETAILS>
    {
        Task<F_HR_EMP_FAMILYDETAILS> FindByEmpIdAsync(int empId);
        Task<IEnumerable<F_HR_EMP_FAMILYDETAILS>> FindAllByEmpIdAsync(int empId);
    }
}
