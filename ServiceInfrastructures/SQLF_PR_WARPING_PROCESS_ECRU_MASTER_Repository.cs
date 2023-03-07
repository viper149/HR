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
    public class SQLF_PR_WARPING_PROCESS_ECRU_MASTER_Repository : BaseRepository<F_PR_WARPING_PROCESS_ECRU_MASTER>, IF_PR_WARPING_PROCESS_ECRU_MASTER
    {
        public SQLF_PR_WARPING_PROCESS_ECRU_MASTER_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<List<F_PR_WARPING_PROCESS_ECRU_MASTER>> GetAllAsync()
        {

            try
            {
                var result = await DenimDbContext.F_PR_WARPING_PROCESS_ECRU_MASTER
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

        public async Task<FPrWarpingProcessEkruMasterViewModel> GetInitObjects(FPrWarpingProcessEkruMasterViewModel fPrWarpingProcessEkruMasterViewModel, bool edit = false)
        {
            try
            {
                if (!edit)
                {
                    fPrWarpingProcessEkruMasterViewModel.PlProductionSetDistributions = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                        .Include(c => c.PROG_)
                        .Include(c => c.SUBGROUP.GROUP)
                        .Where(c => c.SUBGROUP.GROUP.DYEING_TYPE.Equals(2004) && !DenimDbContext.F_PR_WARPING_PROCESS_ECRU_MASTER.Any(e => e.SETID.Equals(c.SETID)))
                        .Select(c => new PL_PRODUCTION_SETDISTRIBUTION()
                        {
                            SETID = c.SETID,
                            PROG_ = c.PROG_
                        }).ToListAsync();
                }
                else
                {
                    fPrWarpingProcessEkruMasterViewModel.PlProductionSetDistributions = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                        .Include(c => c.PROG_)
                        .Include(c => c.SUBGROUP.GROUP)
                        .Where(c => c.SUBGROUP.GROUP.DYEING_TYPE.Equals(2004) && DenimDbContext.F_PR_WARPING_PROCESS_ECRU_MASTER.Any(e => e.SETID.Equals(c.SETID)))
                        .Select(c => new PL_PRODUCTION_SETDISTRIBUTION()
                        {
                            SETID = c.SETID,
                            PROG_ = c.PROG_
                        }).ToListAsync();
                }

                fPrWarpingProcessEkruMasterViewModel.FBasBallInfoList = await DenimDbContext.F_BAS_BALL_INFO
                    .Where(c => c.FOR_.Equals("SLASHER"))
                    .Select(c => new F_BAS_BALL_INFO()
                    {
                        BALLID = c.BALLID,
                        BALL_NO = c.BALL_NO
                    }).ToListAsync();

                fPrWarpingProcessEkruMasterViewModel.FPrWarpingMachineList = await DenimDbContext.F_PR_WARPING_MACHINE
                    //.Where(c => c.TYPE.Equals("SLASHER"))
                    .ToListAsync();

                fPrWarpingProcessEkruMasterViewModel.RndFabricCountinfoList = await DenimDbContext.RND_FABRIC_COUNTINFO
                    .Include(c => c.COUNT)
                    .Select(c => new RND_FABRIC_COUNTINFO
                    {
                        TRNSID = c.TRNSID,
                        YARNTYPE = c.COUNT.COUNTNAME
                    })
                    //.Where(c => c.TYPE.Equals("SLASHER"))
                    .ToListAsync();

                return fPrWarpingProcessEkruMasterViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }




        public async Task<FPrWarpingProcessEkruMasterViewModel> FindAllByIdAsync(int id)
        {
            try
            {
                var result = new FPrWarpingProcessEkruMasterViewModel
                {
                    FPrWarpingProcessEcruMaster = await DenimDbContext.F_PR_WARPING_PROCESS_ECRU_MASTER
                        .Where(c => c.ECRUID.Equals(id)).FirstOrDefaultAsync(),
                    FPrWarpingProcessEcruDetailsList = await DenimDbContext.F_PR_WARPING_PROCESS_ECRU_DETAILS
                        .Where(c => c.ECRU_ID.Equals(id)).ToListAsync(),
                    FPrWarpingProcessEcruYarnConsumDetailsList = await DenimDbContext
                        .F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS.Where(c => c.ECRU_ID.Equals(id)).ToListAsync()
                };
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<WarpingChartDataViewModel> GetEcruWarpingProductionData()
        {
            var date = Convert.ToDateTime("2022-02-27");
            var defaultDate = Convert.ToDateTime("1501-01-01 00:00:00.000");
            var warpChartData = new WarpingChartDataViewModel();

            warpChartData.TotalEcruWarping = await DenimDbContext.F_PR_WARPING_PROCESS_ECRU_MASTER
                .Select(d => new F_PR_WARPING_PROCESS_ECRU_MASTER()
                {
                    WARPLENGTH = d.WARPLENGTH
                }).SumAsync(c => c.WARPLENGTH);

            warpChartData.MonthlyEcruWarping = await DenimDbContext.F_PR_WARPING_PROCESS_ECRU_MASTER
                .Where(c => (c.DEL_DATE ?? defaultDate).ToString("yyyy-MM").Equals(date.ToString("yyyy-MM")))
                .Select(d => new F_PR_WARPING_PROCESS_ECRU_MASTER()
                {
                    WARPLENGTH = d.WARPLENGTH
                }).SumAsync(c => c.WARPLENGTH);

            warpChartData.ComparisonMonthlyEcruWarping = await DenimDbContext.F_PR_WARPING_PROCESS_ECRU_MASTER
                .Where(c => (c.DEL_DATE ?? defaultDate).ToString("yyyy-MM").Equals(date.ToString("yyyy-MM")))
                .Select(d => new F_PR_WARPING_PROCESS_ECRU_MASTER()
                {
                    WARPLENGTH = d.WARPLENGTH
                }).SumAsync(c => c.WARPLENGTH) - await DenimDbContext.F_PR_WARPING_PROCESS_ECRU_MASTER
                .Where(c => (c.DEL_DATE ?? defaultDate).ToString("yyyy-MM").Equals(date.AddMonths(-1).ToString("yyyy-MM")))
                .Select(d => new F_PR_WARPING_PROCESS_ECRU_MASTER()
                {
                    WARPLENGTH = d.WARPLENGTH
                }).SumAsync(c => c.WARPLENGTH);
            warpChartData.TodaysEcruWarping = await DenimDbContext.F_PR_WARPING_PROCESS_ECRU_MASTER
                .Where(c => c.DEL_DATE.Equals(Convert.ToDateTime("2022-05-08 00:00:00.000").Date))
                .Select(d => new F_PR_WARPING_PROCESS_ECRU_MASTER()
                {
                    WARPLENGTH = d.WARPLENGTH
                }).SumAsync(c => c.WARPLENGTH);
            return warpChartData;
        }

        //public Task<List<WarpingChartDataViewModel>> GetEcruWarpingProductionList()
        //{
        //    throw new NotImplementedException();ssss
        //}

        public async Task<List<WarpingChartDataViewModel>> GetEcruWarpingProductionList()
        {
            try
            {
                var defaultDate = Convert.ToDateTime("1501-01-01 00:00:00.000");
                var startDate = Convert.ToDateTime("2022-03-01");
                var endDate = Convert.ToDateTime("2022-03-31");

                var result = await DenimDbContext.F_PR_WARPING_PROCESS_ECRU_MASTER
                    .Where(c => (c.DEL_DATE ?? defaultDate) >= startDate && (c.DEL_DATE ?? defaultDate) <= endDate)
                    .GroupBy(c => (c.DEL_DATE ?? defaultDate).Date)
                    .Select(g => new WarpingChartDataViewModel
                    {
                        Date = g.Key,
                        TotalEcruWarping = g.Sum(c => c.WARPLENGTH ?? 0)
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

