using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_WARPING_PROCESS_SW_MASTER_Repository : BaseRepository<F_PR_WARPING_PROCESS_SW_MASTER>, IF_PR_WARPING_PROCESS_SW_MASTER
    {
        public SQLF_PR_WARPING_PROCESS_SW_MASTER_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
        public async Task<List<F_PR_WARPING_PROCESS_SW_MASTER>> GetAllAsync()
        {

            try
            {
                var result = await DenimDbContext.F_PR_WARPING_PROCESS_SW_MASTER

                    .ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FPrWarpingProcessSwMasterViewModel> GetInitObjects(FPrWarpingProcessSwMasterViewModel prWarpingProcessSwMasterViewModel)
        {
            try
            {
                if (prWarpingProcessSwMasterViewModel.FPrWarpingProcessSwMaster == null)
                {
                    prWarpingProcessSwMasterViewModel.PlProductionSetDistributions = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                        .Include(c => c.PROG_)
                        .Include(c => c.SUBGROUP.GROUP)
                        .Where(c => c.SUBGROUP.GROUP.DYEING_TYPE.Equals(2006) && !DenimDbContext.F_PR_WARPING_PROCESS_SW_MASTER.Any(e => e.SETID.Equals(c.SETID)))
                        .Select(c => new PL_PRODUCTION_SETDISTRIBUTION()
                        {
                            SETID = c.SETID,
                            PROG_ = c.PROG_
                        }).ToListAsync();
                }
                else
                {
                    prWarpingProcessSwMasterViewModel.PlProductionSetDistributions = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                        .Include(c => c.PROG_)
                        .Include(c => c.SUBGROUP.GROUP)
                        .Where(c => c.SUBGROUP.GROUP.DYEING_TYPE.Equals(2005) && DenimDbContext.F_PR_WARPING_PROCESS_SW_MASTER.Any(e => e.SETID.Equals(c.SETID)))
                        .Select(c => new PL_PRODUCTION_SETDISTRIBUTION()
                        {
                            SETID = c.SETID,
                            PROG_ = c.PROG_
                        }).ToListAsync();
                }

                prWarpingProcessSwMasterViewModel.BasBallInfos = await DenimDbContext.F_BAS_BALL_INFO
                    .Where(c => c.FOR_.Equals("SLASHER"))
                    .Select(c => new F_BAS_BALL_INFO()
                    {
                        BALLID = c.BALLID,
                        BALL_NO = c.BALL_NO
                    }).ToListAsync();

                prWarpingProcessSwMasterViewModel.FPrWarpingMachines = await DenimDbContext.F_PR_WARPING_MACHINE
                    .Where(c => c.TYPE.Equals("SLASHER"))
                    .ToListAsync();

                return prWarpingProcessSwMasterViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<string> GetWarpLength(int? setId)
        {
            try
            {
                var result = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Include(c => c.PROG_)
                    .Where(c => c.SETID.Equals(setId))
                    .Select(c => c.PROG_.SET_QTY)
                    .FirstOrDefaultAsync();

                return result.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<WarpingChartDataViewModel>> GetSectionalWarpingProductionList()
        {
            try
            {
                var defaultDate = Convert.ToDateTime("1501-01-01 00:00:00.000");
                var startDate = Convert.ToDateTime("2022-03-01");
                var endDate = Convert.ToDateTime("2022-03-31");

                var result = await DenimDbContext.F_PR_WARPING_PROCESS_SW_MASTER
                    .Where(c => (c.DEL_DATE ?? defaultDate) >= startDate && (c.DEL_DATE ?? defaultDate) <= endDate) 
                    .GroupBy(c => (c.DEL_DATE ?? defaultDate).Date)
                    .Select(g => new WarpingChartDataViewModel
                    {
                        Date = g.Key,
                        TotalSectionalWarping = g.Sum(c => Convert.ToDouble(c.WARPLENGTH ?? "0"))
                    })
                    .OrderBy(d => d.Date)
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
