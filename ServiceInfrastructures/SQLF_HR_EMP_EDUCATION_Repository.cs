using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_HR_EMP_EDUCATION_Repository : BaseRepository<F_HR_EMP_EDUCATION>, IF_HR_EMP_EDUCATION
    {
        public SQLF_HR_EMP_EDUCATION_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {

        }

        public async Task<IEnumerable<F_HR_EMP_EDUCATION>> FindAllEduListByIdAsync(int empId)
        {
            try
            {
                return await DenimDbContext.F_HR_EMP_EDUCATION.Where(e => e.EMPID.Equals(empId)).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<F_HR_EMP_EDUCATION> FindByEmpIdAsync(int empId)
        {
            try
            {
                return await DenimDbContext.F_HR_EMP_EDUCATION.Where(e => e.EMPID.Equals(empId)).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
