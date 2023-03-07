using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_HR_EMP_OFFICIALINFO : IBaseService<F_HR_EMP_OFFICIALINFO>
    {
        Task<F_HR_EMP_OFFICIALINFO> FindByEmpIdAsync(int empId);
        Task<F_HR_EMP_OFFICIALINFO> GetSingleEmployeeDetails(int id);
    }
}
