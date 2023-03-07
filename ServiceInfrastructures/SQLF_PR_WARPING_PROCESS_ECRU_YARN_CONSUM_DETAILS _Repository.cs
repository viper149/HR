using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS__Repository : BaseRepository<F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS>, IF_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS
    {
        public SQLF_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS__Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS>> GetInitCountData(List<F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS> fPrWarpingProcessEcruYarnConsumDetailsList)
        {
            try
            {
                foreach (var item in fPrWarpingProcessEcruYarnConsumDetailsList)
                {
                    item.COUNT_ = await DenimDbContext.RND_FABRIC_COUNTINFO.Include(c => c.COUNT)
                        .FirstOrDefaultAsync(c => c.COUNTID.Equals(item.COUNT_ID)) ?? await DenimDbContext.RND_FABRIC_COUNTINFO
                        .Include(c => c.COUNT).FirstOrDefaultAsync(c =>
                            c.TRNSID.Equals(item.COUNT_ID));
                }

                return fPrWarpingProcessEcruYarnConsumDetailsList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
