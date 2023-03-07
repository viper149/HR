using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces.HR
{
    public interface IF_HRD_EMP_EDU_DEGREE : IBaseService<F_HRD_EMP_EDU_DEGREE>
    {
        Task<IEnumerable<F_HRD_EMP_EDU_DEGREE>> GetAllFHrdEmpEduDegreeAsync();
        Task<bool> FindByDegreeAsync(string degree);
        Task<List<F_HRD_EMP_EDU_DEGREE>> GetAllEduDegreesAsync();
    }
}
