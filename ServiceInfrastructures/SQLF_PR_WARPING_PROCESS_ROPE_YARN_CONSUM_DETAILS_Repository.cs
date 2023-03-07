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
    public class SQLF_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS_Repository: BaseRepository<F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS>, IF_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS
    {
        public SQLF_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS>> GetInitCountData(List<F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS> fPrWarpingProcessRopeYarnConsumDetails)
        {
            try
            {
                foreach (var item in fPrWarpingProcessRopeYarnConsumDetails)
                {
                   item.COUNT_ = await DenimDbContext.BAS_YARN_COUNTINFO.FirstOrDefaultAsync(c =>
                        c.COUNTID.Equals(item.COUNT_ID));
                }
                return fPrWarpingProcessRopeYarnConsumDetails;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
