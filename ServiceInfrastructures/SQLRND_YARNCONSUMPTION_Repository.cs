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
    public class SQLRND_YARNCONSUMPTION_Repository : BaseRepository<RND_YARNCONSUMPTION>, IRND_YARNCONSUMPTION
    {
        public SQLRND_YARNCONSUMPTION_Repository(DenimDbContext denimDbContext) :
            base(denimDbContext)
        {
        }

        public async Task<IEnumerable<RND_YARNCONSUMPTION>> FindByFabCodeIAsync(int fabCode)
        {
            try
            {
                var rndYarnConsumptionList = await DenimDbContext.RND_YARNCONSUMPTION
                    .Include(c => c.COUNT)
                    .Where(ryc => ryc.FABCODE.Equals(fabCode)).ToListAsync();
                return rndYarnConsumptionList;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<double> GetConsumptionByCountIdAndFabCodeAsync(int countId, int fabCode, int yarnFor)
        {
            try
            {
                var rndYarnConsumptionList = await DenimDbContext.RND_YARNCONSUMPTION
                                                    .Where(y => y.FABCODE.Equals(fabCode)
                                                        && y.COUNTID.Equals(countId) && y.YARNFOR.Equals(yarnFor)).ToListAsync();
                var amount = rndYarnConsumptionList.Select(c => c.AMOUNT).Sum();

                return amount ?? 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<RND_YARNCONSUMPTION> GetPrimaryKeyByCountIdAndFabCodeAsync(int countId, int fabCode, int yarnFor, int? color)
        {
            try
            {
                var rndYarnConsumptionDetails = await DenimDbContext.RND_YARNCONSUMPTION
                    .Where(y => y.FABCODE.Equals(fabCode) && y.COUNTID.Equals(countId) && y.YARNFOR.Equals(yarnFor) && y.COLOR.Equals(color))
                    .FirstOrDefaultAsync() ?? await DenimDbContext.RND_YARNCONSUMPTION
                    .Where(y => y.FABCODE.Equals(fabCode) && y.COUNTID.Equals(countId) && y.YARNFOR.Equals(yarnFor))
                    .FirstOrDefaultAsync();

                return rndYarnConsumptionDetails;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
