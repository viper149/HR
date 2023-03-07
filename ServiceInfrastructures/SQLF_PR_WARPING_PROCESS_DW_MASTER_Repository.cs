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
    public class SQLF_PR_WARPING_PROCESS_DW_MASTER_Repository : BaseRepository<F_PR_WARPING_PROCESS_DW_MASTER>, IF_PR_WARPING_PROCESS_DW_MASTER
    {
        public SQLF_PR_WARPING_PROCESS_DW_MASTER_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<F_PR_WARPING_PROCESS_DW_MASTER>> GetAllAsync()
        {
            try
            {
                var result = await DenimDbContext.F_PR_WARPING_PROCESS_DW_MASTER
                    .Include(c => c.SET.PROG_)
                    .ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<PrWarpingProcessSlasherViewModel> GetInitObjects(PrWarpingProcessSlasherViewModel prWarpingProcessSlasherViewModel)
        {
            try
            {
                if (prWarpingProcessSlasherViewModel.FPrWarpingProcessDwMaster == null)
                {
                    prWarpingProcessSlasherViewModel.PlProductionSetDistributions = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                        .Include(c => c.PROG_)
                        .Include(c => c.SUBGROUP.GROUP)
                        .Where(c => c.SUBGROUP.GROUP.DYEING_TYPE.Equals(2005) && !DenimDbContext.F_PR_WARPING_PROCESS_DW_MASTER.Any(e => e.SETID.Equals(c.SETID)))
                        .Select(c => new PL_PRODUCTION_SETDISTRIBUTION()
                        {
                            SETID = c.SETID,
                            PROG_ = c.PROG_
                        }).ToListAsync();
                }
                else
                {
                    prWarpingProcessSlasherViewModel.PlProductionSetDistributions = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                        .Include(c => c.PROG_)
                        .Include(c => c.SUBGROUP.GROUP)
                        .Where(c => c.SUBGROUP.GROUP.DYEING_TYPE.Equals(2005) && DenimDbContext.F_PR_WARPING_PROCESS_DW_MASTER.Any(e => e.SETID.Equals(c.SETID)))
                        .Select(c => new PL_PRODUCTION_SETDISTRIBUTION()
                        {
                            SETID = c.SETID,
                            PROG_ = c.PROG_
                        }).ToListAsync();
                }

                prWarpingProcessSlasherViewModel.BasBallInfos = await DenimDbContext.F_BAS_BALL_INFO
                    .Where(c=>c.FOR_.Equals("SLASHER"))
                    .Select(c => new F_BAS_BALL_INFO()
                    {
                        BALLID = c.BALLID,
                        BALL_NO = c.BALL_NO
                    }).ToListAsync();

                prWarpingProcessSlasherViewModel.FPrWarpingMachines = await DenimDbContext.F_PR_WARPING_MACHINE
                    .Where(c => c.TYPE.Equals("SLASHER"))
                    .ToListAsync();

                return prWarpingProcessSlasherViewModel;
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
                    .Where(c=>c.SETID.Equals(setId))
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


        public async Task<PrWarpingProcessSlasherViewModel> FindAllByIdAsync(int id)
        {
            try
            {
                var result = new PrWarpingProcessSlasherViewModel
                {
                    FPrWarpingProcessDwMaster = await DenimDbContext.F_PR_WARPING_PROCESS_DW_MASTER
                        .Where(c => c.WARPID.Equals(id)).FirstOrDefaultAsync(),
                    FPrWarpingProcessDwDetailsList = await DenimDbContext.F_PR_WARPING_PROCESS_DW_DETAILS
                        .Where(c => c.WARP_ID.Equals(id)).ToListAsync(),
                    FPrWarpingProcessDwYarnConsumDetailsList = await DenimDbContext
                        .F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS.Where(c => c.WARP_ID.Equals(id)).ToListAsync()
                };
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<WarpingChartDataViewModel> GetDirectWarpingProductionData()
        {
            var date = Convert.ToDateTime("2022-02-27");
            var defaultDate = Convert.ToDateTime("1501-01-01 00:00:00.000");
            var warpChartData = new WarpingChartDataViewModel();

            warpChartData.TotalDirectWarping = await DenimDbContext.F_PR_WARPING_PROCESS_DW_MASTER
                .Select(d => new F_PR_WARPING_PROCESS_DW_MASTER()
                {
                    WARPLENGTH = d.WARPLENGTH
                }).SumAsync(c => Convert.ToDouble(c.WARPLENGTH ?? "0"));

            warpChartData.MonthlyDirectWarping = await DenimDbContext.F_PR_WARPING_PROCESS_DW_MASTER
                .Where(c => (c.DEL_DATE ?? defaultDate).ToString("yyyy-MM").Equals(date.ToString("yyyy-MM")))
                .Select(d => new F_PR_WARPING_PROCESS_DW_MASTER()
                {
                    WARPLENGTH = d.WARPLENGTH
                }).SumAsync(c => Convert.ToDouble(c.WARPLENGTH?? "0"));

            warpChartData.ComparisonMonthlyDirectWarping = await DenimDbContext.F_PR_WARPING_PROCESS_DW_MASTER
                .Where(c => (c.DEL_DATE ?? defaultDate).ToString("yyyy-MM").Equals(date.ToString("yyyy-MM")))
                .Select(d => new F_PR_WARPING_PROCESS_DW_MASTER()
                {
                    WARPLENGTH = d.WARPLENGTH 
                }).SumAsync(c => Convert.ToDouble(c.WARPLENGTH ?? "0")) - await DenimDbContext.F_PR_WARPING_PROCESS_DW_MASTER
                .Where(c => (c.DEL_DATE ?? defaultDate).ToString("yyyy-MM").Equals(date.AddMonths(-1).ToString("yyyy-MM")))
                .Select(d => new F_PR_WARPING_PROCESS_DW_MASTER()
                {
                    WARPLENGTH = d.WARPLENGTH
                }).SumAsync(c =>Convert.ToDouble(c.WARPLENGTH ?? "0"));
            warpChartData.TodaysDirectWarping = await DenimDbContext.F_PR_WARPING_PROCESS_DW_MASTER
                .Where(c => c.DEL_DATE.Equals(Convert.ToDateTime("2022-05-08 00:00:00.000").Date))
                .Select(d => new F_PR_WARPING_PROCESS_DW_MASTER()
                {
                    WARPLENGTH = d.WARPLENGTH
                }).SumAsync(c => Convert.ToDouble(c.WARPLENGTH ?? "0"));
            return warpChartData;

        }

        public async Task<List<WarpingChartDataViewModel>> GetDirectWarpingProductionList()
        {
            try
            {
                var defaultDate = Convert.ToDateTime("1501-01-01 00:00:00.000");
                var startDate = Convert.ToDateTime("2022-03-01");
                var endDate = Convert.ToDateTime("2022-03-31");

                var result = await DenimDbContext.F_PR_WARPING_PROCESS_DW_MASTER
                    .Where(c => (c.DEL_DATE ?? defaultDate) >= startDate && (c.DEL_DATE ?? defaultDate) <= endDate)
                    .GroupBy(c => (c.DEL_DATE ?? defaultDate).Date)
                    .Select(c => new WarpingChartDataViewModel
                    {
                        Date = c.Key,
                        TotalDirectWarping = c.Sum(c =>Convert.ToDouble(c.WARPLENGTH ?? "0"))
                    })
                    .OrderBy(c => c.Date)
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
