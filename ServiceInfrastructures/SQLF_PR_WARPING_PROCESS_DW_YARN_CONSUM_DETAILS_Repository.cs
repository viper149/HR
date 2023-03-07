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
    public class SQLF_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS_Repository:BaseRepository<F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS>, IF_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS
    {
        public SQLF_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS>> GetInitCountData(
            List<F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS> fPrWarpingProcessDwYarnConsumDetails)
        {
            try
            {
                foreach (var item in fPrWarpingProcessDwYarnConsumDetails)
                {
                    item.COUNT_ = await DenimDbContext.RND_FABRIC_COUNTINFO.Include(c=>c.COUNT)
                        .FirstOrDefaultAsync(c => c.COUNTID.Equals(item.COUNT_ID)) ?? await DenimDbContext.RND_FABRIC_COUNTINFO
                        .Include(c => c.COUNT).FirstOrDefaultAsync(c =>
                            c.TRNSID.Equals(item.COUNT_ID));
                }

                return fPrWarpingProcessDwYarnConsumDetails;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
