using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_BAS_DEPARTMENT : IBaseService<F_BAS_DEPARTMENT>
    {
        Task<F_BAS_DEPARTMENT> GetSectionByDepartmentIdAsync(int id);
        Task<bool> FindByDepartmentNameAsync(string deptName);
    }
}
