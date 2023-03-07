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
    public class SQLF_BAS_SUBSECTION_Repository : BaseRepository<F_BAS_SUBSECTION>, IF_BAS_SUBSECTION
    {
        public SQLF_BAS_SUBSECTION_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {

        }

        public async Task<bool> FindBySubSectionNameAsync(string subSectionName)
        {
            return await DenimDbContext.F_BAS_SUBSECTION.AnyAsync(e => e.SSECNAME.Equals(subSectionName));
        }

        public async Task<IEnumerable<F_BAS_SUBSECTION>> GetSubSectionsBySectionIdAsync(int sectionId)
        {
            return await DenimDbContext.F_BAS_SUBSECTION
                .Where(e => e.SECID.Equals(sectionId))
                .Select(d=> new F_BAS_SUBSECTION
                {
                    SSECID = d.SSECID,
                    SSECNAME = d.SSECNAME
                })
                .ToListAsync();
        }
    }
}