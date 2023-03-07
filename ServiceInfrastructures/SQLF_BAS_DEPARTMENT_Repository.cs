using System;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_BAS_DEPARTMENT_Repository : BaseRepository<F_BAS_DEPARTMENT>, IF_BAS_DEPARTMENT
    {
        public SQLF_BAS_DEPARTMENT_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {

        }

        public async Task<F_BAS_DEPARTMENT> GetSectionByDepartmentIdAsync(int id)
        {
            try
            {
                var fBasDepartment = await DenimDbContext.F_BAS_DEPARTMENT
                    .Include(c => c.F_BAS_SECTION)
                    .FirstOrDefaultAsync(c => c.DEPTID.Equals(id));
                return fBasDepartment;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public async Task<bool> FindByDepartmentNameAsync(string deptName)
        {
            return await DenimDbContext.F_BAS_DEPARTMENT.AnyAsync(e => e.DEPTNAME.Equals(deptName));
        }
    }
}
