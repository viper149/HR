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
    public class SQLPL_PRODUCTION_SETDISTRIBUTION_Repository: BaseRepository<PL_PRODUCTION_SETDISTRIBUTION>, IPL_PRODUCTION_SETDISTRIBUTION
    {
        public SQLPL_PRODUCTION_SETDISTRIBUTION_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<List<PL_PRODUCTION_SETDISTRIBUTION>> GetSetListBySubGroup(int subGroup)
        {
            try
            {
                return await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION.Where(c => c.SUBGROUPID.Equals(subGroup))
                    .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
