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
    public class SQLF_HR_EMP_OFFICIALINFO_Repository : BaseRepository<F_HR_EMP_OFFICIALINFO>, IF_HR_EMP_OFFICIALINFO
    {
        public SQLF_HR_EMP_OFFICIALINFO_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<F_HR_EMP_OFFICIALINFO> FindByEmpIdAsync(int empId)
        {
            try
            {
                return await DenimDbContext.F_HR_EMP_OFFICIALINFO.AsNoTracking().Where(e => e.EMPID.Equals(empId)).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<F_HR_EMP_OFFICIALINFO> GetSingleEmployeeDetails(int id)
        {
            var result = await DenimDbContext.F_HR_EMP_OFFICIALINFO
                .Include(d => d.DEPT)
                .Include(d => d.SEC)
                //.Include(d => d.SSEC)
                .Where(d => d.EMPID.Equals(id))
                .Select(d => new F_HR_EMP_OFFICIALINFO
                {
                    EMPID = d.EMPID,
                    DEPTID = d.DEPTID,
                    SECID = d.SECID,
                    SSECID = d.SSECID,
                    DEPT = new F_BAS_DEPARTMENT
                    {
                        DEPTID = d.DEPT.DEPTID,
                        DEPTNAME = d.DEPT.DEPTNAME
                    },
                    SEC = new F_BAS_SECTION
                    {
                        SECID = d.SEC.SECID,
                        SECNAME = d.SEC.SECNAME
                    },
                    //SSEC = new F_BAS_SUBSECTION
                    //{
                    //    SSECID = d.SSEC.SSECID,
                    //    SSECNAME = d.SSEC.SSECNAME
                    //}
                }).FirstOrDefaultAsync();
            return result;
        }
    }
}
