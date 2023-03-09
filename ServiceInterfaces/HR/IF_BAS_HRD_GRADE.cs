using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Models;
using HRMS.ServiceInterfaces.BaseInterfaces;

namespace HRMS.ServiceInterfaces.HR
{
    public interface IF_BAS_HRD_GRADE : IBaseService<F_BAS_HRD_GRADE>
    {
        Task<IEnumerable<F_BAS_HRD_GRADE>> GetAllFBasHrdGradeAsync();
        Task<bool> FindByGradeNameAsync(string gradeName);
        Task<List<F_BAS_HRD_GRADE>> GetAllGradesAsync();
    }
}
