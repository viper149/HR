using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_HR_EMP_SALARYSETUP_Repository : BaseRepository<F_HR_EMP_SALARYSETUP>, IF_HR_EMP_SALARYSETUP
    {
        public SQLF_HR_EMP_SALARYSETUP_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<F_HR_EMP_SALARYSETUP> FindByEmpIdAsync(int empId)
        {

            try
            {
                return await DenimDbContext.F_HR_EMP_SALARYSETUP.AsNoTracking().Where(e => e.EMPID.Equals(empId)).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        }
    }
}
