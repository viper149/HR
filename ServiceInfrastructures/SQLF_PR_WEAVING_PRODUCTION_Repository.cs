using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Home;
using DevExpress.DataProcessing.InMemoryDataProcessor;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_WEAVING_PRODUCTION_Repository : BaseRepository<F_PR_WEAVING_PRODUCTION>, IF_PR_WEAVING_PRODUCTION
    {
        public SQLF_PR_WEAVING_PRODUCTION_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<F_PR_WEAVING_PRODUCTION>> GetAllFPrWeavingProductionAsync()
        {
            try
            {
                return await DenimDbContext.F_PR_WEAVING_PRODUCTION
                    .Include(e => e.EMP)
                    .Include(e => e.FABCODENavigation)
                    .Include(e => e.LOOM)
                    .Include(e => e.PO)
                    .OrderByDescending(c => c.WV_PRODID)
                    .Select(d => new F_PR_WEAVING_PRODUCTION()
                    {
                        WV_PRODID = d.WV_PRODID,
                        FABCODENavigation = d.FABCODENavigation != null ? new RND_FABRICINFO
                        {
                            STYLE_NAME = d.FABCODENavigation.STYLE_NAME
                        } : new RND_FABRICINFO(),
                        PO = d.PO == null ? new RND_PRODUCTION_ORDER
                        {
                            SO = new COM_EX_PI_DETAILS()
                        } : new RND_PRODUCTION_ORDER
                        {
                            SO = new COM_EX_PI_DETAILS
                            {
                                SO_NO = d.PO.SO.SO_NO
                            }
                        },
                        EMP = new F_HRD_EMPLOYEE
                        {
                            FIRST_NAME = d.EMP.FIRST_NAME
                        },
                        LOOM = new LOOM_TYPE
                        {
                            LOOM_TYPE_NAME = d.LOOM.LOOM_TYPE_NAME
                        },
                        OPT1 = d.OPT1,
                        OPT2 = d.OPT2,
                        OPT3 = d.OPT3,
                        TOTAL_PROD = d.TOTAL_PROD,
                    }).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FPrWeavingProductionViewModel> GetInitObjByAsync(FPrWeavingProductionViewModel fPrWeavingProductionViewModel)
        {
            fPrWeavingProductionViewModel.EmployeeList = await DenimDbContext.F_HRD_EMPLOYEE
                .Select(d => new F_HRD_EMPLOYEE
                {
                    EMPID = d.EMPID,
                    FIRST_NAME = $"{d.EMPNO} - {d.FIRST_NAME}"
                })
                .ToListAsync();

            fPrWeavingProductionViewModel.LoomTypeList = await DenimDbContext.LOOM_TYPE
                .Select(d => new LOOM_TYPE
                {
                    LOOMID = d.LOOMID,
                    LOOM_TYPE_NAME = d.LOOM_TYPE_NAME
                })
                .ToListAsync();
            fPrWeavingProductionViewModel.SOList = await DenimDbContext.RND_PRODUCTION_ORDER
                .Include(c => c.SO)
                .Include(c => c.RS)
                .Select(d => new RND_PRODUCTION_ORDER
                {
                    POID = d.POID,
                    OPT1 = d.SO != null ? d.SO.SO_NO : d.RS.RSOrder ?? d.RS.DYEINGCODE
                })
                .ToListAsync();
            fPrWeavingProductionViewModel.StyleNameList = await DenimDbContext.RND_FABRICINFO
                .Select(d => new RND_FABRICINFO
                {
                    FABCODE = d.FABCODE,
                    STYLE_NAME = d.STYLE_NAME
                })
                .ToListAsync();

            return fPrWeavingProductionViewModel;
        }

        public async Task<RND_PRODUCTION_ORDER> GetStyleInfoBySo(int id)
        {
            try
            {
                var result = await DenimDbContext.RND_PRODUCTION_ORDER.Include(c => c.SO.STYLE.FABCODENavigation)
                    .FirstOrDefaultAsync(c => c.POID.Equals(id));
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<FPrWeavingProductionViewModel> GetProductionDetailsAsync(
            FPrWeavingProductionViewModel fPrWeavingProductionViewModel)
        {
            try
            {
                foreach (var item in fPrWeavingProductionViewModel.FPrWeavingProductionList)
                {
                    item.FABCODENavigation =
                        await DenimDbContext.RND_FABRICINFO.FirstOrDefaultAsync(c => c.FABCODE.Equals(item.FABCODE));
                    item.PO = await DenimDbContext.RND_PRODUCTION_ORDER.Include(c => c.SO).FirstOrDefaultAsync(c =>
                          c.POID.Equals(item.POID));
                    item.EMP = await DenimDbContext.F_HRD_EMPLOYEE.FirstOrDefaultAsync(c => c.EMPID.Equals(item.EMPID));
                }

                return fPrWeavingProductionViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<DashboardViewModel> GetWeavingDateWiseLengthGraph()
        {
            try
            {
                var dashBoardViewModel = new DashboardViewModel
                {
                    WeavingChartDataViewModel = new WeavingChartDataViewModel
                    {
                        WeavingProductionAirjet = await DenimDbContext.F_PR_WEAVING_PRODUCTION
                            .Where(c => (c.CREATED_AT ?? default).ToString("yyyy-MM-dd").Equals("2022-01-27") && c.LOOMID.Equals(1))
                            .Select(d => d.TOTAL_PROD ?? 0).SumAsync(),
                        WeavingProductionRapier = await DenimDbContext.F_PR_WEAVING_PRODUCTION
                        .Where(c => (c.CREATED_AT ?? default).ToString("yyyy-MM-dd").Equals("2022-01-27") && c.LOOMID.Equals(2)).Select(d => d.TOTAL_PROD ?? 0).SumAsync()
                    }
                };
                return dashBoardViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public async Task<FPrWeavingProductionViewModel> GetWeavingProductionData()
        {
            try
            {
                var date = Common.Common.GetDate();
                var defaultDate = Common.Common.GetDefaultDate();
                var weavingChartData = new FPrWeavingProductionViewModel
                {
                    TotalWeavingProduction = await DenimDbContext.F_PR_WEAVING_PRODUCTION
                        .Select(d => new F_PR_WEAVING_PRODUCTION
                        {
                            TOTAL_PROD = d.TOTAL_PROD ?? 0
                        }).SumAsync(c => c.TOTAL_PROD),

                    TotalAirjetProduction = await DenimDbContext.F_PR_WEAVING_PRODUCTION
                        .Where(c => c.LOOMID.Equals(1))
                        .Select(d => d.TOTAL_PROD ?? 0).SumAsync(),

                    TotalRapierProduction = await DenimDbContext.F_PR_WEAVING_PRODUCTION
                        .Where(c => c.LOOMID.Equals(2)).Select(d => d.TOTAL_PROD ?? 0).SumAsync(),


                    MonthlyTotalWeavingProduction = await DenimDbContext.F_PR_WEAVING_PRODUCTION
                        .Where(c => (c.CREATED_AT ?? defaultDate).Month.Equals(date.Month))
                        .Select(d => new F_PR_WEAVING_PRODUCTION
                        {
                            TOTAL_PROD = d.TOTAL_PROD ?? 0
                        }).SumAsync(c => c.TOTAL_PROD),

                    MonthlyTotalAirjetProduction = await DenimDbContext.F_PR_WEAVING_PRODUCTION
                        .Where(c => c.LOOMID.Equals(1) && (c.CREATED_AT ?? defaultDate).Month.Equals(date.Month))
                        .Select(d => d.TOTAL_PROD ?? 0).SumAsync(),

                    MonthlyTotalRapierProduction = await DenimDbContext.F_PR_WEAVING_PRODUCTION
                        .Where(c => c.LOOMID.Equals(2) && (c.CREATED_AT ?? defaultDate).Month.Equals(date.Month))
                        .Select(d => d.TOTAL_PROD ?? 0).SumAsync(),



                    DailyTotalWeavingProduction = await DenimDbContext.F_PR_WEAVING_PRODUCTION
                        .Where(c => (c.CREATED_AT ?? defaultDate).Date.Equals(date.Date))
                        .Select(d => new F_PR_WEAVING_PRODUCTION
                        {
                            TOTAL_PROD = d.TOTAL_PROD ?? 0
                        }).SumAsync(c => c.TOTAL_PROD),

                    DailyTotalAirjetProduction = await DenimDbContext.F_PR_WEAVING_PRODUCTION
                        .Where(c => c.LOOMID.Equals(1) && (c.CREATED_AT ?? defaultDate).Date.Equals(date.Date))
                        .Select(d => d.TOTAL_PROD ?? 0).SumAsync(),

                    DailyTotalRapierProduction = await DenimDbContext.F_PR_WEAVING_PRODUCTION
                        .Where(c => c.LOOMID.Equals(2) && (c.CREATED_AT ?? defaultDate).Date.Equals(date.Date))
                        .Select(d => d.TOTAL_PROD ?? 0).SumAsync(),

                    ComparisonMonthlyTotalAirjetProduction = await DenimDbContext.F_PR_WEAVING_PRODUCTION
                        .Where(c => c.LOOMID.Equals(1) && (c.CREATED_AT ?? defaultDate).Month.Equals(date.Month))
                        .Select(d => d.TOTAL_PROD ?? 0).SumAsync() - await DenimDbContext.F_PR_WEAVING_PRODUCTION
                        .Where(c => c.LOOMID.Equals(1) && (c.CREATED_AT ?? defaultDate).Month.Equals(date.AddMonths(-1).Month))
                        .Select(d => d.TOTAL_PROD ?? 0).SumAsync(),

                    ComparisonMonthlyTotalRapierProduction = await DenimDbContext.F_PR_WEAVING_PRODUCTION
                        .Where(c => c.LOOMID.Equals(2) && (c.CREATED_AT ?? defaultDate).Month.Equals(date.Month))
                        .Select(d => d.TOTAL_PROD ?? 0).SumAsync() - await DenimDbContext.F_PR_WEAVING_PRODUCTION
                        .Where(c => c.LOOMID.Equals(2) && (c.CREATED_AT ?? defaultDate).Month.Equals(date.AddMonths(-1).Month))
                        .Select(d => d.TOTAL_PROD ?? 0).SumAsync(),


                };


                

                var weavingCompleteSets = await DenimDbContext.F_PR_WEAVING_PROCESS_MASTER_B.CountAsync();

                var fromSizing = await DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER.CountAsync();
                var fromSDyeing = await DenimDbContext.F_PR_SLASHER_DYEING_MASTER.CountAsync();
                var totalSetsforWeaving = fromSizing + fromSDyeing;

                var weavingPendingSets = totalSetsforWeaving - weavingCompleteSets;

                //var weavingPendingSets = await DenimDbContext
                //    .PL_PRODUCTION_SETDISTRIBUTION
                //    .Include(c => c.PROG_)
                //    .Include(c => c.F_PR_SIZING_PROCESS_ROPE_MASTER)
                //    .Include(c => c.SUBGROUP.GROUP)
                //    .Where(c =>
                //        (DenimDbContext.F_PR_SLASHER_DYEING_MASTER.Any(d => d.SETID.Equals(c.SETID)) ||
                //         DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER.Any(e => e.SETID.Equals(c.SETID))) &&
                //        !DenimDbContext.F_PR_WEAVING_PROCESS_MASTER_B.Any(e => e.SETID.Equals(c.SETID)))
                //    .Select(c => new PL_PRODUCTION_SETDISTRIBUTION
                //    {
                //        SETID = c.SETID

                //    }).CountAsync();






                var mWeavingCompleteSets = await DenimDbContext.F_PR_WEAVING_PROCESS_MASTER_B
                    .Where(c => (c.WV_PROCESS_DATE ?? defaultDate).Month.Equals(date.Month))
                    .Select(c => c.SETID).CountAsync();

                var mFromSizing = await DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER
                    .Where(c=>(c.TRNSDATE ?? defaultDate).Month.Equals(date.Month))
                    .Select(c=>c.SETID)
                    .CountAsync();
                var mFromSDyeing = await DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER
                    .Where(c=>(c.TRNSDATE ?? defaultDate).Month.Equals(date.Month))
                    .Select(c=>c.SETID)
                    .CountAsync();

                var mTotalSetsForWeaving = mFromSizing + mFromSDyeing;

                var mWeavingPendingSets = mTotalSetsForWeaving - mWeavingCompleteSets;



                var mCompletePercent = (mWeavingCompleteSets / Convert.ToSingle(mTotalSetsForWeaving)) * 100;
                var mCompletePercentR = Math.Round(mCompletePercent, 1);

                var mPendingPercent = (mWeavingPendingSets / Convert.ToSingle(mTotalSetsForWeaving)) * 100;
                var mPendingPercentR = Math.Round(mPendingPercent, 1);

                //var comparisonMonthlyProduction = sizingChartData.MonthlyProduction - await DenimDbContext
                //    .F_PR_SIZING_PROCESS_ROPE_MASTER
                //    .Where(c => (c.TRNSDATE ?? defaultDate).ToString("yyyy-MM").Equals(date.AddMonths(-1).ToString("yyyy-MM")))
                //    .Select(d => new F_PR_SIZING_PROCESS_ROPE_MASTER
                //    {
                //        LCB_ACT_LENGTH = d.LCB_ACT_LENGTH ?? 0
                //    }).SumAsync(c => c.LCB_ACT_LENGTH);


                weavingChartData.MonthlyCompleteSets = mWeavingCompleteSets;
                weavingChartData.MonthlyPendingSets = mWeavingPendingSets;
                weavingChartData.MonthlyCompletePercent = mCompletePercentR;
                weavingChartData.MonthlyPendingPercent = mPendingPercentR;
                return weavingChartData;
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<FPrWeavingProductionViewModel>> GetWeavingProductionList()
        {
            try

            {
                var data = new List<FPrWeavingProductionViewModel>();
                var date = Convert.ToDateTime("2022-04-26").AddDays(-30);
                var defaultDate = Convert.ToDateTime("1501-01-01 00:00:00.000");


                for (var i = 0; i < 30; i++)
                {
                    data.Add(new FPrWeavingProductionViewModel
                    {
                        date = date.AddDays(i),

                        DailyTotalWeavingProduction = await DenimDbContext.F_PR_WEAVING_PRODUCTION
                            .Where(c => (c.CREATED_AT ?? defaultDate).Date.Equals(date.AddDays(i).Date))
                            .Select(d => new F_PR_WEAVING_PRODUCTION
                            {
                                TOTAL_PROD = d.TOTAL_PROD ?? 0
                            }).SumAsync(c => c.TOTAL_PROD),

                        DailyTotalAirjetProduction = await DenimDbContext.F_PR_WEAVING_PRODUCTION
                            .Where(c => c.LOOMID.Equals(1) && (c.CREATED_AT ?? defaultDate).Date.Equals(date.AddDays(i).Date))
                            .Select(d => d.TOTAL_PROD ?? 0).SumAsync(),

                        DailyTotalRapierProduction = await DenimDbContext.F_PR_WEAVING_PRODUCTION
                            .Where(c => c.LOOMID.Equals(2) && (c.CREATED_AT ?? defaultDate).Date.Equals(date.AddDays(i).Date))
                            .Select(d => d.TOTAL_PROD ?? 0).SumAsync(),

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

        public async Task<List<PL_PRODUCTION_SETDISTRIBUTION>> GetWeavingPendingList()
        {

            //var result = await DenimDbContext
            //    .PL_PRODUCTION_SETDISTRIBUTION
            //    .Include(c => c.PROG_)
            //    .Include(c => c.F_PR_SIZING_PROCESS_ROPE_MASTER)
            //    .Include(c => c.SUBGROUP.GROUP)
            //    .Where(c => DenimDbContext.F_PR_SLASHER_DYEING_MASTER.Any(d => d.SET.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION.Any(e => e.SETID.Equals(c.SETID))) && !DenimDbContext.F_PR_WEAVING_PROCESS_MASTER_B.Any(e => e.SETID.Equals(c.SETID)) ||

            //                (DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER.Any(e => e.SETID.Equals(c.SETID))) && !DenimDbContext.F_PR_WEAVING_PROCESS_MASTER_B.Any(e => e.SETID.Equals(c.SETID)))
            //    .Select(c => new PL_PRODUCTION_SETDISTRIBUTION
            //    {
            //        SETID = c.SETID,
            //        TRNSDATE = c.TRNSDATE,
            //        PROG_ = new PL_BULK_PROG_SETUP_D
            //        {
            //            PROG_NO = c.PROG_.PROG_NO,
            //            PROGRAM_TYPE = c.PROG_.PROGRAM_TYPE ?? "N/A"
            //        }
            //    }).OrderByDescending(c => c.PROG_.PROG_NO).Take(7).ToListAsync();


             var result = await DenimDbContext
                .PL_PRODUCTION_SETDISTRIBUTION
                .Include(c => c.PROG_)
                .Include(c => c.F_PR_SIZING_PROCESS_ROPE_MASTER)
                .Include(c => c.SUBGROUP.GROUP)
                .Where(c => (DenimDbContext.F_PR_SLASHER_DYEING_MASTER.Any(d => d.SETID.Equals(c.SETID)) || DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER.Any(e => e.SETID.Equals(c.SETID))) && !DenimDbContext.F_PR_WEAVING_PROCESS_MASTER_B.Any(e => e.SETID.Equals(c.SETID)))
                .Select(c => new PL_PRODUCTION_SETDISTRIBUTION
                {
                    SETID = c.SETID,
                    TRNSDATE = c.TRNSDATE,
                    PROG_ = new PL_BULK_PROG_SETUP_D
                    {
                        PROG_NO = c.PROG_.PROG_NO,
                        PROGRAM_TYPE = c.PROG_.PROGRAM_TYPE ?? "N/A"
                    }
                }).OrderByDescending(c => c.PROG_.PROG_NO).Take(7).ToListAsync();

            return result;

        }

        //try
        //{
        //    var dashBoardViewModel = new DashboardViewModel();
        //    {
        //        var weavingChartDataViewModel = new WeavingChartDataViewModel();

        //weavingChartDataViewModel.WeavingProductionRapier = await _denimDbContext.F_PR_WEAVING_PRODUCTION
        //    .Where(c => (c.CREATED_AT ?? default).ToString("yyyy-MM-dd").Equals("2022-01-27") && c.LOOMID.Equals(1)).Select(d => d.TOTAL_PROD ?? 0)
        //            .SumAsync();

        //        weavingChartDataViewModel.WeavingProductionAirjet = await _denimDbContext.F_PR_WEAVING_PRODUCTION
        //            .Where(c => (c.CREATED_AT ?? default).ToString("yyyy-MM-dd").Equals("2022-01-27") && c.LOOMID.Equals(2)).Select(d => d.TOTAL_PROD ?? 0)
        //            .SumAsync();
        //    }


        //    return dashBoardViewModel;
        //}
        //catch (Exception e)
        //{
        //    Console.WriteLine(e);
        //    throw;
        //}
        //        }
    }
}
