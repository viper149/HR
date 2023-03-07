using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.HR;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.HR
{
    public class SQLF_HRD_EDUCATION_Repository : BaseRepository<F_HRD_EDUCATION>, IF_HRD_EDUCATION
    {
        public SQLF_HRD_EDUCATION_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<List<F_HRD_EDUCATION>> GetAllEducationByEmpIdAsync(int id)
        {
            return await DenimDbContext.F_HRD_EDUCATION
                .Where(d => d.EMPID.Equals(id))
                .ToListAsync();
        }
    }
}
