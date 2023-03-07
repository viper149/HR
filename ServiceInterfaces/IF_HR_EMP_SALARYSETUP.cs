using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_HR_EMP_SALARYSETUP : IBaseService<F_HR_EMP_SALARYSETUP>
    {
        Task<F_HR_EMP_SALARYSETUP> FindByEmpIdAsync(int empId);
    }
}
