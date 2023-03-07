using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Factory.Production;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_WARPING_PROCESS_ROPE_MASTER_Repository : BaseRepository<F_PR_WARPING_PROCESS_ROPE_MASTER>,
        IF_PR_WARPING_PROCESS_ROPE_MASTER
    {
        public SQLF_PR_WARPING_PROCESS_ROPE_MASTER_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<PrWarpingProcessRopeViewModel> GetInitObjects(
            PrWarpingProcessRopeViewModel prWarpingProcessRopeViewModel)
        {
            try
            {
                prWarpingProcessRopeViewModel.BasYarnCountInfos = await DenimDbContext.BAS_YARN_COUNTINFO
                    .Select(c => new BAS_YARN_COUNTINFO()
                    {
                        COUNTID = c.COUNTID,
                        COUNTNAME = c.COUNTNAME
                    }).ToListAsync();

                if (prWarpingProcessRopeViewModel.FPrWarpingProcessRopeMaster == null)
                {
                    prWarpingProcessRopeViewModel.PlProductionPlanDetailsList = await DenimDbContext
                        .PL_PRODUCTION_PLAN_DETAILS
                        .Include(c => c.GROUP.RND_DYEING_TYPE)
                        .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                        .ThenInclude(c => c.PROG_.PROG_NO)
                        .Where(c => c.GROUP.RND_DYEING_TYPE.DTYPE.Equals("Rope") &&
                                    !DenimDbContext.F_PR_WARPING_PROCESS_ROPE_MASTER.Any(e =>
                                        e.SUBGROUPID.Equals(c.SUBGROUPID)))
                        .Select(c => new PL_PRODUCTION_PLAN_DETAILS()
                        {
                            SUBGROUPID = c.SUBGROUPID,
                            SUBGROUPNO = c.SUBGROUPNO,
                            OPT1 = $"{c.SUBGROUPNO} - {c.PL_PRODUCTION_SETDISTRIBUTION.FirstOrDefault().PROG_.PROG_NO}"
                        }).ToListAsync();
                }
                else
                {
                    prWarpingProcessRopeViewModel.PlProductionPlanDetailsList = await DenimDbContext
                        .PL_PRODUCTION_PLAN_DETAILS
                        .Include(c => c.GROUP.RND_DYEING_TYPE)
                        .Where(c => c.GROUP.RND_DYEING_TYPE.DTYPE.Equals("Rope"))
                        .Select(c => new PL_PRODUCTION_PLAN_DETAILS()
                        {
                            SUBGROUPID = c.SUBGROUPID,
                            SUBGROUPNO = c.SUBGROUPNO,
                            OPT1 = $"{c.SUBGROUPNO}"
                        }).ToListAsync();
                }

                prWarpingProcessRopeViewModel.PlProductionSetDistributions = await DenimDbContext
                    .PL_PRODUCTION_SETDISTRIBUTION
                    .Include(c => c.PROG_)
                    .Select(c => new PL_PRODUCTION_SETDISTRIBUTION()
                    {
                        SETID = c.SETID,
                        PROG_ = c.PROG_
                    }).ToListAsync();



                var FHrEmployees = await DenimDbContext.F_HR_EMP_OFFICIALINFO
                    .Include(c => c.EMP)
                    .Where(c => c.SECID.Equals(161) && !c.OPN2.Equals("Y"))
                    .ToListAsync();

                prWarpingProcessRopeViewModel.FHrEmployees = FHrEmployees.Select(c => new F_HRD_EMPLOYEE
                {
                    EMPID = c.EMP.EMPID,
                    FIRST_NAME = c.EMP.FIRST_NAME + " " + c.EMP.LAST_NAME + '-' + c.EMP.EMPNO
                }).ToList();

                prWarpingProcessRopeViewModel.BasBallInfos = await DenimDbContext.F_BAS_BALL_INFO
                    .Where(c => c.FOR_.Equals("Rope"))
                    .Select(c => new F_BAS_BALL_INFO()
                    {
                        BALLID = c.BALLID,
                        BALL_NO = c.BALL_NO
                    }).ToListAsync();

                prWarpingProcessRopeViewModel.FPrWarpingMachines = await DenimDbContext.F_PR_WARPING_MACHINE
                    .Where(c => c.TYPE.Equals("ROPE"))
                    .ToListAsync();

                return prWarpingProcessRopeViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<int> InsertAndGetIdAsync(F_PR_WARPING_PROCESS_ROPE_MASTER prWarpingProcessRopeMaster)
        {
            try
            {
                await DenimDbContext.F_PR_WARPING_PROCESS_ROPE_MASTER.AddAsync(prWarpingProcessRopeMaster);
                await SaveChanges();
                return prWarpingProcessRopeMaster.WARPID;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<F_PR_WARPING_PROCESS_ROPE_MASTER>> GetAllWithNameAsync()
        {
            try
            {
                var result = await DenimDbContext.F_PR_WARPING_PROCESS_ROPE_MASTER
                    .Include(c => c.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c =>
                        c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation)
                    .Include(c => c.SUBGROUP.GROUP.F_DYEING_PROCESS_ROPE_MASTER)
                    .ThenInclude(c => c.F_DYEING_PROCESS_ROPE_DETAILS)
                    .Include(c => c.SUBGROUP.GROUP)
                    .Select(c => new F_PR_WARPING_PROCESS_ROPE_MASTER
                    {
                        WARPID = c.WARPID,
                        TIME_START = c.TIME_START,
                        TIME_END = c.TIME_END,
                        SUBGROUPID = c.SUBGROUPID,
                        WARP_LENGTH = c.WARP_LENGTH,
                        DELIVERY_DATE = c.DELIVERY_DATE,
                        IS_DECLARE = c.IS_DECLARE,
                        CREATED_AT = c.CREATED_AT,
                        REMARKS = c.REMARKS,
                        CREATED_BY = c.CREATED_BY,
                        UPDATED_AT = c.UPDATED_AT,
                        UPDATED_BY = c.UPDATED_BY,
                        SUBGROUP = c.SUBGROUP,
                        OPT1 = c.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION.Select(e =>
                                e.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation
                                    .COLOR)
                            .FirstOrDefault(),
                        OPT2 = c.SUBGROUP.GROUP.F_DYEING_PROCESS_ROPE_MASTER.Count == 0 &&
                               c.SUBGROUP.GROUP.F_DYEING_PROCESS_ROPE_MASTER.All(e =>
                                   e.F_DYEING_PROCESS_ROPE_DETAILS.All(f => f.CLOSE_STATUS))
                            ? "No"
                            : "Yes",
                        OPT3 = c.SUBGROUP.GROUP.GROUP_NO.ToString(),
                        OPT4 =
                            $"{c.SUBGROUP.SUBGROUPNO} - {c.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION.FirstOrDefault().PROG_.PROG_NO}"
                    })
                    .ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<F_PR_WARPING_PROCESS_ROPE_MASTER>> GetAllPendingWithNameAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<PrWarpingProcessRopeDataViewModel> GetDataBySubGroupIdAsync(string subGroup)
        {
            var set = await DenimDbContext.PL_PRODUCTION_PLAN_DETAILS
                .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                .ThenInclude(c => c.PROG_.PL_BULK_PROG_YARN_D)
                .ThenInclude(c => c.COUNT.COUNT)
                .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                .ThenInclude(c => c.PROG_.PL_BULK_PROG_YARN_D)
                .ThenInclude(c => c.LOT)
                .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                .ThenInclude(c => c.COUNT)
                .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation)
                .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.LOOM)
                .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER)
                .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.OTYPE)
                .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                .ThenInclude(c => c.PROG_.YARNFOR)
                .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.RS.RND_SAMPLE_INFO_DETAILS)
                .ThenInclude(c => c.COUNT)
                .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.RS.COLOR)
                .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.RS.D)
                .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.RS.LOOM)
                .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.RS.SDRF.BUYER)
                .Where(c => c.SUBGROUPID.Equals(int.Parse(subGroup)))
                .Select(e => new PrWarpingProcessRopeDataViewModel
                {
                    PL_PRODUCTION_PLAN_DETAILS = new PL_PRODUCTION_PLAN_DETAILS
                    {
                        RATIO = e.RATIO,
                        PL_PRODUCTION_SETDISTRIBUTION = e.PL_PRODUCTION_SETDISTRIBUTION.Select(f =>
                            new PL_PRODUCTION_SETDISTRIBUTION
                            {
                                PROG_ = new PL_BULK_PROG_SETUP_D
                                {
                                    PROG_ID = f.PROG_.PROG_ID,
                                    PROG_NO = f.PROG_.PROG_NO,
                                    SET_QTY = f.PROG_.SET_QTY,
                                    WARP_TYPE = f.PROG_.WARP_TYPE,
                                    PROCESS_TYPE = f.PROG_.PROCESS_TYPE,
                                    PROGRAM_TYPE = f.PROG_.PROGRAM_TYPE,
                                    BLK_PROG_ = new PL_BULK_PROG_SETUP_M
                                    {
                                        RndProductionOrder = new RND_PRODUCTION_ORDER
                                        {
                                            RSNO = f.PROG_.BLK_PROG_.RndProductionOrder.RSNO,
                                            ORDER_QTY_MTR = f.PROG_.BLK_PROG_.RndProductionOrder.ORDER_QTY_MTR,
                                            OTYPE = new RND_ORDER_TYPE
                                            {
                                                OTYPENAME = $"{f.PROG_.BLK_PROG_.RndProductionOrder.OTYPE.OTYPENAME}"
                                            },
                                            SO = new COM_EX_PI_DETAILS
                                            {
                                                SO_NO = $"{f.PROG_.BLK_PROG_.RndProductionOrder.SO.SO_NO}",
                                                STYLE = new COM_EX_FABSTYLE
                                                {
                                                    FABCODENavigation = new RND_FABRICINFO
                                                    {
                                                        TOTALENDS = f.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE
                                                            .FABCODENavigation.TOTALENDS,
                                                        STYLE_NAME = f.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE
                                                            .FABCODENavigation.STYLE_NAME,
                                                        COLORCODENavigation = new BAS_COLOR
                                                        {
                                                            COLOR =
                                                                $"{f.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation.COLOR}"
                                                        },
                                                        LOOM = new LOOM_TYPE
                                                        {
                                                            LOOM_TYPE_NAME =
                                                                $"{f.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.LOOM.LOOM_TYPE_NAME}"
                                                        },
                                                        RND_FABRIC_COUNTINFO = f.PROG_.BLK_PROG_.RndProductionOrder.SO
                                                            .STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO.Select(g =>
                                                                new RND_FABRIC_COUNTINFO
                                                                {
                                                                    YARNFOR = g.YARNFOR,
                                                                    COUNT = new BAS_YARN_COUNTINFO
                                                                    {
                                                                        COUNTID = g.COUNT != null ? g.COUNT.COUNTID : 0,
                                                                        COUNTNAME = $"{g.COUNT.COUNTNAME}"
                                                                    }
                                                                }).ToList()
                                                    }
                                                },
                                                PIMASTER = new COM_EX_PIMASTER
                                                {
                                                    PINO = $"{f.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.PINO}",
                                                    BUYER = new BAS_BUYERINFO
                                                    {
                                                        BUYER_NAME =
                                                            $"{f.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER.BUYER_NAME}"
                                                    }
                                                }
                                            },
                                            RS = new RND_SAMPLE_INFO_DYEING()
                                            {
                                                RSOrder = $"{f.PROG_.BLK_PROG_.RndProductionOrder.RS.RSOrder}",
                                                TOTAL_ENDS = f.PROG_.BLK_PROG_.RndProductionOrder.RS.TOTAL_ENDS,
                                                LOOM = new LOOM_TYPE
                                                {
                                                    LOOM_TYPE_NAME =
                                                        $"{f.PROG_.BLK_PROG_.RndProductionOrder.RS.LOOM.LOOM_TYPE_NAME}"
                                                },
                                                RND_SAMPLE_INFO_DETAILS = f.PROG_.BLK_PROG_.RndProductionOrder.RS
                                                    .RND_SAMPLE_INFO_DETAILS.Select(g => new RND_SAMPLE_INFO_DETAILS
                                                    {
                                                        YARNTYPE = g.YARNTYPE,
                                                        COUNT = new BAS_YARN_COUNTINFO
                                                        {
                                                            COUNTID = g.COUNT != null ? g.COUNT.COUNTID : 0,
                                                            COUNTNAME = $"{g.COUNT.COUNTNAME}"
                                                        }
                                                    }).ToList(),
                                                SDRF = new MKT_SDRF_INFO()
                                                {
                                                    SDRF_NO =
                                                        $"{(f.PROG_.BLK_PROG_.RndProductionOrder.RS.SDRF == null ? "" : f.PROG_.BLK_PROG_.RndProductionOrder.RS.SDRF.SDRF_NO)}",
                                                    BUYER = new BAS_BUYERINFO
                                                    {
                                                        BUYER_NAME =
                                                            $"{f.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER.BUYER_NAME}"
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    PL_BULK_PROG_YARN_D = f.PROG_.PL_BULK_PROG_YARN_D.Select(g =>
                                        new PL_BULK_PROG_YARN_D
                                        {
                                            LOT = new BAS_YARN_LOTINFO
                                            {
                                                LOTNO = $"{g.LOT.LOTNO}",
                                                BRAND = $"{g.LOT.BRAND}"
                                            }
                                        }).ToList(),
                                    YARNFOR = new YARNFOR
                                    {
                                        YARNNAME = $"{f.PROG_.YARNFOR.YARNNAME}"
                                    }
                                }
                            }).ToList()
                    }
                }).FirstOrDefaultAsync();

            return set;
        }

        public async Task<PrWarpingProcessRopeDataViewModel> GetDataBySetIdAsync(string setId)
        {
            try
            {
                var result = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.OTYPE)
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.WV)
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation)
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.LOOM)
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c => c.COUNT)
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER)

                    .Include(c => c.PROG_)
                    .ThenInclude(c => c.YARNFOR)
                    .Include(c => c.PROG_)
                    .ThenInclude(c => c.PL_BULK_PROG_YARN_D)
                    .ThenInclude(c => c.LOT)
                    .Include(c => c.SUBGROUP)
                    .Select(c => new PrWarpingProcessRopeDataViewModel
                    {
                        PlProductionSetDistribution = c,
                        PiDetails = c.PROG_.BLK_PROG_.RndProductionOrder.SO
                    })
                    .FirstOrDefaultAsync(c =>
                        c.PlProductionSetDistribution.SETID.Equals(int.Parse(setId)) &&
                        (c.PlProductionSetDistribution.PROG_.BLK_PROG_.RndProductionOrder.OTYPEID.Equals(401) ||
                         c.PlProductionSetDistribution.PROG_.BLK_PROG_.RndProductionOrder.OTYPEID.Equals(402) ||
                         c.PlProductionSetDistribution.PROG_.BLK_PROG_.RndProductionOrder.OTYPEID.Equals(419) ||
                         c.PlProductionSetDistribution.PROG_.BLK_PROG_.RndProductionOrder.OTYPEID.Equals(422)));

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<PrWarpingProcessRopeViewModel> FindAllByIdAsync(int id)
        {
            try
            {
                var result = await DenimDbContext.F_PR_WARPING_PROCESS_ROPE_MASTER

                    .Include(c => c.F_PR_WARPING_PROCESS_ROPE_DETAILS)
                    .Include(c => c.F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS)
                    .ThenInclude(c => c.BALL_ID_FKNavigation)

                    .Include(c => c.F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS)
                    .ThenInclude(c => c.BALL_ID_FK_Link)

                    .Include(c => c.F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS)
                    .ThenInclude(c => c.MACHINE_NONavigation)

                    .Include(c => c.F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS)
                    .ThenInclude(c => c.EMP)

                    .Include(c => c.F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS)
                    .ThenInclude(c => c.COUNT_)

                    .Include(c => c.F_PR_WARPING_PROCESS_ROPE_DETAILS)
                    .ThenInclude(c => c.PL_PRODUCTION_SETDISTRIBUTION.PROG_)

                    .Include(c => c.F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS)
                    .ThenInclude(c => c.COUNT_)
                    //.ToListAsync();
                    //.Select(c => new PrWarpingProcessRopeViewModel
                    // {
                    //     FPrWarpingProcessRopeMaster = c,
                    //     FPrWarpingProcessRopeDetailsList = c.F_PR_WARPING_PROCESS_ROPE_DETAILS.Select(x => new F_PR_WARPING_PROCESS_ROPE_DETAILS
                    //     {
                    //         WARPID = x.WARPID,
                    //         BALL_NO = x.BALL_NO,
                    //         WARP_LENGTH_PER_SET = x.WARP_LENGTH_PER_SET,
                    //         PL_PRODUCTION_SETDISTRIBUTION = new PL_PRODUCTION_SETDISTRIBUTION
                    //         {
                    //             PROG_ = new PL_BULK_PROG_SETUP_D
                    //             {
                    //                 PROG_NO = x.PL_PRODUCTION_SETDISTRIBUTION.PROG_.PROG_NO
                    //             },
                    //             SETID = x.PL_PRODUCTION_SETDISTRIBUTION.SETID
                    //         },
                    //         FPrWarpingProcessRopeBallDetailsList = x.F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS.Select(g => new F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS
                    //         {
                    //             WARP_PROG_ID = g.WARP_PROG_ID,
                    //             BALL_ID_FKNavigation = new F_BAS_BALL_INFO
                    //             {
                    //                 BALL_NO = g.BALL_ID_FKNavigation.BALL_NO
                    //             },
                    //             BALL_ID_FK_Link = new F_BAS_BALL_INFO
                    //             {
                    //                 BALL_NO = g.BALL_ID_FK_Link.BALL_NO
                    //             },
                    //             BALL_LENGTH = g.BALL_LENGTH,
                    //             LINK_BALL_LENGTH = g.LINK_BALL_LENGTH,
                    //             SHIFT_NAME = g.SHIFT_NAME,
                    //             COUNT_ = new BAS_YARN_COUNTINFO
                    //             {
                    //                 COUNTNAME = g.COUNT_.COUNTNAME
                    //             },
                    //             COUNT_ID = g.COUNT_ID,
                    //             MACHINE_NONavigation = new F_PR_WARPING_MACHINE
                    //             {
                    //                 MACHINE_NAME = g.MACHINE_NONavigation.MACHINE_NAME
                    //             },
                    //             MACHINE_NO = g.MACHINE_NO,
                    //             BREAKS_ENDS = g.BREAKS_ENDS,
                    //             ENDS_ROPE = g.ENDS_ROPE,
                    //             EMP = new F_HRD_EMPLOYEE
                    //             {
                    //                 FIRST_NAME = g.EMP.FIRST_NAME + " " + g.EMP.LAST_NAME
                    //             },
                    //             OPERATOR = g.OPERATOR,
                    //             REMARKS = g.REMARKS
                    //         }).ToList()
                    //     }).ToList(),
                    //     FPrWarpingProcessRopeYarnConsumDetailsList = c.F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS.Select(h => new F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS
                    //     {
                    //         COUNT_ = new BAS_YARN_COUNTINFO
                    //         {
                    //             COUNTNAME = h.COUNT_.COUNTNAME
                    //         },
                    //         BGT_KG_TOTAL = h.BGT_KG_TOTAL,
                    //         BGT_KG_PER_SET = h.BGT_KG_PER_SET,
                    //         WASTE_TOTAL = h.WASTE_TOTAL,
                    //         WASTE_PERCENTAGE_TOTAL = h.WASTE_PERCENTAGE_TOTAL,
                    //         REMARKS = h.REMARKS,
                    //         CONSM_ID = h.CONSM_ID
                    //     }).ToList()
                    // })
                    .Where(c => c.WARPID.Equals(id))
                    .FirstOrDefaultAsync();


                var prWarpingProcessRopeViewModel = new PrWarpingProcessRopeViewModel
                {
                    FPrWarpingProcessRopeMaster = result,
                    FPrWarpingProcessRopeDetailsList = result.F_PR_WARPING_PROCESS_ROPE_DETAILS.ToList(),
                    FPrWarpingProcessRopeBallDetailsList = result.F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS.ToList(),
                    FPrWarpingProcessRopeYarnConsumDetailsList =
                        result.F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS.ToList(),
                    FPrWarpingProcessRopeDetails = result.F_PR_WARPING_PROCESS_ROPE_DETAILS.FirstOrDefault()
                };

                //foreach (var item in prWarpingProcessRopeViewModel.FPrWarpingProcessRopeDetailsList)
                //{
                //    item.FPrWarpingProcessRopeBallDetailsList = item.F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS.ToList();
                //}

                return prWarpingProcessRopeViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<WarpingChartDataViewModel>> GetWarpingDateWiseLengthGraph()
        {
            try

            {
                var data = new List<WarpingChartDataViewModel>();
                var date = Convert.ToDateTime("2022-05-08");
                var defaultDate = Convert.ToDateTime("1501-01-01 00:00:00.000");

                for (var i = 0; i < 15; i++)
                {
                    data.Add(new WarpingChartDataViewModel
                    {
                        date1 = date.AddDays(i),

                        //warpChartData.TodaysWarping =await _denimDbContext.F_PR_WARPING_PROCESS_ROPE_MASTER
                        //    .Where(c => c.DELIVERY_DATE.Equals(date))
                        //    .Select(d => new F_PR_WARPING_PROCESS_ROPE_MASTER()
                        //    {
                        //        WARP_LENGTH = d.WARP_LENGTH
                        //    }).SumAsync(c => c.WARP_LENGTH) + await _denimDbContext.F_PR_WARPING_PROCESS_DW_MASTER
                        //    .Where(c => c.DEL_DATE.Equals(date))
                        //    .Select(d => new F_PR_WARPING_PROCESS_DW_MASTER()
                        //    {
                        //        WARPLENGTH = d.WARPLENGTH
                        //    }).SumAsync(c => Convert.ToDouble(c.WARPLENGTH ?? "0")) + await _denimDbContext
                        //    .F_PR_WARPING_PROCESS_SW_MASTER
                        //    .Where(c => c.DEL_DATE.Equals(date))
                        //    .Select(d => new F_PR_WARPING_PROCESS_SW_MASTER()
                        //    {
                        //        WARPLENGTH = d.WARPLENGTH
                        //    }).SumAsync(c => Convert.ToDouble(c.WARPLENGTH ?? "0")) + await _denimDbContext
                        //    .F_PR_WARPING_PROCESS_ECRU_MASTER
                        //    .Where(c => (c.DEL_DATE ?? default).ToString("yyyy-MM-dd").Equals(date))
                        //    .Select(d => new F_PR_WARPING_PROCESS_ECRU_MASTER()
                        //    {
                        //        WARPLENGTH = d.WARPLENGTH
                        //    }).SumAsync(c => Convert.ToDouble(c.WARPLENGTH ?? "0"));

                        //warpChartData.MonthlyWarping = await _denimDbContext.F_PR_WARPING_PROCESS_ROPE_MASTER
                        //    .Where(c => c.DELIVERY_DATE.Equals(date.Month))
                        //    .Select(d => new F_PR_WARPING_PROCESS_ROPE_MASTER()
                        //    {
                        //        WARP_LENGTH = d.WARP_LENGTH
                        //    }).SumAsync(c => c.WARP_LENGTH) + await _denimDbContext.F_PR_WARPING_PROCESS_DW_MASTER
                        //    .Where(c => c.DEL_DATE.Equals(date.Month))
                        //    .Select(d => new F_PR_WARPING_PROCESS_DW_MASTER()
                        //    {
                        //        WARPLENGTH = d.WARPLENGTH
                        //    }).SumAsync(c => Convert.ToDouble(c.WARPLENGTH ?? "0")) + await _denimDbContext
                        //    .F_PR_WARPING_PROCESS_SW_MASTER
                        //    .Where(c => c.DEL_DATE.Equals(date.Month))
                        //    .Select(d => new F_PR_WARPING_PROCESS_SW_MASTER()
                        //    {
                        //        WARPLENGTH = d.WARPLENGTH
                        //    }).SumAsync(c => Convert.ToDouble(c.WARPLENGTH ?? "0")) + await _denimDbContext
                        //    .F_PR_WARPING_PROCESS_ECRU_MASTER
                        //    .Where(c => (c.DEL_DATE ?? default).ToString("yyyy-MM-dd").Equals(date.Month))
                        //    .Select(d => new F_PR_WARPING_PROCESS_ECRU_MASTER()
                        //    {
                        //        WARPLENGTH = d.WARPLENGTH
                        //    }).SumAsync(c => Convert.ToDouble(c.WARPLENGTH ?? "0"));

                        //warpChartData.BallWarping = await _denimDbContext.F_PR_WARPING_PROCESS_ROPE_MASTER
                        //    .Where(c => c.DELIVERY_DATE.Equals(Convert.ToDateTime("2022-05-08 00:00:00.000").Date))
                        //    .Select(d => new F_PR_WARPING_PROCESS_ROPE_MASTER
                        //    {
                        //        WARP_LENGTH = d.WARP_LENGTH
                        //    }).SumAsync(c => c.WARP_LENGTH);

                        //warpChartData.DirectWarping = await _denimDbContext.F_PR_WARPING_PROCESS_DW_MASTER
                        //    .Where(c => c.DEL_DATE.Equals(Convert.ToDateTime("2022-04-30 00:00:00.000").Date))
                        //    .Select(d => new F_PR_WARPING_PROCESS_DW_MASTER()
                        //    {
                        //        WARPLENGTH = d.WARPLENGTH
                        //    }).SumAsync(c => Convert.ToDouble(c.WARPLENGTH ?? "0"));

                        //warpChartData.SectionalWarping = await _denimDbContext.F_PR_WARPING_PROCESS_SW_MASTER
                        //    .Where(c => c.DEL_DATE.Equals(Convert.ToDateTime("2022-05-08 00:00:00.000").Date))
                        //    .Select(d => new F_PR_WARPING_PROCESS_SW_MASTER()
                        //    {
                        //        WARPLENGTH = d.WARPLENGTH
                        //    }).SumAsync(c => Convert.ToDouble(c.WARPLENGTH ?? "0"));

                        //warpChartData.EcruWarping = await _denimDbContext.F_PR_WARPING_PROCESS_ECRU_MASTER
                        //    .Where(c => (c.DEL_DATE ?? default).ToString("yyyy-MM-dd").Equals("2022-04-30"))
                        //    .Select(d => new F_PR_WARPING_PROCESS_ECRU_MASTER()
                        //    {
                        //        WARPLENGTH = d.WARPLENGTH
                        //    }).SumAsync(c => Convert.ToDouble(c.WARPLENGTH ?? "0"));
                        //warpChartData.Recone = await _denimDbContext.F_PR_RECONE_MASTER
                        //    .Where(c => c.TRANSDATE.Equals(Convert.ToDateTime("2022-04-26 ").Date))
                        //    .Select(d => new F_PR_RECONE_MASTER()
                        //    {
                        //        WARP_LENGTH = d.WARP_LENGTH
                        //    }).SumAsync(c => Convert.ToDouble(c.WARP_LENGTH ?? 0));

                        TotalWarping = await DenimDbContext.F_PR_WARPING_PROCESS_ROPE_MASTER
                            .Where(c => c.DELIVERY_DATE.Equals(date.AddDays(i).Date))
                            .Select(d => new F_PR_WARPING_PROCESS_ROPE_MASTER()
                            {
                                WARP_LENGTH = d.WARP_LENGTH
                            }).SumAsync(c => c.WARP_LENGTH) + await DenimDbContext.F_PR_WARPING_PROCESS_DW_MASTER
                            .Where(c => c.DEL_DATE.Equals(date.AddDays(i).Date))
                            .Select(d => new F_PR_WARPING_PROCESS_DW_MASTER()
                            {
                                WARPLENGTH = d.WARPLENGTH
                            }).SumAsync(c => Convert.ToDouble(c.WARPLENGTH ?? "0")) + await DenimDbContext
                            .F_PR_WARPING_PROCESS_SW_MASTER
                            .Where(c => c.DEL_DATE.Equals(date.AddDays(i).Date))
                            .Select(d => new F_PR_WARPING_PROCESS_SW_MASTER()
                            {
                                WARPLENGTH = d.WARPLENGTH
                            }).SumAsync(c => Convert.ToDouble(c.WARPLENGTH ?? "0")) + await DenimDbContext
                            .F_PR_WARPING_PROCESS_ECRU_MASTER
                            .Where(c => (c.DEL_DATE ?? default).ToString("yyyy-MM-dd").Equals(date.AddDays(i).Date))
                            .Select(d => new F_PR_WARPING_PROCESS_ECRU_MASTER()
                            {
                                WARPLENGTH = d.WARPLENGTH
                            }).SumAsync(c => c.WARPLENGTH)
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

        public async Task<WarpingChartDataViewModel> GetWarpingDataDayMonthAsync()
        {
            var date = Convert.ToDateTime("2022-02-27");
            var defaultDate = Convert.ToDateTime("1501-01-01 00:00:00.000");
            //var warpingChartDataViewModel = new WarpingChartDataViewModel();

            //var rope = await _denimDbContext.F_PR_WARPING_PROCESS_ROPE_MASTER
            //    .Where(c => (c.DELIVERY_DATE ?? defaultDate).Equals(date))
            //    .Select(d => new F_PR_WARPING_PROCESS_ROPE_MASTER
            //    {
            //        WARP_LENGTH = d.WARP_LENGTH
            //    }).SumAsync(c => c.WARP_LENGTH);

            //var dw = await _denimDbContext.F_PR_WARPING_PROCESS_DW_MASTER
            //    .Where(c => (c.DEL_DATE ?? defaultDate).Equals(date))
            //    .Select(d => new F_PR_WARPING_PROCESS_DW_MASTER
            //    {
            //        WARPLENGTH = d.WARPLENGTH
            //    }).SumAsync(c => Convert.ToDouble(c.WARPLENGTH ?? "0"));

            //var sw = await _denimDbContext
            //    .F_PR_WARPING_PROCESS_SW_MASTER
            //    .Where(c => (c.DEL_DATE ?? defaultDate).Equals(date))
            //    .Select(d => new F_PR_WARPING_PROCESS_SW_MASTER
            //    {
            //        WARPLENGTH = d.WARPLENGTH
            //    }).SumAsync(c => Convert.ToDouble(c.WARPLENGTH ?? "0"));

            //var ecru = await _denimDbContext
            //    .F_PR_WARPING_PROCESS_ECRU_MASTER
            //    .Where(c => (c.DEL_DATE ?? defaultDate).Date.Equals(date))
            //    .Select(d => new F_PR_WARPING_PROCESS_ECRU_MASTER
            //    {
            //        WARPLENGTH = d.WARPLENGTH
            //    }).SumAsync(c => (c.WARPLENGTH ?? 0));

            //warpingChartDataViewModel.TodaysWarping = rope + dw + sw + ecru;

            //return warpingChartDataViewModel;

            //MonthlyWarping = await _denimDbContext.F_PR_WARPING_PROCESS_ROPE_MASTER
            //    .Where(c => (c.DELIVERY_DATE).Equals(date.Month))
            //    .Select(d => new F_PR_WARPING_PROCESS_ROPE_MASTER
            //    {
            //        WARP_LENGTH = d.WARP_LENGTH
            //    }).SumAsync(c => c.WARP_LENGTH) + await _denimDbContext.F_PR_WARPING_PROCESS_DW_MASTER
            //    .Where(c => c.DEL_DATE.Equals(date.Month))
            //    .Select(d => new F_PR_WARPING_PROCESS_DW_MASTER
            //    {
            //        WARPLENGTH = d.WARPLENGTH
            //    }).SumAsync(c => Convert.ToDouble(c.WARPLENGTH ?? "0")) + await _denimDbContext
            //    .F_PR_WARPING_PROCESS_SW_MASTER
            //    .Where(c => c.DEL_DATE.Equals(date.Month))
            //    .Select(d => new F_PR_WARPING_PROCESS_SW_MASTER()
            //    {
            //        WARPLENGTH = d.WARPLENGTH
            //    }).SumAsync(c => Convert.ToDouble(c.WARPLENGTH ?? "0")) + await _denimDbContext
            //    .F_PR_WARPING_PROCESS_ECRU_MASTER
            //    .Where(c => (c.DEL_DATE ?? default).ToString("yyyy-MM-dd").Equals(date.Month))
            //    .Select(d => new F_PR_WARPING_PROCESS_ECRU_MASTER()
            //    {
            //        WARPLENGTH = d.WARPLENGTH
            //    }).SumAsync(c => Convert.ToDouble(c.WARPLENGTH ?? "0"))

            return new WarpingChartDataViewModel()
            {
                TodaysWarping = await DenimDbContext.F_PR_WARPING_PROCESS_ROPE_MASTER
                    .Where(c => (c.DELIVERY_DATE ?? defaultDate).Equals(date))
                    .Select(d => new F_PR_WARPING_PROCESS_ROPE_MASTER
                    {
                        WARP_LENGTH = d.WARP_LENGTH
                    }).SumAsync(c => c.WARP_LENGTH) + await DenimDbContext.F_PR_WARPING_PROCESS_DW_MASTER
                    .Where(c => (c.DEL_DATE ?? defaultDate).Equals(date))
                    .Select(d => new F_PR_WARPING_PROCESS_DW_MASTER
                    {
                        WARPLENGTH = d.WARPLENGTH
                    }).SumAsync(c => Convert.ToDouble(c.WARPLENGTH ?? "0")) + await DenimDbContext
                    .F_PR_WARPING_PROCESS_SW_MASTER
                    .Where(c => (c.DEL_DATE ?? defaultDate).Equals(date))
                    .Select(d => new F_PR_WARPING_PROCESS_SW_MASTER
                    {
                        WARPLENGTH = d.WARPLENGTH
                    }).SumAsync(c => Convert.ToDouble(c.WARPLENGTH ?? "0")) + await DenimDbContext
                    .F_PR_WARPING_PROCESS_ECRU_MASTER
                    .Where(c => (c.DEL_DATE ?? defaultDate).Date.Equals(date))
                    .Select(d => new F_PR_WARPING_PROCESS_ECRU_MASTER
                    {
                        WARPLENGTH = d.WARPLENGTH
                    }).SumAsync(c => c.WARPLENGTH),

                MonthlyWarping = await DenimDbContext.F_PR_WARPING_PROCESS_ROPE_MASTER
                    .Where(c => (c.DELIVERY_DATE ?? defaultDate).ToString("yyyy-MM").Equals(date.ToString("yyyy-MM")))
                    .Select(d => new F_PR_WARPING_PROCESS_ROPE_MASTER
                    {
                        WARP_LENGTH = d.WARP_LENGTH
                    }).SumAsync(c => c.WARP_LENGTH) + await DenimDbContext.F_PR_WARPING_PROCESS_DW_MASTER
                    .Where(c => (c.DEL_DATE ?? defaultDate).ToString("yyyy-MM").Equals(date.ToString("yyyy-MM")))
                    .Select(d => new F_PR_WARPING_PROCESS_DW_MASTER
                    {
                        WARPLENGTH = d.WARPLENGTH
                    }).SumAsync(c => Convert.ToDouble(c.WARPLENGTH ?? "0")) + await DenimDbContext
                    .F_PR_WARPING_PROCESS_SW_MASTER
                    .Where(c => (c.DEL_DATE ?? defaultDate).ToString("yyyy-MM").Equals(date.ToString("yyyy-MM")))
                    .Select(d => new F_PR_WARPING_PROCESS_SW_MASTER()
                    {
                        WARPLENGTH = d.WARPLENGTH
                    }).SumAsync(c => Convert.ToDouble(c.WARPLENGTH ?? "0")) + await DenimDbContext
                    .F_PR_WARPING_PROCESS_ECRU_MASTER
                    .Where(c => (c.DEL_DATE ?? defaultDate).ToString("yyyy-MM").Equals(date.ToString("yyyy-MM")))
                    .Select(d => new F_PR_WARPING_PROCESS_ECRU_MASTER()
                    {
                        WARPLENGTH = d.WARPLENGTH
                    }).SumAsync(c => c.WARPLENGTH),


            };
        }

        public async Task<WarpingChartDataViewModel> GetWarpingPendingSets()
        {

            var date = Convert.ToDateTime("2022-02-27");
            var defaultDate = Convert.ToDateTime("1501-01-01 00:00:00.000");
            var warpingChartDataViewModel = new WarpingChartDataViewModel
            {
                WarpingPendingSets = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION.CountAsync(c =>
                    !DenimDbContext.F_PR_WARPING_PROCESS_ROPE_DETAILS.Any(e => e.SETID.Equals(c.SETID)) &&
                    !DenimDbContext.F_PR_WARPING_PROCESS_DW_MASTER.Any(e => e.SETID.Equals(c.SETID)) &&
                    !DenimDbContext.F_PR_WARPING_PROCESS_ECRU_MASTER.Any(e => e.SETID.Equals(c.SETID)) &&
                    !DenimDbContext.F_PR_WARPING_PROCESS_SW_MASTER.Any(e => e.SETID.Equals(c.SETID))),

                WarpingCompleteSets = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION.CountAsync(c =>
                    DenimDbContext.F_PR_WARPING_PROCESS_ROPE_DETAILS.Any(e => e.SETID.Equals(c.SETID)) ||
                    DenimDbContext.F_PR_WARPING_PROCESS_DW_MASTER.Any(e => e.SETID.Equals(c.SETID)) ||
                    DenimDbContext.F_PR_WARPING_PROCESS_ECRU_MASTER.Any(e => e.SETID.Equals(c.SETID)) ||
                    DenimDbContext.F_PR_WARPING_PROCESS_SW_MASTER.Any(e => e.SETID.Equals(c.SETID))),

                PendingPercent = float.Parse(Math.Round((await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION.CountAsync(c =>
                    !DenimDbContext.F_PR_WARPING_PROCESS_ROPE_DETAILS.Any(e => e.SETID.Equals(c.SETID)) &&
                    !DenimDbContext.F_PR_WARPING_PROCESS_DW_MASTER.Any(e => e.SETID.Equals(c.SETID)) &&
                    !DenimDbContext.F_PR_WARPING_PROCESS_ECRU_MASTER.Any(e => e.SETID.Equals(c.SETID)) &&
                    !DenimDbContext.F_PR_WARPING_PROCESS_SW_MASTER.Any(e => e.SETID.Equals(c.SETID)))) / Convert.ToSingle(await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION.Select(c => c.SETID).CountAsync()) * 100, 2).ToString("0.0")),


                CompletePercent = Math.Round((await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION.CountAsync(c =>
                                                  DenimDbContext.F_PR_WARPING_PROCESS_ROPE_DETAILS.Any(e => e.SETID.Equals(c.SETID)) ||
                                                  DenimDbContext.F_PR_WARPING_PROCESS_DW_MASTER.Any(e => e.SETID.Equals(c.SETID)) ||
                                                  DenimDbContext.F_PR_WARPING_PROCESS_ECRU_MASTER.Any(e => e.SETID.Equals(c.SETID)) ||
                                                  DenimDbContext.F_PR_WARPING_PROCESS_SW_MASTER.Any(e => e.SETID.Equals(c.SETID)))
                                              / Convert.ToSingle(await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION.Select(c => c.SETID).CountAsync())) * 100)

            };

            return warpingChartDataViewModel;
        }

        public async Task<List<PL_PRODUCTION_SETDISTRIBUTION>> GetWarpingPendingSetList()
        {
            var result =
                await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Where(c => !DenimDbContext.F_PR_WARPING_PROCESS_ROPE_DETAILS.Any(e => e.SETID.Equals(c.SETID)) &&
                                !DenimDbContext.F_PR_WARPING_PROCESS_DW_MASTER.Any(e => e.SETID.Equals(c.SETID)) &&
                                !DenimDbContext.F_PR_WARPING_PROCESS_ECRU_MASTER.Any(e => e.SETID.Equals(c.SETID)) &&
                                !DenimDbContext.F_PR_WARPING_PROCESS_SW_MASTER.Any(e => e.SETID.Equals(c.SETID)))
                    .Select(c => new PL_PRODUCTION_SETDISTRIBUTION()
                    {
                        TRNSDATE = c.TRNSDATE,
                        PROG_ = new PL_BULK_PROG_SETUP_D
                        {
                            PROG_NO = c.PROG_.PROG_NO,
                            PROGRAM_TYPE = c.PROG_.PROGRAM_TYPE ?? "N/A"
                        }
                    }).OrderByDescending(c => c.PROG_.PROG_NO).Take(7).ToListAsync();
            return result;


        }

        public async Task<WarpingChartDataViewModel> GetBudgetConsumedYarn()
        {
            return new WarpingChartDataViewModel()
            {
                ConsumedYarn = await DenimDbContext.F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS
                                   .Select(c => c.CONSUM_TOTAL).SumAsync() +
                               await DenimDbContext.F_PR_WARPING_PROCESS_SW_YARN_CONSUM_DETAILS.Select(c => c.CONSM)
                                   .SumAsync() +
                               await DenimDbContext.F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS.Select(c => c.CONSM)
                                   .SumAsync() +
                               await DenimDbContext.F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS.Select(c => c.CONSM)
                                   .SumAsync(),
                BudgetYrn =
                    await DenimDbContext.F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS.Select(c => c.BGT_KG_TOTAL)
                        .SumAsync() +
                    await DenimDbContext.F_PR_WARPING_PROCESS_SW_YARN_CONSUM_DETAILS.Select(c => c.BGT_KG).SumAsync() +
                    await DenimDbContext.F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS.Select(c => c.BGT_KG)
                        .SumAsync() + await DenimDbContext.F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS
                        .Select(c => c.BGT_KG).SumAsync()
            };
        }

        public async Task<WarpingChartDataViewModel> GetRopeWarpingProductionData()
        {
            var date = Convert.ToDateTime("2022-02-27");
            var defaultDate = Convert.ToDateTime("1501-01-01 00:00:00.000");
            var warpChartData = new WarpingChartDataViewModel();

            warpChartData.TotalRopeWarping = await DenimDbContext.F_PR_WARPING_PROCESS_ROPE_MASTER
                //.Where(c => c.DELIVERY_DATE.Equals(Convert.ToDateTime("2022-05-08 00:00:00.000").Date))
                .Select(d => new F_PR_WARPING_PROCESS_ROPE_MASTER()
                {
                    WARP_LENGTH = d.WARP_LENGTH
                }).SumAsync(c => c.WARP_LENGTH);
            warpChartData.MonthlyRopeWarping = await DenimDbContext.F_PR_WARPING_PROCESS_ROPE_MASTER
                .Where(c => (c.DELIVERY_DATE ?? defaultDate).ToString("yyyy-MM").Equals(date.ToString("yyyy-MM")))
                .Select(d => new F_PR_WARPING_PROCESS_ROPE_MASTER()
                {
                    WARP_LENGTH = d.WARP_LENGTH
                }).SumAsync(c => c.WARP_LENGTH);
            warpChartData.ComparisonMonthlyRopeWarping = await DenimDbContext.F_PR_WARPING_PROCESS_ROPE_MASTER
                .Where(c => (c.DELIVERY_DATE ?? defaultDate).ToString("yyyy-MM").Equals(date.ToString("yyyy-MM")))
                .Select(d => new F_PR_WARPING_PROCESS_ROPE_MASTER()
                {
                    WARP_LENGTH = d.WARP_LENGTH ?? 0
                }).SumAsync(c => c.WARP_LENGTH) - await DenimDbContext.F_PR_WARPING_PROCESS_ROPE_MASTER
                .Where(c => (c.DELIVERY_DATE ?? defaultDate).ToString("yyyy-MM")
                    .Equals(date.AddMonths(-1).ToString("yyyy-MM")))
                .Select(d => new F_PR_WARPING_PROCESS_ROPE_MASTER()
                {
                    WARP_LENGTH = d.WARP_LENGTH
                }).SumAsync(c => c.WARP_LENGTH);
            warpChartData.TodaysRopeWarping = await DenimDbContext.F_PR_WARPING_PROCESS_ROPE_MASTER
                .Where(c => c.DELIVERY_DATE.Equals(Convert.ToDateTime("2022-05-08 00:00:00.000").Date))
                .Select(d => new F_PR_WARPING_PROCESS_ROPE_MASTER()
                {
                    WARP_LENGTH = d.WARP_LENGTH
                }).SumAsync(c => c.WARP_LENGTH);
            return warpChartData;
        }

        public async Task<List<WarpingChartDataViewModel>> GetRopeWarpingProductionList()
        {

            var data = new List<WarpingChartDataViewModel>();
            var date = Convert.ToDateTime("2022-05-08");
            var defaultDate = Convert.ToDateTime("1501-01-01 00:00:00.000");

            for (var i = 0; i < 15; i++)
            {
                data.Add(new WarpingChartDataViewModel
                {
                    TotalWarping = await DenimDbContext.F_PR_WARPING_PROCESS_ROPE_MASTER
                        .Where(c => (c.DELIVERY_DATE ?? defaultDate).Equals(date.AddDays(i).Date))
                        .Select(d => new F_PR_WARPING_PROCESS_ROPE_MASTER()
                        {
                            WARP_LENGTH = d.WARP_LENGTH
                        }).SumAsync(c => c.WARP_LENGTH)
                });
            }


            return data;
        }

        public async Task<List<WarpingChartDataViewModel>> GetWarpingProductionList()
        {
            try

            {
                var data = new List<WarpingChartDataViewModel>();
                var date = Convert.ToDateTime("2022-05-08");
                var defaultDate = Convert.ToDateTime("1501-01-01 00:00:00.000");

                for (var i = 0; i < 15; i++)
                {
                    data.Add(new WarpingChartDataViewModel
                    {
                        date1 = date.AddDays(i),


                        TotalWarping = await DenimDbContext.F_PR_WARPING_PROCESS_ROPE_MASTER
                            .Where(c => c.DELIVERY_DATE.Equals(date.AddDays(i).Date))
                            .Select(d => new F_PR_WARPING_PROCESS_ROPE_MASTER()
                            {
                                WARP_LENGTH = d.WARP_LENGTH
                            }).SumAsync(c => c.WARP_LENGTH) + await DenimDbContext.F_PR_WARPING_PROCESS_DW_MASTER
                            .Where(c => c.DEL_DATE.Equals(date.AddDays(i).Date))
                            .Select(d => new F_PR_WARPING_PROCESS_DW_MASTER()
                            {
                                WARPLENGTH = d.WARPLENGTH
                            }).SumAsync(c => Convert.ToDouble(c.WARPLENGTH ?? "0")) + await DenimDbContext
                            .F_PR_WARPING_PROCESS_SW_MASTER
                            .Where(c => c.DEL_DATE.Equals(date.AddDays(i).Date))
                            .Select(d => new F_PR_WARPING_PROCESS_SW_MASTER()
                            {
                                WARPLENGTH = d.WARPLENGTH
                            }).SumAsync(c => Convert.ToDouble(c.WARPLENGTH ?? "0")) + await DenimDbContext
                            .F_PR_WARPING_PROCESS_ECRU_MASTER
                            .Where(c => (c.DEL_DATE ?? default).ToString("yyyy-MM-dd").Equals(date.AddDays(i).Date))
                            .Select(d => new F_PR_WARPING_PROCESS_ECRU_MASTER()
                            {
                                WARPLENGTH = d.WARPLENGTH
                            }).SumAsync(c => c.WARPLENGTH)
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

        public async Task<WarpingChartDataViewModel> MonthlyWarpingPendingsAndCompleteSets()
        {
            var date = Convert.ToDateTime("2022-02-27");
            var defaultDate = Convert.ToDateTime("1501-01-01 00:00:00.000");
            var warpingChartDataViewModel = new WarpingChartDataViewModel();
            var totalSet = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION.Select(c => c.SETID).CountAsync();

            var mPendingSets = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                .Where(c => (c.TRNSDATE ?? defaultDate).ToString("yyyy-MM").Equals(date.ToString("yyyy-MM")))
                .CountAsync(c =>
                    !DenimDbContext.F_PR_WARPING_PROCESS_ROPE_DETAILS.Any(e => e.SETID.Equals(c.SETID)) &&
                    !DenimDbContext.F_PR_WARPING_PROCESS_DW_MASTER.Any(e => e.SETID.Equals(c.SETID)) &&
                    !DenimDbContext.F_PR_WARPING_PROCESS_ECRU_MASTER.Any(e => e.SETID.Equals(c.SETID)) &&
                    !DenimDbContext.F_PR_WARPING_PROCESS_SW_MASTER.Any(e => e.SETID.Equals(c.SETID)));

            var mPendingPercent = (mPendingSets / Convert.ToSingle(totalSet)) * 100;
            var mPendingPercentR = Math.Round(mPendingPercent, 1);


            var mCompeteSets = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION.CountAsync(c =>
                DenimDbContext.F_PR_WARPING_PROCESS_ROPE_DETAILS.Any(e => e.SETID.Equals(c.SETID)) ||
                DenimDbContext.F_PR_WARPING_PROCESS_DW_MASTER.Any(e => e.SETID.Equals(c.SETID)) ||
                DenimDbContext.F_PR_WARPING_PROCESS_ECRU_MASTER.Any(e => e.SETID.Equals(c.SETID)) ||
                DenimDbContext.F_PR_WARPING_PROCESS_SW_MASTER.Any(e => e.SETID.Equals(c.SETID)));

            var mCompletePercent = (mCompeteSets / Convert.ToSingle(totalSet)) * 100;
            var mCompletePercentR = Math.Round(mCompletePercent, 1);


            warpingChartDataViewModel.MonthlyCompletePercent = mCompletePercentR;
            warpingChartDataViewModel.MonthlyPendingPercent = mPendingPercentR;
            warpingChartDataViewModel.MonthlyWarpingCompleteSets = mCompeteSets;
            warpingChartDataViewModel.MonthlyWarpingPendingSets = mPendingSets;

            return warpingChartDataViewModel;
        }
    }
}

