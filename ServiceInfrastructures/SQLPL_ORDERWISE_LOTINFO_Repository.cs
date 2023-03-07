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
    public class SQLPL_ORDERWISE_LOTINFO_Repository: BaseRepository<PL_ORDERWISE_LOTINFO>, IPL_ORDERWISE_LOTINFO
    {
        public SQLPL_ORDERWISE_LOTINFO_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<PL_ORDERWISE_LOTINFO>> GetInitObjects(List<PL_ORDERWISE_LOTINFO> plOrderwiseLotInfos)
        {
            try
            {
                foreach (var item in plOrderwiseLotInfos)
                {
                    item.LOT = await DenimDbContext.BAS_YARN_LOTINFO.FirstOrDefaultAsync(c => c.LOTID.Equals(item.LOTID));
                    item.SUPP = await DenimDbContext.BAS_SUPPLIERINFO.FirstOrDefaultAsync(c => c.SUPPID.Equals(item.SUPPID));
                    item.YARNFOR = await DenimDbContext.YARNFOR.FirstOrDefaultAsync(c => c.YARNID.Equals(item.YARNTYPE));
                }
                return plOrderwiseLotInfos;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<PL_ORDERWISE_LOTINFO>> FindByPoIdAsync(int poid)
        {
            try
            {
                var plOrderwiseLotInfos =
                    await DenimDbContext.PL_ORDERWISE_LOTINFO.Where(c => c.POID.Equals(poid)).ToListAsync();

                foreach (var item in plOrderwiseLotInfos)
                {
                    item.LOT = await DenimDbContext.BAS_YARN_LOTINFO.FirstOrDefaultAsync(c => c.LOTID.Equals(item.LOTID));
                    item.SUPP = await DenimDbContext.BAS_SUPPLIERINFO.FirstOrDefaultAsync(c => c.SUPPID.Equals(item.SUPPID));
                    item.YARNFOR = await DenimDbContext.YARNFOR.FirstOrDefaultAsync(c => c.YARNID.Equals(item.YARNTYPE));
                }
                return plOrderwiseLotInfos;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
