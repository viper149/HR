using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Models;
using HRMS.ServiceInterfaces.BaseInterfaces;

namespace HRMS.ServiceInterfaces.HR
{
    public interface IF_HRD_EDUCATION : IBaseService<F_HRD_EDUCATION>
    {
        Task<List<F_HRD_EDUCATION>> GetAllEducationByEmpIdAsync(int id);
    }
}
