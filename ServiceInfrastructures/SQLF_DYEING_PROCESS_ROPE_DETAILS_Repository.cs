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
    public class SQLF_DYEING_PROCESS_ROPE_DETAILS_Repository : BaseRepository<F_DYEING_PROCESS_ROPE_DETAILS>, IF_DYEING_PROCESS_ROPE_DETAILS
    {
        public SQLF_DYEING_PROCESS_ROPE_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
        public async Task<IEnumerable<F_DYEING_PROCESS_ROPE_DETAILS>> GetInitBallData(
            List<F_DYEING_PROCESS_ROPE_DETAILS> fDyeingProcessRopeDetailsList)
        {
            try
            {
                foreach (var item in fDyeingProcessRopeDetailsList)
                {
                    item.SUBGROUP = await DenimDbContext.PL_PRODUCTION_PLAN_DETAILS
                        .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                        .ThenInclude(c => c.PROG_.PROG_NO)
                        .Where(c => c.SUBGROUPID.Equals(item.SUBGROUPID))
                        .Select(c => new PL_PRODUCTION_PLAN_DETAILS
                        {
                            OPT1 = $"{c.SUBGROUPNO} - {c.PL_PRODUCTION_SETDISTRIBUTION.FirstOrDefault(e => !e.PROG_.PROG_NO.Contains("-")).PROG_.PROG_NO}"
                        })
                        .FirstOrDefaultAsync();

                    item.BALL = await DenimDbContext.F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS
                        .Include(c => c.BALL_ID_FKNavigation)
                        .FirstOrDefaultAsync(c => c.BALLID.Equals(item.BALLID));

                    item.ROPE_NONavigation = await DenimDbContext.F_PR_ROPE_INFO
                        .FirstOrDefaultAsync(c => c.ID.Equals(item.ROPE_NO));

                    item.R_MACHINE_NONavigation = await DenimDbContext.F_PR_ROPE_MACHINE_INFO
                        .FirstOrDefaultAsync(c => c.ID.Equals(item.R_MACHINE_NO));

                    item.CAN_NONavigation = await DenimDbContext.F_PR_TUBE_INFO
                        .FirstOrDefaultAsync(c => c.ID.Equals(item.CAN_NO));
                }

                return fDyeingProcessRopeDetailsList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<DyeingChartDataViewModel> GetDyeingPendingSets()
        {
            var date = Convert.ToDateTime("2022-02-27");
            var defaultDate = Convert.ToDateTime("1501-01-01 00:00:00.000");
            var dyeingChartDataViewModel = new DyeingChartDataViewModel();

            var totalSubGroup = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                .Where(c => (c.TRNSDATE ?? defaultDate).ToString("yyyy-MM--dd").Equals(date.ToString("yyyy-MM-dd")))
                .Select(c => c.SUBGROUPID).CountAsync();

            var totalSet = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                .Where(c => (c.TRNSDATE ?? defaultDate).ToString("yyyy-MM-dd").Equals(date.ToString("yyyy-MM-dd")))
                .Select(c => c.SETID).CountAsync();

            var TotalSets = totalSubGroup + totalSet;

            var ropeDyeingPendingSets = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                .Where(c => (c.TRNSDATE ?? defaultDate).ToString("yyyy-MM-dd").Equals(date.ToString("yyyy-MM-dd")))
                .CountAsync(c =>
                !DenimDbContext.F_DYEING_PROCESS_ROPE_DETAILS.Any(d => d.SUBGROUPID.Equals(c.SUBGROUPID)));

            var slasherDyeingPendingSet = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                .Where(c => (c.TRNSDATE ?? defaultDate).ToString("yyyy-MM-dd").Equals(date.ToString("yyyy-MM-dd")))
                .CountAsync(c =>
                !DenimDbContext.F_PR_SLASHER_DYEING_MASTER.Any(d => d.SETID.Equals(c.SETID)));

            var TotalPendingSets = ropeDyeingPendingSets + slasherDyeingPendingSet;

            var ropeDyeingCompeteSets = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                .Where(c => (c.TRNSDATE ?? defaultDate).ToString("yyyy-MM-dd").Equals(date.ToString("yyyy-MM-dd")))
                .CountAsync(c =>
                DenimDbContext.F_DYEING_PROCESS_ROPE_DETAILS.Any(d => d.SUBGROUPID.Equals(c.SUBGROUPID)));

            var slasherDyeingCompleteSets = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                .Where(c => (c.TRNSDATE ?? defaultDate).ToString("yyyy-MM-dd").Equals(date.ToString("yyyy-MM-dd")))
                .CountAsync(c =>
                DenimDbContext.F_DYEING_PROCESS_ROPE_DETAILS.Any(d => d.SUBGROUPID.Equals(c.SUBGROUPID)));
            var TotalCompleteSets = ropeDyeingCompeteSets + slasherDyeingCompleteSets;

            var dyeingPendingPercent = TotalPendingSets / TotalSets * 100;
            var dyeingCompletePercent = TotalCompleteSets / TotalSets * 100;

            //{
            //    DyeingPendingSets = await _denimDbContext.PL_PRODUCTION_SETDISTRIBUTION.CountAsync(c =>
            //        !_denimDbContext.F_PR_WARPING_PROCESS_ROPE_DETAILS.Any(e => e.SETID.Equals(c.SETID)) &&
            //        !_denimDbContext.F_PR_WARPING_PROCESS_DW_MASTER.Any(e => e.SETID.Equals(c.SETID)) &&
            //        !_denimDbContext.F_PR_WARPING_PROCESS_ECRU_MASTER.Any(e => e.SETID.Equals(c.SETID)) &&
            //        !_denimDbContext.F_PR_WARPING_PROCESS_SW_MASTER.Any(e => e.SETID.Equals(c.SETID))),

            //    DyeingCompleteSets = await _denimDbContext.PL_PRODUCTION_SETDISTRIBUTION.CountAsync(c =>
            //        _denimDbContext.F_PR_WARPING_PROCESS_ROPE_DETAILS.Any(e => e.SETID.Equals(c.SETID)) ||
            //        _denimDbContext.F_PR_WARPING_PROCESS_DW_MASTER.Any(e => e.SETID.Equals(c.SETID)) ||
            //        _denimDbContext.F_PR_WARPING_PROCESS_ECRU_MASTER.Any(e => e.SETID.Equals(c.SETID)) ||
            //        _denimDbContext.F_PR_WARPING_PROCESS_SW_MASTER.Any(e => e.SETID.Equals(c.SETID))),

            //    PendingPercent = float.Parse(Math.Round((await _denimDbContext.PL_PRODUCTION_SETDISTRIBUTION.CountAsync(c =>
            //        !_denimDbContext.F_PR_WARPING_PROCESS_ROPE_DETAILS.Any(e => e.SETID.Equals(c.SETID)) &&
            //        !_denimDbContext.F_PR_WARPING_PROCESS_DW_MASTER.Any(e => e.SETID.Equals(c.SETID)) &&
            //        !_denimDbContext.F_PR_WARPING_PROCESS_ECRU_MASTER.Any(e => e.SETID.Equals(c.SETID)) &&
            //        !_denimDbContext.F_PR_WARPING_PROCESS_SW_MASTER.Any(e => e.SETID.Equals(c.SETID)))) / Convert.ToSingle(await _denimDbContext.PL_PRODUCTION_SETDISTRIBUTION.Select(c => c.SETID).CountAsync()) * 100, 2).ToString("0.0")),


            //    CompletePercent = Math.Round((await _denimDbContext.PL_PRODUCTION_SETDISTRIBUTION.CountAsync(c =>
            //                                      _denimDbContext.F_PR_WARPING_PROCESS_ROPE_DETAILS.Any(e => e.SETID.Equals(c.SETID)) ||
            //                                      _denimDbContext.F_PR_WARPING_PROCESS_DW_MASTER.Any(e => e.SETID.Equals(c.SETID)) ||
            //                                      _denimDbContext.F_PR_WARPING_PROCESS_ECRU_MASTER.Any(e => e.SETID.Equals(c.SETID)) ||
            //                                      _denimDbContext.F_PR_WARPING_PROCESS_SW_MASTER.Any(e => e.SETID.Equals(c.SETID)))
            //                                  / Convert.ToSingle(await _denimDbContext.PL_PRODUCTION_SETDISTRIBUTION.Select(c => c.SETID).CountAsync())) * 100)

            //};
            dyeingChartDataViewModel.DyeingPendingSets = TotalPendingSets;
            dyeingChartDataViewModel.DyeingCompleteSets = TotalCompleteSets;
            dyeingChartDataViewModel.PendingPercent = dyeingPendingPercent;
            dyeingChartDataViewModel.CompletePercent = dyeingCompletePercent;



            return dyeingChartDataViewModel;
        }

        public async Task<DyeingChartDataViewModel> GetMonthlyDyeingPendingAndCompleteSets()
        {
            var date = Convert.ToDateTime("2022-02-27");
            var defaultDate = Convert.ToDateTime("1501-01-01 00:00:00.000");
            var dyeingChartDataViewModel = new DyeingChartDataViewModel();

            var totalSubGroup = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                .Where(c => (c.TRNSDATE ?? defaultDate).ToString("yyyy-MM").Equals(date.ToString("yyyy-MM")))
                .Select(c => c.SUBGROUPID).CountAsync();
            var totalSet = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                .Where(c => (c.TRNSDATE ?? defaultDate).ToString("yyyy-MM").Equals(date.ToString("yyyy-MM")))
                .Select(c => c.SETID).CountAsync();
            var TotalSets = totalSubGroup + totalSet;


            var ropeDyeingPendingSets = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                .Where(c => (c.TRNSDATE ?? defaultDate).ToString("yyyy-MM").Equals(date.ToString("yyyy-MM")))
                .CountAsync(c =>
                    !DenimDbContext.F_DYEING_PROCESS_ROPE_DETAILS.Any(d => d.SUBGROUPID.Equals(c.SUBGROUPID)));
            var slasherDyeingPendingSet = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                .Where(c => (c.TRNSDATE ?? defaultDate).ToString("yyyy-MM").Equals(date.ToString("yyyy-MM")))
                .CountAsync(c =>
                    !DenimDbContext.F_PR_SLASHER_DYEING_MASTER.Any(d => d.SETID.Equals(c.SETID)));
            var mTotalPendingSets = ropeDyeingPendingSets + slasherDyeingPendingSet;

            var ropeDyeingCompeteSets = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                .Where(c => (c.TRNSDATE ?? defaultDate).ToString("yyyy-MM").Equals(date.ToString("yyyy-MM")))
                .CountAsync(c =>
                    DenimDbContext.F_DYEING_PROCESS_ROPE_DETAILS.Any(d => d.SUBGROUPID.Equals(c.SUBGROUPID)));

            var slasherDyeingCompleteSets = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                .Where(c => (c.TRNSDATE ?? defaultDate).ToString("yyyy-MM").Equals(date.ToString("yyyy-MM")))
                .CountAsync(c =>
                    DenimDbContext.F_DYEING_PROCESS_ROPE_DETAILS.Any(d => d.SUBGROUPID.Equals(c.SUBGROUPID)));
            var mTotalCompleteSets = ropeDyeingCompeteSets + slasherDyeingCompleteSets;


            var mTotalPendingPercent = (mTotalPendingSets / Convert.ToSingle(TotalSets)) * 100;
            var mTotalPendingPercentR = Math.Round(mTotalPendingPercent, 1);

            var mTotalCompletePercent = (mTotalCompleteSets / Convert.ToSingle(TotalSets)) * 100;
            var mTotalCompletePercentR = Math.Round(mTotalCompletePercent, 1);


            dyeingChartDataViewModel.MonthlyDyeingCompletePercent = mTotalCompletePercentR;
            dyeingChartDataViewModel.MonthlyDyeingPendingPercent = mTotalPendingPercentR;
            dyeingChartDataViewModel.MonthlyDyeingCompleteSets = mTotalCompleteSets;
            dyeingChartDataViewModel.MonthlyDyeingPendingSets = mTotalPendingSets;

            return dyeingChartDataViewModel;
        }

        public async Task<DyeingChartDataViewModel> GetDyeingChemicalConsumed()
        {
            var date = Convert.ToDateTime("2022-02-27");
            var defaultDate = Convert.ToDateTime("1501-01-01 00:00:00.000");

            return new DyeingChartDataViewModel()
            {

                ConsumedChemical = await DenimDbContext.F_DYEING_PROCESS_ROPE_CHEM
                .Where(c => (c.CREATED_AT ?? defaultDate).ToString("yyyy-MM-dd").Equals(date.ToString("yyyy-MM-dd")))
                .Select(c => c.QTY).SumAsync()

            };
        }

        public async Task<List<DyeingChartDataViewModel>> GetDyeingProductionList()
        {
            try

            {
                var data = new List<DyeingChartDataViewModel>();
                var date = Convert.ToDateTime("2022-02-27").AddDays(-15);
                var defaultDate = Convert.ToDateTime("1501-01-01 00:00:00.000");


                for (var i = 0; i < 15; i++)
                {
                    //var d = date.AddDays(i).Date;
                    //var t = Convert.ToDateTime("2022-02-27 00:00:00.000").ToString("yyyy-MM-dd");
                    //var total = await DenimDbContext.F_DYEING_PROCESS_ROPE_MASTER
                    //    .Where(c => (c.TRNSDATE ?? defaultDate).Date.Equals(date.AddDays(i).Date))
                    //    .SumAsync(c => c.DYEING_LENGTH ?? 0) 
                    //            + 
                    //            await DenimDbContext.F_PR_SLASHER_DYEING_DETAILS
                    //    .Include(c=>c.SL)
                    //    .Where(c => (c.SL.TRNSDATE ?? defaultDate).Date.Equals(date.AddDays(i).Date))
                    //    .SumAsync(c => Convert.ToDouble(c.LENGTH_PER_BEAM ?? 0));

                    //    var rope = await DenimDbContext.F_DYEING_PROCESS_ROPE_MASTER
                    //        .Where(c => (c.TRNSDATE ?? defaultDate).Date.Equals(date.AddDays(i).Date))
                    //        .SumAsync(c => c.DYEING_LENGTH ?? 0);

                    //var slasher = await DenimDbContext.F_PR_SLASHER_DYEING_DETAILS
                    //    .Include(c => c.SL)
                    //    .Where(c => (c.SL.TRNSDATE ?? defaultDate).Date.Equals(date.AddDays(i).Date))
                    //    .SumAsync(c => Convert.ToDouble(c.LENGTH_PER_BEAM ?? 0));

                    data.Add(new DyeingChartDataViewModel
                    {
                        date = date.AddDays(i),

                        TotalRopeDyeing = await DenimDbContext.F_DYEING_PROCESS_ROPE_MASTER
                            .Where(c => (c.TRNSDATE ?? defaultDate).Date.Equals(date.AddDays(i).Date))
                            .SumAsync(c => c.DYEING_LENGTH ?? 0),
                        TotalSlasherDyeing = await DenimDbContext.F_PR_SLASHER_DYEING_DETAILS
                            .Include(c => c.SL)
                            .Where(c => (c.SL.TRNSDATE ?? defaultDate).Date.Equals(date.AddDays(i).Date))
                            .SumAsync(c => Convert.ToDouble(c.LENGTH_PER_BEAM ?? 0)),


                        TotalDyeing = await DenimDbContext.F_DYEING_PROCESS_ROPE_MASTER
                                          .Where(c => (c.TRNSDATE ?? defaultDate).Date.Equals(date.AddDays(i).Date))
                                          .SumAsync(c => c.DYEING_LENGTH ?? 0)
                                      +
                                      await DenimDbContext.F_PR_SLASHER_DYEING_DETAILS
                                          .Include(c => c.SL)
                                          .Where(c => (c.SL.TRNSDATE ?? defaultDate).Date.Equals(date.AddDays(i).Date))
                                          .SumAsync(c => Convert.ToDouble(c.LENGTH_PER_BEAM ?? 0))

                    });
                }


                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<DyeingChartDataViewModel> GetDyeingProductionData()
        {
            var date = Convert.ToDateTime("2022-02-28");
            var defaultDate = Convert.ToDateTime("1501-01-01 00:00:00.000");
            var dyeingChartDataViewModel = new DyeingChartDataViewModel();

            var totalRopeDyeing = await DenimDbContext.F_DYEING_PROCESS_ROPE_MASTER
                .SumAsync(c => c.DYEING_LENGTH ?? 0);

            var totalSlasherDyeing = await DenimDbContext.F_PR_SLASHER_DYEING_DETAILS
                .Include(c => c.SL)
                .SumAsync(c => Convert.ToDouble(c.LENGTH_PER_BEAM ?? 0));

            var totalDyeing = totalRopeDyeing + totalSlasherDyeing;


            var monthlyRopeDyeing = await DenimDbContext.F_DYEING_PROCESS_ROPE_MASTER
                .Where(c => (c.TRNSDATE ?? defaultDate).Month.Equals(date.Month))
                .SumAsync(c => c.DYEING_LENGTH ?? 0);

            var monthlySlasherDyeing = await DenimDbContext.F_PR_SLASHER_DYEING_DETAILS
                .Include(c => c.SL)
                .Where(c => (c.SL.TRNSDATE ?? defaultDate).Month.Equals(date.Month))
                .SumAsync(c => Convert.ToDouble(c.LENGTH_PER_BEAM ?? 0));

            var monthlyDyeing = monthlyRopeDyeing + monthlySlasherDyeing;

            var comparisonMonthlyRopeDyeing = monthlyRopeDyeing - await DenimDbContext.F_DYEING_PROCESS_ROPE_MASTER
                .Where(c => (c.TRNSDATE ?? defaultDate).Month.Equals(date.AddMonths(-1).Month))
                .SumAsync(c => c.DYEING_LENGTH ?? 0);

            var comparisonMonthlySlasherDyeing = monthlySlasherDyeing - await DenimDbContext.F_PR_SLASHER_DYEING_DETAILS
                .Include(c => c.SL)
                .Where(c => (c.SL.TRNSDATE ?? defaultDate).Month.Equals(date.AddMonths(-1).Month))
                .SumAsync(c => Convert.ToDouble(c.LENGTH_PER_BEAM ?? 0));



            var comparisonMonthlyDyeing = monthlyDyeing - comparisonMonthlyRopeDyeing + comparisonMonthlySlasherDyeing;

            //                         await DenimDbContext.F_PR_WARPING_PROCESS_ROPE_MASTER
            //.Where(c => (c.DELIVERY_DATE ?? defaultDate).ToString("yyyy-MM")
            //    .Equals(date.AddMonths(-1).ToString("yyyy-MM")))
            //.Select(d => new F_PR_WARPING_PROCESS_ROPE_MASTER()
            //{
            //    WARP_LENGTH = d.WARP_LENGTH
            //}).SumAsync(c => c.WARP_LENGTH);

            var todayRopeDyeing = await DenimDbContext.F_DYEING_PROCESS_ROPE_MASTER
                .Where(c => (c.TRNSDATE ?? defaultDate).Date.Equals(date.Date))
                .SumAsync(c => c.DYEING_LENGTH ?? 0);


            var todaySlasherDyeing = await DenimDbContext.F_PR_SLASHER_DYEING_DETAILS
                .Include(c => c.SL)
                .Where(c => (c.SL.TRNSDATE ?? defaultDate).Date.Equals(date.Date))
                .SumAsync(c => Convert.ToDouble(c.LENGTH_PER_BEAM ?? 0));

            var todayDyeing = todayRopeDyeing + todaySlasherDyeing;

            dyeingChartDataViewModel.TotalRopeDyeing = totalRopeDyeing;
            dyeingChartDataViewModel.TotalSlasherDyeing = totalSlasherDyeing;
            dyeingChartDataViewModel.TodayRopeDyeing = todayRopeDyeing;
            dyeingChartDataViewModel.TodaySlasherDyeing = todaySlasherDyeing;
            dyeingChartDataViewModel.TodayDyeing = todayDyeing;
            dyeingChartDataViewModel.MonthlyRopeDyeing = monthlyRopeDyeing;
            dyeingChartDataViewModel.MonthlySlasherDyeing = monthlySlasherDyeing;
            dyeingChartDataViewModel.MonthlyDyeing = monthlyDyeing;
            dyeingChartDataViewModel.TotalDyeing = totalDyeing;
            dyeingChartDataViewModel.ComparisonMonthlyRopeDyeing = comparisonMonthlyRopeDyeing;
            dyeingChartDataViewModel.ComparisonMonthlySlasherDyeing = comparisonMonthlySlasherDyeing;
            dyeingChartDataViewModel.ComparisonMonthlyDyeing = comparisonMonthlyDyeing;
            return dyeingChartDataViewModel;
        }

        public async Task<List<PL_PRODUCTION_SETDISTRIBUTION>> GetDyeingPendingSetList()
        {
            var result =
                await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Where(c => !DenimDbContext.F_DYEING_PROCESS_ROPE_DETAILS.Any(e => e.SUBGROUPID.Equals(c.SUBGROUPID)) &&
                                !DenimDbContext.F_PR_SLASHER_DYEING_MASTER.Any(e => e.SETID.Equals(c.SETID)))  
                    .Select(c => new PL_PRODUCTION_SETDISTRIBUTION()
                    {
                        TRNSDATE = c.TRNSDATE,
                        PROG_ = new PL_BULK_PROG_SETUP_D
                        {
                            PROG_NO = c.PROG_.PROG_NO,
                            PROGRAM_TYPE = c.PROG_.PROGRAM_TYPE ?? "N/A"
                        }
                    }).OrderBy(c => c.TRNSDATE).Take(7).ToListAsync();
            return result;
        }
    }
}
