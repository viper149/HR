using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Data;
using HRMS.Models;
using HRMS.ServiceInfrastructures.BaseInfrastructures;
using HRMS.ServiceInterfaces.HR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.ServiceInfrastructures.HR
{
    public class SQLF_HRD_EDUCATION_Repository : BaseRepository<F_HRD_EDUCATION>, IF_HRD_EDUCATION
    {
        public SQLF_HRD_EDUCATION_Repository(HRDbContext hrDbContext) : base(hrDbContext)
        {
        }

        public async Task<List<F_HRD_EDUCATION>> GetAllEducationByEmpIdAsync(int id)
        {
            return await HrDbContext.F_HRD_EDUCATION
                .Where(d => d.EMPID.Equals(id))
                .ToListAsync();
        }
    }
}
