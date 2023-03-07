using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Factory.Production;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_WARPING_PROCESS_ROPE_DETAILS_Repository : BaseRepository<F_PR_WARPING_PROCESS_ROPE_DETAILS>, IF_PR_WARPING_PROCESS_ROPE_DETAILS
    {
        public SQLF_PR_WARPING_PROCESS_ROPE_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<PrWarpingProcessRopeViewModel> GetInitSoData(PrWarpingProcessRopeViewModel prWarpingProcessRopeViewModel)
        {
            try
            {
                foreach (var i in prWarpingProcessRopeViewModel.FPrWarpingProcessRopeBallDetailsList)
                {
                    i.COUNT_ = await DenimDbContext.BAS_YARN_COUNTINFO
                        .FirstOrDefaultAsync(c => c.COUNTID.Equals(i.COUNT_ID));
                    i.BALL_ID_FKNavigation = await DenimDbContext.F_BAS_BALL_INFO.FirstOrDefaultAsync(c => c.BALLID.Equals(i.BALL_ID_FK));
                    i.EMP = await DenimDbContext.F_HRD_EMPLOYEE
                            .Select(c => new F_HRD_EMPLOYEE
                            {
                                EMPID = c.EMPID,
                                FIRST_NAME = c.FIRST_NAME + ' ' + c.LAST_NAME
                            })
                            .FirstOrDefaultAsync(c => c.EMPID.Equals(i.OPERATOR))
                        ;

                    if (i.LINK_BALL_NO != 0)
                    {
                        i.BALL_ID_FK_Link = await DenimDbContext.F_BAS_BALL_INFO.FirstOrDefaultAsync(c => c.BALLID.Equals(i.LINK_BALL_NO));
                    }

                    i.MACHINE_NONavigation = await DenimDbContext.F_PR_WARPING_MACHINE.FirstOrDefaultAsync(c => c.ID.Equals(i.MACHINE_NO));

                }
                return prWarpingProcessRopeViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> InsertAndGetIdAsync(F_PR_WARPING_PROCESS_ROPE_DETAILS prWarpingProcessRopeDetails)
        {
            try
            {
                await DenimDbContext.F_PR_WARPING_PROCESS_ROPE_DETAILS.AddAsync(prWarpingProcessRopeDetails);
                await SaveChanges();
                return prWarpingProcessRopeDetails.WARP_PROG_ID;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<PL_PRODUCTION_PLAN_DETAILS> GetSetList(int subGroupId)
        {
            try
            {
                var result = await DenimDbContext.PL_PRODUCTION_PLAN_DETAILS
                    .Include(c=>c.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c=>c.PROG_.BLK_PROG_.RndProductionOrder)
                    .Where(c => c.SUBGROUPID.Equals(subGroupId))
                    .FirstOrDefaultAsync();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<F_PR_WARPING_PROCESS_ROPE_DETAILS>> GetWarpSetList(int warpId)
        {
            try
            {
                var result = await DenimDbContext.F_PR_WARPING_PROCESS_ROPE_DETAILS
                    .Include(c=>c.PL_PRODUCTION_SETDISTRIBUTION.PROG_.BLK_PROG_.RndProductionOrder)
                    .Where(c => c.WARPID.Equals(warpId))
                    .ToListAsync();

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
