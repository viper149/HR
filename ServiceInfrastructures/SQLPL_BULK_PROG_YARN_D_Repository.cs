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
    public class SQLPL_BULK_PROG_YARN_D_Repository: BaseRepository<PL_BULK_PROG_YARN_D>, IPL_BULK_PROG_YARN_D
    {
        public SQLPL_BULK_PROG_YARN_D_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<PL_BULK_PROG_YARN_D>> GetYarnListByProgId(int progId)
        {
            try
            {
                var result = await DenimDbContext.PL_BULK_PROG_YARN_D
                    .Where(c => c.PROG_ID.Equals(progId))
                    .ToListAsync();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<RND_FABRIC_COUNTINFO> GetCountDetails(int countId)
        {
            try
            {
                var result = await DenimDbContext.RND_FABRIC_COUNTINFO
                    .Include(c=>c.YarnFor)
                    .FirstOrDefaultAsync(c => c.TRNSID.Equals(countId));
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
