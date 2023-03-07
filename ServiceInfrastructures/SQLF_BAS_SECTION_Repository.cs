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
    public class SQLF_BAS_SECTION_Repository : BaseRepository<F_BAS_SECTION>, IF_BAS_SECTION
    {
        public SQLF_BAS_SECTION_Repository(DenimDbContext denimDbContext) : base(denimDbContext) {}

        public async Task<IEnumerable<F_BAS_SECTION>> GetSectionsByDeptIdAsync(int id)
        {
            return await DenimDbContext.F_BAS_SECTION
                    .Where(d => d.DEPTID == id)
                    .Select(d => new F_BAS_SECTION
                    {
                        SECID = d.SECID,
                        SECNAME = d.SECNAME
                    })
                    .ToListAsync();
        }

        public async Task<F_BAS_SECTION> GetSubSectionBySectionId(int id)
        {
            try
            {
                var result = await DenimDbContext.F_BAS_SECTION
                    .Include(c => c.F_BAS_SUBSECTION)
                    .FirstOrDefaultAsync(c => c.SECID.Equals(id));
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> FindBySectionNameAsync(string sectionName)
        {
            return await DenimDbContext.F_BAS_SECTION.AnyAsync(e => e.SECNAME.Equals(sectionName));
        }
    }
}
