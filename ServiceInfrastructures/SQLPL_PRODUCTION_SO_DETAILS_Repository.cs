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
    public class SQLPL_PRODUCTION_SO_DETAILS_Repository: BaseRepository<PL_PRODUCTION_SO_DETAILS>, IPL_PRODUCTION_SO_DETAILS
    {
        public SQLPL_PRODUCTION_SO_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<PL_PRODUCTION_SO_DETAILS> FindBySetIdAsync(int progId)
        {
            try
            {
                var result = await DenimDbContext.PL_BULK_PROG_SETUP_D
                    .Include(c => c.BLK_PROG_.RndProductionOrder)
                    .Where(c => c.PROG_ID.Equals(progId))
                    .Select(c=>new PL_PRODUCTION_SO_DETAILS
                    {
                        POID = c.BLK_PROG_.RndProductionOrder.POID
                    })
                    .FirstOrDefaultAsync();

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
