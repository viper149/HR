using System;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_WARPING_PROCESS_DW_DETAILS_Repository:BaseRepository<F_PR_WARPING_PROCESS_DW_DETAILS>, IF_PR_WARPING_PROCESS_DW_DETAILS
    {
        public SQLF_PR_WARPING_PROCESS_DW_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<PrWarpingProcessSlasherViewModel> GetInitSoData(PrWarpingProcessSlasherViewModel prWarpingProcessSlasherViewModel)
        {
            try
            {
                foreach (var item in prWarpingProcessSlasherViewModel.FPrWarpingProcessDwDetailsList)
                {
                    item.BALL_NONavigation = await DenimDbContext.F_BAS_BALL_INFO
                        .FirstOrDefaultAsync(c => c.BALLID.Equals(item.BALL_NO));
                    item.LINK_BALL_NONavigation = await DenimDbContext.F_BAS_BALL_INFO.FirstOrDefaultAsync(
                            c => c.BALLID.Equals(item.LINK_BALL_NO));
                    item.COUNT = await DenimDbContext.RND_FABRIC_COUNTINFO
                        .Include(c=>c.COUNT)
                        .FirstOrDefaultAsync(c => c.COUNTID.Equals(item.COUNTID)) ?? await DenimDbContext.RND_FABRIC_COUNTINFO
                        .Include(c => c.COUNT)
                        .FirstOrDefaultAsync(c => c.TRNSID.Equals(item.COUNTID));

                    item.MACHINE_ = await DenimDbContext.F_PR_WARPING_MACHINE.FirstOrDefaultAsync(c => c.ID.Equals(item.MACHINE_ID));
                }
                return prWarpingProcessSlasherViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
