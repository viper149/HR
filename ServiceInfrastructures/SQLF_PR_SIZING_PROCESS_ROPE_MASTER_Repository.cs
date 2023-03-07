using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Factory.Production;
using DenimERP.ViewModels.Rnd;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_SIZING_PROCESS_ROPE_MASTER_Repository : BaseRepository<F_PR_SIZING_PROCESS_ROPE_MASTER>, IF_PR_SIZING_PROCESS_ROPE_MASTER
    {
        public SQLF_PR_SIZING_PROCESS_ROPE_MASTER_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<F_PR_SIZING_PROCESS_ROPE_MASTER>> GetAllAsync()
        {
            try
            {
                var result = await DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER
                    .Include(c => c.SET.PROG_)
                    .Include(c => c.F_PR_SIZING_PROCESS_ROPE_CHEM)
                    .ToListAsync();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FSizingProductionRopeViewModel> GetInitObjects(FSizingProductionRopeViewModel fSizingProductionRopeViewModel)
        {
            try
            {
                fSizingProductionRopeViewModel.FSizingMachines = await DenimDbContext.F_SIZING_MACHINE
                    .Select(c => new F_SIZING_MACHINE
                    {
                        ID = c.ID,
                        MACHINE_NO = c.MACHINE_NO
                    }).ToListAsync();

                fSizingProductionRopeViewModel.FChemStoreProductInfos =
                    await DenimDbContext.F_CHEM_STORE_PRODUCTINFO.Select(c => new F_CHEM_STORE_PRODUCTINFO
                    {
                        PRODUCTID = c.PRODUCTID,
                        PRODUCTNAME = c.PRODUCTNAME
                    }).ToListAsync();

                fSizingProductionRopeViewModel.FWeavingBeams = await DenimDbContext.F_WEAVING_BEAM
                    .Select(c => new F_WEAVING_BEAM
                    {
                        ID = c.ID,
                        BEAM_NO = c.BEAM_NO
                    }).ToListAsync();


                var FHrEmployees = await DenimDbContext.F_HR_EMP_OFFICIALINFO
                    .Include(c => c.EMP)
                    .Where(c => c.SECID.Equals(160) && !c.OPN2.Equals("Y"))
                    .ToListAsync();

                fSizingProductionRopeViewModel.FHrEmployees = FHrEmployees.Select(c => new F_HRD_EMPLOYEE
                {
                    EMPID = c.EMP.EMPID,
                    FIRST_NAME = c.EMP.FIRST_NAME + " " + c.EMP.LAST_NAME + '-' + c.EMP.EMPNO
                }).ToList();

                //fSizingProductionRopeViewModel.PlProductionPlanMasters = await _denimDbContext.PL_PRODUCTION_PLAN_MASTER
                //    .Select(c => new PL_PRODUCTION_PLAN_MASTER
                //    {
                //        GROUPID = c.GROUPID,
                //        GROUP_NO = c.GROUP_NO
                //    }).ToListAsync();

                //fSizingProductionRopeViewModel.PlProductionSetDistributions = await _denimDbContext
                //    .PL_PRODUCTION_SETDISTRIBUTION
                //    .Include(c => c.PROG_)
                //    .Select(c => new PL_PRODUCTION_SETDISTRIBUTION
                //    {
                //        SETID = c.SETID,
                //        PROG_ = c.PROG_
                //    }).ToListAsync();


                fSizingProductionRopeViewModel.PlProductionSetDistributions = await DenimDbContext
                    .PL_PRODUCTION_SETDISTRIBUTION
                    .Include(c => c.PROG_)
                    .Include(c => c.SUBGROUP.GROUP)
                    .Where(c => DenimDbContext.F_LCB_PRODUCTION_ROPE_MASTER.Any(e => e.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION.Any(d => d.SETID.Equals(c.SETID))) && c.PROG_.YARN_TYPE != 4 && !DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER.Any(e => e.SETID.Equals(c.SETID)) || (DenimDbContext.F_PR_WARPING_PROCESS_ECRU_MASTER.Any(e => e.SETID.Equals(c.SETID)) && c.SUBGROUP.GROUP.DYEING_TYPE == 2004) && !DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER.Any(e => e.SETID.Equals(c.SETID)))
                    .Select(c => new PL_PRODUCTION_SETDISTRIBUTION
                    {
                        SETID = c.SETID,
                        PROG_ = new PL_BULK_PROG_SETUP_D
                        {
                            PROG_NO = $"{c.SUBGROUP.SUBGROUPNO} - {c.PROG_.PROG_NO}"
                        }
                    }).ToListAsync();

                fSizingProductionRopeViewModel.PlProductionSetDistributionsForEdit = await DenimDbContext
                    .PL_PRODUCTION_SETDISTRIBUTION
                    .Include(c => c.PROG_)
                    .Include(c => c.SUBGROUP.GROUP)
                    .Where(c => DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER.Any(e => e.SETID.Equals(c.SETID)))
                    .Select(c => new PL_PRODUCTION_SETDISTRIBUTION
                    {
                        SETID = c.SETID,
                        PROG_ = new PL_BULK_PROG_SETUP_D
                        {
                            PROG_NO = $"{c.SUBGROUP.SUBGROUPNO} - {c.PROG_.PROG_NO}"
                        }
                    }).ToListAsync();

                return fSizingProductionRopeViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<IEnumerable<F_PR_SIZING_PROCESS_ROPE_CHEM>> GetInitChemData(List<F_PR_SIZING_PROCESS_ROPE_CHEM> fPrSizingProcessRopeChems)
        {
            try
            {
                foreach (var item in fPrSizingProcessRopeChems)
                {
                    item.CHEM_PROD = await DenimDbContext.F_CHEM_STORE_PRODUCTINFO
                        .Include(c => c.F_PR_SIZING_PROCESS_ROPE_CHEM)
                        .Where(c => c.PRODUCTID.Equals(item.CHEM_PRODID)).FirstOrDefaultAsync();
                }
                return fPrSizingProcessRopeChems;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<F_PR_SIZING_PROCESS_ROPE_DETAILS>> GetInitBeamData(List<F_PR_SIZING_PROCESS_ROPE_DETAILS> fPrSizingProcessRopeDetails)
        {
            try
            {
                foreach (var item in fPrSizingProcessRopeDetails)
                {
                    item.EMP = await DenimDbContext.F_HRD_EMPLOYEE
                        .Where(c => c.EMPID.Equals(item.EMPID))
                        .Select(c => new F_HRD_EMPLOYEE
                        {
                            EMPID = c.EMPID,
                            FIRST_NAME = c.FIRST_NAME
                        })
                        .FirstOrDefaultAsync();

                    item.S_M = await DenimDbContext.F_SIZING_MACHINE
                        .Where(c => c.ID.Equals(item.S_MID)).FirstOrDefaultAsync();

                    item.W_BEAM = await DenimDbContext.F_WEAVING_BEAM
                        .Where(c => c.ID.Equals(item.W_BEAMID)).FirstOrDefaultAsync();
                }
                return fPrSizingProcessRopeDetails;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> InsertAndGetIdAsync(F_PR_SIZING_PROCESS_ROPE_MASTER fPrSizingProcessRopeMaster)
        {
            try
            {
                await DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER.AddAsync(fPrSizingProcessRopeMaster);
                await SaveChanges();
                return fPrSizingProcessRopeMaster.SID;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }


        public async Task<FSizingProductionRopeViewModel> FindAllByIdAsync(int sId)
        {
            try
            {
                var result = await DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER
                    .Include(c => c.SET.PROG_)
                    .Include(c => c.F_PR_SIZING_PROCESS_ROPE_CHEM)
                    .ThenInclude(c => c.CHEM_PROD)
                    .Include(c => c.F_PR_SIZING_PROCESS_ROPE_DETAILS)
                    .ThenInclude(c => c.EMP)
                    .Include(c => c.F_PR_SIZING_PROCESS_ROPE_DETAILS)
                    .ThenInclude(c => c.S_M)
                    .Include(c => c.F_PR_SIZING_PROCESS_ROPE_DETAILS)
                    .ThenInclude(c => c.W_BEAM)
                    .Where(c => c.SID.Equals(sId))
                    .FirstOrDefaultAsync();

                var fSizingProductionRopeViewModel = new FSizingProductionRopeViewModel
                {
                    FPrSizingProcessRopeMaster = result,
                    FPrSizingProcessRopeChemList = result.F_PR_SIZING_PROCESS_ROPE_CHEM.ToList(),
                    FPrSizingProcessRopeDetailsList = result.F_PR_SIZING_PROCESS_ROPE_DETAILS.ToList()
                };

                return fSizingProductionRopeViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<RndProductionOrderDetailViewModel> GetSetDetails(int setId)
        {
            try
            {
                var result = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Include(c => c.SUBGROUP)
                    .ThenInclude(c => c.F_LCB_PRODUCTION_ROPE_MASTER)

                    .Include(c => c.SUBGROUP)
                    .ThenInclude(c => c.F_PR_WARPING_PROCESS_ROPE_MASTER)

                    .Include(c => c.F_PR_WARPING_PROCESS_ROPE_DETAILS)
                    .Include(c => c.F_PR_WEAVING_BEAM_RECEIVING)

                    .Include(c => c.F_PR_SIZING_PROCESS_ROPE_MASTER)
                    .ThenInclude(c => c.F_PR_SIZING_PROCESS_ROPE_DETAILS)
                    .ThenInclude(c => c.S_M)

                    .Include(c => c.F_PR_SIZING_PROCESS_ROPE_MASTER)
                    .ThenInclude(c => c.F_PR_SIZING_PROCESS_ROPE_DETAILS)
                    .ThenInclude(c => c.W_BEAM)

                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.WV)
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation)
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.LOOM)


                    //.Include(c=>c.F_LCB_PRODUCTION_ROPE_MASTER)

                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c => c.LOT)

                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c => c.SUPP)

                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c => c.COUNT)

                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_YARNCONSUMPTION)

                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER)

                    .Select(c => new RndProductionOrderDetailViewModel
                    {
                        ComExPiDetails = c.PROG_.BLK_PROG_.RndProductionOrder.SO,
                        PlProductionSetDistribution = c,
                        RndFabricCountInfoViewModels = c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO
                            .Select(e => new RndFabricCountInfoViewModel
                            {
                                RndFabricCountinfo = e,
                                AMOUNT = c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(d => d.FABCODE.Equals(e.FABCODE) && d.COUNTID.Equals(e.COUNTID)).AMOUNT
                            }).ToList()
                    })
                    .FirstOrDefaultAsync(c => c.PlProductionSetDistribution.SETID.Equals(setId));


                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<FSizingProductionRopeViewModel>> GetSizingDateWiseLengthGraph()
        {
            try

            {
                var data = new List<FSizingProductionRopeViewModel>();
                var date = Convert.ToDateTime("2022-05-09");

                for (var i = 0; i < 15; i++)
                {
                    data.Add(new FSizingProductionRopeViewModel
                    {
                        TotalSizing = await DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER
                            .Where(c => c.TRNSDATE.Equals(date.AddDays(i).Date))
                            .Select(d => new F_PR_SIZING_PROCESS_ROPE_MASTER()
                            {
                                SIZING_ENDS = d.SIZING_ENDS
                            }).SumAsync(c => c.SIZING_ENDS)
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

        public async Task<FSizingProductionRopeViewModel> GetSizingDataDayMonthAsync()
        {
            {
                var date = Convert.ToDateTime("2022-02-27");
                var defaultDate = Convert.ToDateTime("1501-01-01 00:00:00.000");

                return new FSizingProductionRopeViewModel
                {
                    TodaySizing = await DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER
                        .Where(c => (c.TRNSDATE ?? defaultDate).Equals(date))
                        .Select(d => new F_PR_SIZING_PROCESS_ROPE_MASTER
                        {
                            SIZING_ENDS = d.SIZING_ENDS
                        }).SumAsync(c => c.SIZING_ENDS),

                    MonthlySizing = await DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER
                        .Where(c => (c.TRNSDATE ?? defaultDate).ToString("yyyy-MM").Equals(date.ToString("yyyy-MM")))
                        .Select(d => new F_PR_SIZING_PROCESS_ROPE_MASTER
                        {
                            SIZING_ENDS = d.SIZING_ENDS
                        }).SumAsync(c => c.SIZING_ENDS)
                };
            }
        }

        public async Task<FSizingProductionRopeViewModel> GetSizingProductionData()
        {
            try
            {
                var date = Common.Common.GetDate();
                var defaultDate = Common.Common.GetDefaultDate();
                var sizingChartData = new FSizingProductionRopeViewModel
                {
                    TotalProduction = await DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER
                        .Select(d => new F_PR_SIZING_PROCESS_ROPE_MASTER
                        {
                            LCB_ACT_LENGTH = d.LCB_ACT_LENGTH ?? 0
                        }).SumAsync(c => c.LCB_ACT_LENGTH),
                    MonthlyProduction = await DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER
                        .Where(c => (c.TRNSDATE ?? defaultDate).ToString("yyyy-MM").Equals(date.ToString("yyyy-MM")))
                        .Select(d => new F_PR_SIZING_PROCESS_ROPE_MASTER
                        {
                            LCB_ACT_LENGTH = d.LCB_ACT_LENGTH ?? 0
                        }).SumAsync(c => c.LCB_ACT_LENGTH),
                    DailyProduction = await DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER
                        .Where(c => (c.TRNSDATE ?? defaultDate).Date.Equals(date.Date))
                        .Select(d => new F_PR_SIZING_PROCESS_ROPE_MASTER
                        {
                            LCB_ACT_LENGTH = d.LCB_ACT_LENGTH ?? 0
                        }).SumAsync(c => c.LCB_ACT_LENGTH)
                };


                var LcbCompleteSets = await DenimDbContext.F_LCB_PRODUCTION_ROPE_MASTER
                    .Include(c => c.SUBGROUP)
                    .ThenInclude(d => d.PL_PRODUCTION_SETDISTRIBUTION)
                    .Select(c => c.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION.Select(d => d.SETID)).SelectMany(setIds => setIds).Distinct().CountAsync();

                var SizingCompleteLcb = await DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER
                    .Include(c => c.SET)
                    .ThenInclude(c => c.PROG_)
                    .Where(c => c.SET.PROG_.PROCESS_TYPE.Equals("ROPE"))
                    .Distinct().CountAsync();

                var SizingPendingLcb = LcbCompleteSets - SizingCompleteLcb;

                var EcruWarpingCompleteSets = await DenimDbContext.F_PR_WARPING_PROCESS_ECRU_MASTER.Distinct().CountAsync();

                var SizingCompleteEcru = await DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER
                    .Include(c => c.SET)
                    .ThenInclude(c => c.PROG_)
                    .Where(c => c.SET.PROG_.PROCESS_TYPE.Equals("ECRU-SIZING"))
                    .Distinct().CountAsync();
                var SizingPendingEcru = EcruWarpingCompleteSets - SizingCompleteEcru;

                var sizingPendingSets = SizingPendingLcb + SizingPendingEcru;
                var sizingCompleteSets = await DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER.Distinct().CountAsync();



                var mLcbCompleteSets = await DenimDbContext.F_LCB_PRODUCTION_ROPE_MASTER
                    .Include(c => c.SUBGROUP)
                    .ThenInclude(d => d.PL_PRODUCTION_SETDISTRIBUTION)
                    .Where(c => (c.TRANSDATE ?? defaultDate).Month.Equals(date.Month))
                    .Select(c => c.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION.Select(d => d.SETID)).SelectMany(setIds => setIds).Distinct().CountAsync();

                var mSizingCompleteLcb = await DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER
                    .Include(c => c.SET)
                    .ThenInclude(c => c.PROG_)
                    .Where(c => c.SET.PROG_.PROCESS_TYPE.Equals("ROPE") && (c.TRNSDATE ?? defaultDate).Month.Equals(date.Month))
                    .Distinct().CountAsync();

                var mSizingPendingLcb = mLcbCompleteSets - mSizingCompleteLcb;

                var mEcruWarpingCompleteSets = await DenimDbContext.F_PR_WARPING_PROCESS_ECRU_MASTER
                    .Where(c => (c.TIME_START ?? defaultDate).Month.Equals(date.Month))
                    .Distinct().CountAsync();


                var mSizingCompleteEcru = await DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER
                    .Include(c => c.SET)
                    .ThenInclude(c => c.PROG_)
                    .Where(c => c.SET.PROG_.PROCESS_TYPE.Equals("ECRU-SIZING") && (c.TRNSDATE ?? defaultDate).Month.Equals(date.Month))
                    .Distinct().CountAsync();

                var mSizingPendingEcru = mEcruWarpingCompleteSets - mSizingCompleteEcru;

                var mSizingPendingSets = mSizingPendingLcb + mSizingPendingEcru;
                var mTotalSetsForSizing = mLcbCompleteSets + mEcruWarpingCompleteSets;
                var mSizingCompleteSets = await DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER
                    .Where(c => (c.TRNSDATE ?? defaultDate).Month.Equals(date.Month))
                    .Distinct().CountAsync();

                var mCompletePercent = (mSizingCompleteSets / Convert.ToSingle(mTotalSetsForSizing)) * 100;
                var mCompletePercentR = Math.Round(mCompletePercent, 1);

                var mPendingPercent = (mSizingPendingSets / Convert.ToSingle(mTotalSetsForSizing)) * 100;
                var mPendingPercentR = Math.Round(mPendingPercent, 1);

                var comparisonMonthlyProduction = sizingChartData.MonthlyProduction - await DenimDbContext
                    .F_PR_SIZING_PROCESS_ROPE_MASTER
                    .Where(c => (c.TRNSDATE ?? defaultDate).ToString("yyyy-MM").Equals(date.AddMonths(-1).ToString("yyyy-MM")))
                    .Select(d => new F_PR_SIZING_PROCESS_ROPE_MASTER
                    {
                        LCB_ACT_LENGTH = d.LCB_ACT_LENGTH ?? 0
                    }).SumAsync(c => c.LCB_ACT_LENGTH);

                sizingChartData.CompleteSets = sizingCompleteSets;
                sizingChartData.PendingSets = sizingPendingSets;
                sizingChartData.MonthlyCompleteSets = mSizingCompleteSets;
                sizingChartData.MonthlyPendingSets = mSizingPendingSets;
                sizingChartData.MonthlyCompletePercent = mCompletePercentR;
                sizingChartData.MonthlyPendingPercent = mPendingPercentR;
                sizingChartData.ComparisonMonthlyProduction = comparisonMonthlyProduction;



                return sizingChartData;
                ;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<FSizingProductionRopeViewModel>> GetSizingProductionList()
        {
            try

            {
                var data = new List<FSizingProductionRopeViewModel>();
                var date = Convert.ToDateTime("2022-04-26").AddDays(-30);
                var defaultDate = Convert.ToDateTime("1501-01-01 00:00:00.000");


                for (var i = 0; i < 30; i++)
                {
                    data.Add(new FSizingProductionRopeViewModel
                    {
                        date = date.AddDays(i),

                        TotalProduction = await DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER
                            .Where(c => (c.TRNSDATE ?? defaultDate).Date.Equals(date.AddDays(i).Date))
                            .Select(d => new F_PR_SIZING_PROCESS_ROPE_MASTER
                            {
                                LCB_ACT_LENGTH = d.LCB_ACT_LENGTH ?? 0
                            }).SumAsync(c => c.LCB_ACT_LENGTH)


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

        public async Task<List<PL_PRODUCTION_SETDISTRIBUTION>> GetSizingPendingSetList()
        {

            var result = await DenimDbContext
                .PL_PRODUCTION_SETDISTRIBUTION
                .Include(c => c.PROG_)
                .Include(c => c.F_PR_SIZING_PROCESS_ROPE_MASTER)
                .Include(c => c.SUBGROUP.GROUP)
                .Where(c => DenimDbContext.F_LCB_PRODUCTION_ROPE_MASTER.Any(e => e.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION.Any(d => d.SETID.Equals(c.SETID))) && c.PROG_.PROCESS_TYPE != "SLASHER" && !DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER.Any(e => e.SETID.Equals(c.SETID)) || (DenimDbContext.F_PR_WARPING_PROCESS_ECRU_MASTER.Any(e => e.SETID.Equals(c.SETID))) && !DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER.Any(e => e.SETID.Equals(c.SETID)))
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

    }
}
