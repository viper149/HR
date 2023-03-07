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
using DenimERP.ViewModels.Rnd;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_LCB_PRODUCTION_ROPE_MASTER_Repository : BaseRepository<F_LCB_PRODUCTION_ROPE_MASTER>, IF_LCB_PRODUCTION_ROPE_MASTER
    {
        public SQLF_LCB_PRODUCTION_ROPE_MASTER_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<F_LCB_PRODUCTION_ROPE_MASTER>> GetAllAsync()
        {
            try
            {
                var result = await DenimDbContext.F_LCB_PRODUCTION_ROPE_MASTER
                    .Include(c => c.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c => c.PROG_)
                    .ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FLcbProductionRopeViewModel> GetInitObjects(FLcbProductionRopeViewModel fLcbProductionRopeViewModel)
        {
            try
            {
                fLcbProductionRopeViewModel.PlProductionPlanMasters = await DenimDbContext.PL_PRODUCTION_PLAN_MASTER
                    .Select(c => new PL_PRODUCTION_PLAN_MASTER
                    {
                        GROUPID = c.GROUPID,
                        GROUP_NO = c.GROUP_NO
                    }).ToListAsync();

                //fLcbProductionRopeViewModel.PlProductionSetDistributions = await _denimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                //    .Include(c => c.PROG_)
                //    .Where(c => _denimDbContext.F_DYEING_PROCESS_ROPE_DETAILS.Any(e => e.SETID.Equals(c.SETID)
                //        //&& e.CLOSE_STATUS
                //        ))
                //    .Select(c => new PL_PRODUCTION_SETDISTRIBUTION
                //    {
                //        SETID = c.SETID,
                //        PROG_ = c.PROG_
                //    }).ToListAsync();

                fLcbProductionRopeViewModel.PlProductionPlanDetailsList = await DenimDbContext.PL_PRODUCTION_PLAN_DETAILS
                    .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c => c.PROG_.PROG_NO)
                    .Include(c => c.GROUP.RND_DYEING_TYPE)
                    .Where(c => c.GROUP.RND_DYEING_TYPE.DTYPE.Equals("Rope") && c.F_DYEING_PROCESS_ROPE_DETAILS.All(d => d.CLOSE_STATUS) && !DenimDbContext.F_LCB_PRODUCTION_ROPE_MASTER.Any(e => e.SUBGROUPID.Equals(c.SUBGROUPID)) && DenimDbContext.F_DYEING_PROCESS_ROPE_DETAILS.Any(e => e.SUBGROUPID.Equals(c.SUBGROUPID)))
                    .Select(c => new PL_PRODUCTION_PLAN_DETAILS
                    {
                        SUBGROUPID = c.SUBGROUPID,
                        OPT1 = $"{c.SUBGROUPNO} - {c.PL_PRODUCTION_SETDISTRIBUTION.FirstOrDefault().PROG_.PROG_NO}"
                    }).ToListAsync();

                fLcbProductionRopeViewModel.PlProductionPlanDetailsEditList = await DenimDbContext.PL_PRODUCTION_PLAN_DETAILS
                    .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c => c.PROG_.PROG_NO)
                    .Include(c => c.GROUP.RND_DYEING_TYPE)
                    .Where(c => c.GROUP.RND_DYEING_TYPE.DTYPE.Equals("Rope") && c.F_DYEING_PROCESS_ROPE_DETAILS.All(d => d.CLOSE_STATUS) && DenimDbContext.F_LCB_PRODUCTION_ROPE_MASTER.Any(e => e.SUBGROUPID.Equals(c.SUBGROUPID)) && DenimDbContext.F_DYEING_PROCESS_ROPE_DETAILS.Any(e => e.SUBGROUPID.Equals(c.SUBGROUPID)))
                    .Select(c => new PL_PRODUCTION_PLAN_DETAILS
                    {
                        SUBGROUPID = c.SUBGROUPID,
                        OPT1 = $"{c.SUBGROUPNO} - {c.PL_PRODUCTION_SETDISTRIBUTION.FirstOrDefault().PROG_.PROG_NO}"
                    }).ToListAsync();

                fLcbProductionRopeViewModel.FDyeingProcessRopeDetailsList = await DenimDbContext.F_DYEING_PROCESS_ROPE_DETAILS
                    .Include(c => c.CAN_NONavigation)
                    .Select(c => new F_DYEING_PROCESS_ROPE_DETAILS
                    {
                        ROPEID = c.ROPEID,
                        CAN_NONavigation = c.CAN_NONavigation
                    }).ToListAsync();

                fLcbProductionRopeViewModel.FLcbMachines = await DenimDbContext.F_LCB_MACHINE.Select(c => new F_LCB_MACHINE
                {
                    ID = c.ID,
                    MACHINE_NO = c.MACHINE_NO
                }).ToListAsync();
                fLcbProductionRopeViewModel.FLcbBeams = await DenimDbContext.F_LCB_BEAM
                    .Where(c => c.FOR_.Equals("LCB"))
                    .Select(c => new F_LCB_BEAM
                    {
                        ID = c.ID,
                        BEAM_NO = c.BEAM_NO
                    }).ToListAsync();

                var FHrEmployees = await DenimDbContext.F_HR_EMP_OFFICIALINFO
                    .Include(c => c.EMP)
                    .Where(c => c.SECID.Equals(159) && !c.OPN2.Equals("Y"))
                    .ToListAsync();

                fLcbProductionRopeViewModel.FHrEmployees = FHrEmployees.Select(c => new F_HRD_EMPLOYEE
                {
                    EMPID = c.EMP.EMPID,
                    FIRST_NAME = c.EMP.FIRST_NAME + " " + c.EMP.LAST_NAME + '-' + c.EMP.EMPNO
                }).ToList();

                return fLcbProductionRopeViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<int> InsertAndGetIdAsync(F_LCB_PRODUCTION_ROPE_MASTER fLcbProductionRopeMaster)
        {
            try
            {
                await DenimDbContext.F_LCB_PRODUCTION_ROPE_MASTER.AddAsync(fLcbProductionRopeMaster);
                await SaveChanges();
                return fLcbProductionRopeMaster.LCBPROID;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<RndProductionOrderDetailViewModel> GetSetDetails(int setId)
        {
            try
            {
                var result = DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Include(c => c.SUBGROUP.GROUP.RND_DYEING_TYPE)
                    .Include(c => c.SUBGROUP.GROUP.F_DYEING_PROCESS_ROPE_MASTER)

                    .Include(c => c.F_PR_WARPING_PROCESS_ROPE_DETAILS)


                    .Include(c => c.F_PR_SIZING_PROCESS_ROPE_MASTER)
                    .ThenInclude(c => c.F_PR_SIZING_PROCESS_ROPE_DETAILS)
                    .ThenInclude(c => c.S_M)

                    .Include(c => c.F_PR_SIZING_PROCESS_ROPE_MASTER)
                    .ThenInclude(c => c.F_PR_SIZING_PROCESS_ROPE_DETAILS)
                    .ThenInclude(c => c.W_BEAM)

                    .Include(c => c.F_PR_SLASHER_DYEING_MASTER)
                    .ThenInclude(c => c.F_PR_SLASHER_DYEING_DETAILS)
                    .ThenInclude(c => c.W_BEAM)

                    .Include(c => c.F_PR_SLASHER_DYEING_MASTER)
                    .ThenInclude(c => c.F_PR_SLASHER_DYEING_DETAILS)
                    .ThenInclude(c => c.SL_M)

                    .Include(c => c.F_PR_WARPING_PROCESS_DW_MASTER)
                    .ThenInclude(c => c.F_PR_WARPING_PROCESS_DW_DETAILS)

                    .Include(c => c.F_PR_WEAVING_PROCESS_MASTER_B)
                    .ThenInclude(c => c.F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS)

                    .Include(c => c.F_PR_WEAVING_PROCESS_MASTER_B)
                    .ThenInclude(c => c.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B)
                    .ThenInclude(c => c.F_PR_WEAVING_PROCESS_DETAILS_B)

                    //.Include(c => c.F_LCB_PRODUCTION_ROPE_MASTER)

                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.WV)
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation)
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.LOOM)

                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c => c.LOT)

                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c => c.SUPP)

                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c => c.COUNT)

                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_YARNCONSUMPTION)

                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER)
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BRAND)

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
                    .FirstOrDefault(c => c.PlProductionSetDistribution.SETID.Equals(setId));


                //var result = _denimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                //    .Include(c => c.SUBGROUP)
                //    .Include(c => c.F_PR_WARPING_PROCESS_ROPE_DETAILS)
                //    .Include(c => c.F_PR_WEAVING_BEAM_RECEIVING)
                //    .ThenInclude(c => c.F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS)
                //    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder)
                //    .Include(c => c.F_PR_SIZING_PROCESS_ROPE_MASTER)
                //    .ThenInclude(c => c.F_PR_SIZING_PROCESS_ROPE_DETAILS)
                //    .ThenInclude(c => c.S_M)
                //    .Include(c => c.F_PR_SIZING_PROCESS_ROPE_MASTER)
                //    .ThenInclude(c => c.F_PR_SIZING_PROCESS_ROPE_DETAILS)
                //    .ThenInclude(c => c.W_BEAM)

                //    .Include(c => c.F_PR_WEAVING_PROCESS_MASTER_B)
                //    .ThenInclude(c => c.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B)
                //    .ThenInclude(c => c.F_PR_WEAVING_PROCESS_DETAILS_B)
                //    .Include(c=>c.F_LCB_PRODUCTION_ROPE_MASTER)
                //    .GroupJoin(_denimDbContext.COM_EX_PI_DETAILS
                //            .Include(c => c.STYLE.FABCODENavigation.WV)
                //            .Include(c => c.STYLE.FABCODENavigation.COLORCODENavigation)
                //            .Include(c => c.STYLE.FABCODENavigation.LOOM)
                //            .Include(c => c.PIMASTER.BUYER),
                //        f0 => f0.PROG_.BLK_PROG_.RndProductionOrder.ORDERNO,
                //        f00 => f00.TRNSID,
                //        (f0, f00) => new RndProductionOrderDetailViewModel
                //        {
                //            ComExPiDetailsList = f00.ToList(),
                //            PlProductionSetDistribution = f0
                //        })
                //    .GroupJoin(_denimDbContext.RND_FABRIC_COUNTINFO.OrderBy(c => c.COUNTID)
                //            .Include(c => c.LOT)
                //            .Include(c => c.SUPP)
                //            .Include(c => c.COUNT),
                //        f1 => f1.ComExPiDetailsList.FirstOrDefault().STYLE.FABCODE,
                //        f2 => f2.FABCODE,
                //        (f1, f2) => new RndProductionOrderDetailViewModel
                //        {
                //            RndFabricCountInfos = f2.ToList(),
                //            ComExPiDetailsList = f1.ComExPiDetailsList,
                //            PlProductionSetDistribution = f1.PlProductionSetDistribution
                //        }
                //    )
                //    .GroupJoin(_denimDbContext.RND_YARNCONSUMPTION.OrderBy(c => c.COUNTID),
                //        f3 => f3.ComExPiDetailsList.FirstOrDefault().STYLE.FABCODE,
                //        f4 => f4.FABCODE,
                //        (f3, f4) => new RndProductionOrderDetailViewModel
                //        {
                //            RndFabricCountInfos = f3.RndFabricCountInfos,
                //            ComExPiDetailsList = f3.ComExPiDetailsList,
                //            PlProductionSetDistribution = f3.PlProductionSetDistribution,
                //            RndYarnconsumptions = f4.ToList()
                //        })
                //    .Select(c => new RndProductionOrderDetailViewModel
                //    {
                //        ComExPiDetailsList = c.ComExPiDetailsList,
                //        PlProductionSetDistribution = c.PlProductionSetDistribution,
                //        RndFabricCountInfoViewModels = c.RndFabricCountInfos.GroupJoin(c.RndYarnconsumptions.ToList(),
                //            f5 => f5.COUNTID,
                //            f6 => f6.COUNTID,
                //            (f5, f6) => new RndFabricCountInfoViewModel
                //            {
                //                RndFabricCountinfo = f5,
                //                AMOUNT = f6.FirstOrDefault().AMOUNT
                //            })
                //            .Select(e => new RndFabricCountInfoViewModel
                //            {
                //                RndFabricCountinfo = e.RndFabricCountinfo,
                //                AMOUNT = e.AMOUNT
                //            }).ToList()
                //    })
                //    .FirstOrDefault(c => c.PlProductionSetDistribution.SETID.Equals(setId));


                //foreach (var item in result.RndFabricCountInfoViewModels)
                //{
                //    item.RndFabricCountinfo = new RND_FABRIC_COUNTINFO
                //    {
                //        COUNTID = item.RndFabricCountinfo.COUNTID,
                //        EncryptedId = item.RndFabricCountinfo.EncryptedId,
                //        LOTID = item.RndFabricCountinfo.LOTID,
                //        SUPPID = item.RndFabricCountinfo.SUPPID,
                //        FABCODE = item.RndFabricCountinfo.FABCODE,
                //        TRNSID = item.RndFabricCountinfo.TRNSID,
                //        RATIO = item.RndFabricCountinfo.RATIO,
                //        YARNFOR = item.RndFabricCountinfo.YARNFOR,
                //        YARNTYPE = item.RndFabricCountinfo.YARNTYPE,
                //        NE = item.RndFabricCountinfo.NE,
                //        LOT = await _denimDbContext.BAS_YARN_LOTINFO.FirstOrDefaultAsync(c => c.LOTID.Equals(item.RndFabricCountinfo.LOTID)),
                //        COUNT = await _denimDbContext.BAS_YARN_COUNTINFO.FirstOrDefaultAsync(c => c.COUNTID.Equals(item.RndFabricCountinfo.COUNTID)),
                //        SUPP = await _denimDbContext.BAS_SUPPLIERINFO.FirstOrDefaultAsync(c => c.SUPPID.Equals(item.RndFabricCountinfo.SUPPID))
                //    };

                //    foreach (var i in result.PlProductionSetDistribution.F_PR_WEAVING_BEAM_RECEIVING)
                //    {
                //        item.FPrWeavingWeftYarnConsumDetails = i.F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS.FirstOrDefault(c => c.COUNTID.Equals(item.RndFabricCountinfo.TRNSID));
                //        if (item.FPrWeavingWeftYarnConsumDetails == null) continue;
                //        if (item.FPrWeavingWeftYarnConsumDetails != null) item.IsConsumption = true;
                //        break;
                //    }
                //}

                //var setNo = result.PlProductionSetDistribution.PROG_.PROG_NO.Replace("/", "");
                //var inspDetails = await _denimDbContext.F_PR_INSPECTION_PROCESS_DETAILS.Where(c=>c.ROLLNO.Contains(setNo)).OrderByDescending(c => c.ROLLNO).FirstOrDefaultAsync();
                //if (inspDetails != null)
                //{
                //    result.PlProductionSetDistribution.OPT3 = (long.Parse(inspDetails.ROLLNO) + 1).ToString();
                //}
                //else
                //{
                //    result.PlProductionSetDistribution.OPT3 = setNo+"001";
                //}

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<dynamic> GetSubGroupDetails(int subGroupId)
        {
            try
            {
                var select = await DenimDbContext.PL_PRODUCTION_PLAN_DETAILS
                    .Include(c => c.GROUP.RND_DYEING_TYPE)
                    .Include(c => c.GROUP.F_DYEING_PROCESS_ROPE_MASTER)
                    .Include(c => c.F_LCB_PRODUCTION_ROPE_MASTER)
                    .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.WV)
                    .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation)
                    .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.LOOM)
                    .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c => c.LOT)
                    .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c => c.SUPP)
                    .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c => c.COUNT)
                    .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_YARNCONSUMPTION)
                    .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER)
                    .Include(c => c.F_DYEING_PROCESS_ROPE_DETAILS)
                    .ThenInclude(c => c.CAN_NONavigation)
                    .Include(c => c.F_PR_WARPING_PROCESS_ROPE_MASTER)
                    .ThenInclude(c => c.F_PR_WARPING_PROCESS_ROPE_DETAILS)
                    .Where(c => c.SUBGROUPID.Equals(subGroupId))
                    .Select(e => new PL_PRODUCTION_PLAN_DETAILS
                    {
                        RATIO = e.RATIO,
                        GROUP = new PL_PRODUCTION_PLAN_MASTER
                        {
                            F_DYEING_PROCESS_ROPE_MASTER = e.GROUP.F_DYEING_PROCESS_ROPE_MASTER.Select(f => new F_DYEING_PROCESS_ROPE_MASTER
                            {
                                GROUP_LENGTH = f.GROUP_LENGTH,
                                DYEING_LENGTH = f.DYEING_LENGTH
                            }).ToList()
                        },
                        F_PR_WARPING_PROCESS_ROPE_MASTER = e.F_PR_WARPING_PROCESS_ROPE_MASTER.Select(f => new F_PR_WARPING_PROCESS_ROPE_MASTER
                        {
                            WARP_LENGTH = f.WARP_LENGTH,
                            F_PR_WARPING_PROCESS_ROPE_DETAILS = f.F_PR_WARPING_PROCESS_ROPE_DETAILS.Select(g => new F_PR_WARPING_PROCESS_ROPE_DETAILS
                            {
                                BALL_NO = $"{Fraction(g.BALL_NO ?? "0")}"
                            }).ToList()
                        }).ToList(),
                        F_DYEING_PROCESS_ROPE_DETAILS = e.F_DYEING_PROCESS_ROPE_DETAILS
                            .OrderBy(f => f.CAN_NONavigation.TUBE_NO)
                            .Select(f => new F_DYEING_PROCESS_ROPE_DETAILS
                            {
                                ROPEID = f.ROPEID,
                                CAN_NONavigation = new F_PR_TUBE_INFO
                                {
                                    TUBE_NO = $"{f.CAN_NONavigation.TUBE_NO}"
                                }
                            }).ToList(),
                        PL_PRODUCTION_SETDISTRIBUTION = e.PL_PRODUCTION_SETDISTRIBUTION.Select(f => new PL_PRODUCTION_SETDISTRIBUTION
                        {
                            PROG_ = new PL_BULK_PROG_SETUP_D
                            {
                                BLK_PROG_ = new PL_BULK_PROG_SETUP_M
                                {
                                    RndProductionOrder = new RND_PRODUCTION_ORDER
                                    {
                                        TOTAL_ENOS = f.PROG_.BLK_PROG_.RndProductionOrder.TOTAL_ENOS,
                                        SO = new COM_EX_PI_DETAILS
                                        {
                                            SO_NO = $"{f.PROG_.BLK_PROG_.RndProductionOrder.SO.SO_NO}",
                                            STYLE = new COM_EX_FABSTYLE
                                            {
                                                FABCODENavigation = new RND_FABRICINFO
                                                {
                                                    STYLE_NAME = $"{f.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME}",
                                                    COLORCODENavigation = new BAS_COLOR
                                                    {
                                                        COLOR = $"{f.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation.COLOR}",
                                                    },
                                                    LOOM = new LOOM_TYPE
                                                    {
                                                        LOOM_TYPE_NAME = $"{f.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.LOOM.LOOM_TYPE_NAME}"
                                                    }
                                                }
                                            },
                                            PIMASTER = new COM_EX_PIMASTER
                                            {
                                                PINO = $"{f.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.PINO}",
                                                BUYER = new BAS_BUYERINFO
                                                {
                                                    BUYER_NAME = $"{f.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER.BUYER_NAME}"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }).ToList()
                    }).ToListAsync();

                var firstOrDefault = @select.Select(c => new
                {
                    STYLENAME = $"{c.PL_PRODUCTION_SETDISTRIBUTION.Select(d => d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME).FirstOrDefault()}",
                    Buyer = $"{c.PL_PRODUCTION_SETDISTRIBUTION.Select(d => d.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER.BUYER_NAME).FirstOrDefault()}",
                    Color = $"{c.PL_PRODUCTION_SETDISTRIBUTION.Select(d => d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation.COLOR).FirstOrDefault()}",
                    Loomtype = $"{c.PL_PRODUCTION_SETDISTRIBUTION.Select(d => d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.LOOM.LOOM_TYPE_NAME).FirstOrDefault()}",
                    Orderno = $"{c.PL_PRODUCTION_SETDISTRIBUTION.Select(d => d.PROG_.BLK_PROG_.RndProductionOrder.SO.SO_NO).FirstOrDefault()}",
                    Ratio = $"{c.RATIO}",
                    Totalends = c.PL_PRODUCTION_SETDISTRIBUTION.FirstOrDefault()?.PROG_.BLK_PROG_.RndProductionOrder.TOTAL_ENOS,
                    Pino = $"{c.PL_PRODUCTION_SETDISTRIBUTION.Select(d => d.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.PINO).FirstOrDefault()}",
                    Endsrope = $"{(c.PL_PRODUCTION_SETDISTRIBUTION.FirstOrDefault()?.PROG_.BLK_PROG_.RndProductionOrder.TOTAL_ENOS / double.Parse(c.F_PR_WARPING_PROCESS_ROPE_MASTER.FirstOrDefault()?.F_PR_WARPING_PROCESS_ROPE_DETAILS.FirstOrDefault()?.BALL_NO ?? "0"))}",
                    Dyeinglength = c.GROUP.F_DYEING_PROCESS_ROPE_MASTER.FirstOrDefault()?.DYEING_LENGTH,
                    Grouplength = c.GROUP.F_DYEING_PROCESS_ROPE_MASTER.FirstOrDefault()?.GROUP_LENGTH,
                    Setlength = c.F_PR_WARPING_PROCESS_ROPE_MASTER.Select(d => d.WARP_LENGTH).FirstOrDefault(),
                    BallQty = $"{c.F_PR_WARPING_PROCESS_ROPE_MASTER.FirstOrDefault()?.F_PR_WARPING_PROCESS_ROPE_DETAILS.FirstOrDefault()?.BALL_NO}",
                    //Setratiolength = (c.GROUP.F_DYEING_PROCESS_ROPE_MASTER.FirstOrDefault().DYEING_LENGTH * c.GROUP.F_DYEING_PROCESS_ROPE_MASTER.FirstOrDefault().GROUP_LENGTH) / c.GROUP.F_DYEING_PROCESS_ROPE_MASTER.FirstOrDefault().GROUP_LENGTH,
                    Canlist = c.F_DYEING_PROCESS_ROPE_DETAILS.Select(d => new
                    {
                        Ropeid = $"{d.ROPEID}",
                        Canno = $"{d.CAN_NONavigation.TUBE_NO}"
                    })



                    //RndFabricCountInfoViewModels = c.PL_PRODUCTION_SETDISTRIBUTION.FirstOrDefault().PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO
                    //   .Select(e => new RndFabricCountInfoViewModel
                    //   {
                    //       RndFabricCountinfo = e,
                    //       AMOUNT = e.FABCODENavigation.RND_YARNCONSUMPTION.Where(d => d.FABCODE.Equals(e.FABCODE) && d.COUNTID.Equals(e.COUNTID) && d.YARNFOR.Equals(e.YARNFOR)).Select(d=>d.AMOUNT).FirstOrDefault()

                    //       //c.PL_PRODUCTION_SETDISTRIBUTION.Select(m=>m.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(d=>d.FABCODE.Equals(e.FABCODE) && d.COUNTID.Equals(e.COUNTID) && d.YARNFOR.Equals(e.YARNFOR)).AMOUNT).FirstOrDefault()
                    //   }).ToList()
                }).FirstOrDefault();


                return firstOrDefault;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FLcbProductionRopeViewModel> FindAllByIdAsync(int lcbId)
        {
            try
            {
                var result = DenimDbContext.F_LCB_PRODUCTION_ROPE_MASTER
                    .Include(c => c.F_LCB_PRODUCTION_ROPE_DETAILS)
                    .ThenInclude(c => c.EMPLOYEE)
                    .Include(c => c.F_LCB_PRODUCTION_ROPE_DETAILS)
                    .ThenInclude(c => c.CAN.CAN_NONavigation)
                    .Include(c => c.F_LCB_PRODUCTION_ROPE_DETAILS)
                    .ThenInclude(c => c.F_LCB_PRODUCTION_ROPE_PROCESS_INFO)
                    .ThenInclude(c => c.BEAM)
                    .Include(c => c.F_LCB_PRODUCTION_ROPE_DETAILS)
                    .ThenInclude(c => c.F_LCB_PRODUCTION_ROPE_PROCESS_INFO)
                    .ThenInclude(c => c.MACHINE)
                    .Where(c => c.LCBPROID.Equals(lcbId))
                    .Select(c => new FLcbProductionRopeViewModel
                    {
                        FLcbProductionRopeMaster = c,
                        FLcbProductionRopeDetailsList = c.F_LCB_PRODUCTION_ROPE_DETAILS.Select(d => new F_LCB_PRODUCTION_ROPE_DETAILS
                        {
                            LCB_D_ID = d.LCB_D_ID,
                            TRANSDATE = d.TRANSDATE,
                            CAN = new F_DYEING_PROCESS_ROPE_DETAILS
                            {
                                CAN_NONavigation = new F_PR_TUBE_INFO
                                {
                                    TUBE_NO = d.CAN.CAN_NONavigation.TUBE_NO
                                }
                            },
                            CANID = d.CANID,
                            ENDS = d.ENDS,
                            SHIFT = d.SHIFT,
                            TIME = d.TIME,
                            EMPLOYEE = new F_HRD_EMPLOYEE
                            {
                                FIRST_NAME = d.EMPLOYEE.FIRST_NAME
                            },
                            REMARKS = d.REMARKS,
                            FLcbProductionRopeProcessInfoList = d.F_LCB_PRODUCTION_ROPE_PROCESS_INFO.Select(m => new F_LCB_PRODUCTION_ROPE_PROCESS_INFO
                            {
                                MACHINEID = m.MACHINEID,
                                MACHINE = new F_LCB_MACHINE
                                {
                                    MACHINE_NO = m.MACHINE.MACHINE_NO
                                },
                                BEAM = new F_LCB_BEAM
                                {
                                    BEAM_NO = m.BEAM.BEAM_NO
                                },
                                LCB_P_ID = m.LCB_P_ID,
                                LENGTH = m.LENGTH,
                                TENS = m.TENS,
                                BREAK = m.BREAK,
                                KNOT = m.KNOT,
                                REMARKS = m.REMARKS
                            }).ToList()
                        }).ToList()
                    })
                    .FirstOrDefault();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<LCBChartDataViewModel> GetLCBDateWiseLengthGraph()
        {
            try
            {
                var lcbChartData = new LCBChartDataViewModel();

                lcbChartData.LCBData = await DenimDbContext.F_LCB_PRODUCTION_ROPE_MASTER
                    .Where(c => c.TRANSDATE.Equals(Convert.ToDateTime("2022-04-26 00:00:00.000").Date))
                    .Select(d => new F_LCB_PRODUCTION_ROPE_MASTER()
                    {
                        LCB_LENGTH = d.LCB_LENGTH
                    }).SumAsync(c => c.LCB_LENGTH);

                //lcbChartData.ReconData = await DenimDbContext.F_PR_RECONE_MASTER
                //    .Where(c => c.TRANSDATE.Equals(Convert.ToDateTime("2022-04-26 00:00:00.000").Date))
                //    .Select(d => new F_PR_RECONE_MASTER()
                //    {
                //        WARP_LENGTH = d.WARP_LENGTH
                //    }).SumAsync(c => Convert.ToDouble(c.WARP_LENGTH ?? 0));

                return lcbChartData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<LCBChartDataViewModel> GetLCBProductionData()
        {
            try
            {
                var date = Convert.ToDateTime("2022-02-27");
                var defaultDate = Convert.ToDateTime("1501-01-01 00:00:00.000");
                var lcbChartData = new LCBChartDataViewModel();

                lcbChartData.LcbCompleteLength = await DenimDbContext.F_LCB_PRODUCTION_ROPE_MASTER
                    .Where(c => c.TRANSDATE.Equals(Convert.ToDateTime("2022-04-26 00:00:00.000").Date))
                    .Select(d => new F_LCB_PRODUCTION_ROPE_MASTER
                    {
                        LCB_LENGTH = d.LCB_LENGTH ?? 0
                    }).SumAsync(c => c.LCB_LENGTH);

                lcbChartData.ComparisonMonthlyLCB = await DenimDbContext.F_LCB_PRODUCTION_ROPE_MASTER
                    .Where(c => (c.TRANSDATE ?? defaultDate).ToString("yyyy-MM").Equals(date.ToString("yyyy-MM")))
                    .Select(d => new F_LCB_PRODUCTION_ROPE_MASTER
                    {
                        LCB_LENGTH = d.LCB_LENGTH ?? 0
                    }).SumAsync(c => c.LCB_LENGTH) - await DenimDbContext.F_LCB_PRODUCTION_ROPE_MASTER
                    .Where(c => (c.TRANSDATE ?? defaultDate).ToString("yyyy-MM")
                        .Equals(date.AddMonths(-1).ToString("yyyy-MM")))
                    .Select(d => new F_LCB_PRODUCTION_ROPE_MASTER
                    {
                        LCB_LENGTH = d.LCB_LENGTH ?? 0
                    }).SumAsync(c => c.LCB_LENGTH);


                lcbChartData.TotalProduction = await DenimDbContext.F_LCB_PRODUCTION_ROPE_MASTER
                    .Select(d => new F_LCB_PRODUCTION_ROPE_MASTER
                    {
                        LCB_LENGTH = d.LCB_LENGTH ?? 0
                    }).SumAsync(c => c.LCB_LENGTH);

                lcbChartData.MonthlyProduction = await DenimDbContext.F_LCB_PRODUCTION_ROPE_MASTER
                    .Where(c => (c.TRANSDATE ?? defaultDate).ToString("yyyy-MM").Equals(date.ToString("yyyy-MM")))
                    .Select(d => new F_LCB_PRODUCTION_ROPE_MASTER
                    {
                        LCB_LENGTH = d.LCB_LENGTH ?? 0
                    }).SumAsync(c => c.LCB_LENGTH);

                lcbChartData.DailyProduction = await DenimDbContext.F_LCB_PRODUCTION_ROPE_MASTER
                    .Where(c => (c.TRANSDATE ?? defaultDate).Date.Equals(date.Date))
                    .Select(d => new F_LCB_PRODUCTION_ROPE_MASTER
                    {
                        LCB_LENGTH = d.LCB_LENGTH ?? 0
                    }).SumAsync(c => c.LCB_LENGTH);

                var totalRopeWarpingSet = await DenimDbContext.F_PR_WARPING_PROCESS_ROPE_DETAILS
                    .Select(d => new F_PR_WARPING_PROCESS_ROPE_DETAILS
                    {
                        SETID = d.SETID ?? 0
                    }).CountAsync();

                var completeSets = await DenimDbContext.F_LCB_PRODUCTION_ROPE_MASTER
                    .Include(c => c.SUBGROUP)
                    .ThenInclude(s => s.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(d => d.SETID)
                    .Select(c => c.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION.Select(d => d.SETID))
                    .SelectMany(setIds => setIds)
                    .Distinct()
                    .CountAsync();



                var pendingSets = totalRopeWarpingSet - completeSets;

                lcbChartData.CompleteSets = completeSets;
                lcbChartData.PendingSets = pendingSets;


                return lcbChartData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<LCBChartDataViewModel>> GetLCBProductionList()
        {
            try

            {
                var data = new List<LCBChartDataViewModel>();
                var date = Convert.ToDateTime("2022-04-26").AddDays(-30);
                var defaultDate = Convert.ToDateTime("1501-01-01 00:00:00.000");


                for (var i = 0; i < 30; i++)
                {
                    data.Add(new LCBChartDataViewModel
                    {
                        date = date.AddDays(i),

                        LcbCompleteLength = await DenimDbContext.F_LCB_PRODUCTION_ROPE_MASTER
                            .Where(c => (c.TRANSDATE ?? defaultDate).Date.Equals(date.AddDays(i).Date))
                            .Select(d => new F_LCB_PRODUCTION_ROPE_MASTER
                            {
                                LCB_LENGTH = d.LCB_LENGTH ?? 0
                            }).SumAsync(c => c.LCB_LENGTH)


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

        public async Task<LCBChartDataViewModel> GetMonthlyLCBPendingAndCompleteSets()
        {
            var date = Convert.ToDateTime("2022-04-26");
            var defaultDate = Convert.ToDateTime("1501-01-01 00:00:00.000");
            var lcbChartDataViewModel = new LCBChartDataViewModel();
            var totalSet = await DenimDbContext.F_PR_WARPING_PROCESS_ROPE_DETAILS.Select(c => c.SETID).CountAsync();

            //var mPendingSets = await DenimDbContext.PL_PRODUCTION_PLAN_DETAILS
            //    .Where(c => (c.SUBGROUPDATE ?? defaultDate).Date.Equals(date.Date))
            //    .CountAsync(c =>
            //        !DenimDbContext.F_LCB_PRODUCTION_ROPE_MASTER.Any(e => e.SUBGROUPID.Equals(c.SUBGROUPID)));

            

            //var mCompeteSets = await DenimDbContext.PL_PRODUCTION_PLAN_DETAILS  
            //    .Where(c => (c.SUBGROUPDATE ?? defaultDate).Date.Equals(date.Date))
            //    .CountAsync(c =>
            //        DenimDbContext.F_LCB_PRODUCTION_ROPE_MASTER.Any(e => e.SUBGROUPID.Equals(c.SUBGROUPID)));

            



            var mCompeteSets = await DenimDbContext.F_LCB_PRODUCTION_ROPE_MASTER
                .Include(c => c.SUBGROUP)
                .ThenInclude(s => s.PL_PRODUCTION_SETDISTRIBUTION)
                .ThenInclude(d => d.SETID)
                .Where(c => (c.TRANSDATE ?? defaultDate).Month.Equals(date.Month)) /*ToString("yyyy-MM")*/
                .Select(c => c.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION.Select(d => d.SETID))
                .SelectMany(setIds => setIds)
                .Distinct()
                .CountAsync();

            var mCompletePercent = (mCompeteSets / Convert.ToSingle(totalSet)) * 100;
            var mCompletePercentR = Math.Round(mCompletePercent, 1);

            var mPendingSets = totalSet - mCompeteSets;
            var mPendingPercent = (mPendingSets / Convert.ToSingle(totalSet)) * 100;
            var mPendingPercentR = Math.Round(mPendingPercent, 1);



            lcbChartDataViewModel.MonthlyCompletePercent = mCompletePercentR;
            lcbChartDataViewModel.MonthlyPendingPercent = mPendingPercentR;
            lcbChartDataViewModel.MonthlyLCBCompleteSets = mCompeteSets;
            lcbChartDataViewModel.MonthlyLCBPendingSets = mPendingSets;

            return lcbChartDataViewModel;
        }

        public async Task<List<PL_PRODUCTION_SETDISTRIBUTION>> GeLcbPendingSetList()
        {
            var result = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Where(c => !DenimDbContext.F_LCB_PRODUCTION_ROPE_MASTER.Include(d => d.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION).Any(d => d.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION.Select(e => e.SETID).Equals(c.SETID)))
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

        private static decimal Fraction(string f)
        {
            var numbers = f.Split(' ', '+');
            decimal temp, result = 0.0m;
            decimal numerator, denominator;
            foreach (var str in numbers)
            {
                if (decimal.TryParse(str, out temp))
                {
                    result += temp;
                }
                else if (str.Contains("/"))
                {
                    var frac = str.Split('/');
                    decimal.TryParse(frac[0], out numerator);
                    decimal.TryParse(frac[1], out denominator);

                    result += numerator / denominator;
                }
            }

            return result;
        }
    }
}
