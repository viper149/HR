using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Rnd;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_FS_FABRIC_CLEARENCE_2ND_BEAM_Repository : BaseRepository<F_FS_FABRIC_CLEARENCE_2ND_BEAM>, IF_FS_FABRIC_CLEARENCE_2ND_BEAM
    {
        public SQLF_FS_FABRIC_CLEARENCE_2ND_BEAM_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<List<F_FS_FABRIC_CLEARENCE_2ND_BEAM>> GetAllAsync()
        {
            try
            {
                var result = await DenimDbContext.F_FS_FABRIC_CLEARENCE_2ND_BEAM
                    .Include(c => c.BEAM)
                    .Include(c => c.EMP)
                    .Include(c => c.LBTEST_)
                    .Include(c => c.LGTEST_)
                    .Include(c => c.ORDERNONavigation.SO.STYLE.FABCODENavigation)
                    .Include(c => c.ORDERNONavigation.RS)
                    .Include(c => c.TT)
                    .Include(c => c.SET.PROG_)
                    .Include(c => c.TROLLY_NONavigation)
                    .Select(c => new F_FS_FABRIC_CLEARENCE_2ND_BEAM
                    {
                        TRIAL_NO = c.TRIAL_NO,
                        AID = c.AID,
                        OPT1 = c.EMP == null ? "" : c.EMP.FIRST_NAME,
                        OPT2 = c.TT == null ? "" : c.TT.NAME,
                        OPT3 = c.ORDERNONavigation != null ? c.ORDERNONavigation.SO != null ? c.ORDERNONavigation.SO.SO_NO : c.ORDERNONavigation.RS != null ? c.ORDERNONavigation.RS.DYEINGCODE : "" : "",
                        OPT4 = c.SET == null ? "" : c.SET.PROG_ == null ? "" : c.SET.PROG_.PROG_NO,
                        OPT5 = c.LGTEST_ == null ? "" : c.LGTEST_.LAB_NO,
                        OPT6 = c.LBTEST_ == null ? "" : c.LBTEST_.LAB_NO,
                        OPT7 = c.ORDERNONavigation.SO.STYLE.FABCODENavigation.STYLE_NAME,
                        QUALITY_COMMENTS = c.QUALITY_COMMENTS,

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

        public async Task<FFsFabricClearance2ndBeamViewModel> GetInitData(FFsFabricClearance2ndBeamViewModel fFsFabricClearance2NdBeamViewModel)
        {
            try
            {
                fFsFabricClearance2NdBeamViewModel.BEAMList = await DenimDbContext.F_PR_WEAVING_PROCESS_DETAILS_B
                    .Include(c => c.WV_BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM)
                    .Include(c => c.WV_BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM)
                    .Include(c => c.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM)
                    .Where(c => c.OTHERS_DOFF.Equals(3))
                    .Select(c => new F_PR_WEAVING_PROCESS_DETAILS_B
                    {
                        TRNSID = c.TRNSID,
                        OPT1 = c.WV_BEAM == null ? "" : c.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS != null ? c.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM.BEAM_NO : c.WV_BEAM.F_PR_SLASHER_DYEING_DETAILS != null ? c.WV_BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM.BEAM_NO : "N/A"
                    }).ToListAsync();

                fFsFabricClearance2NdBeamViewModel.EMPList = await DenimDbContext.F_HRD_EMPLOYEE.Select(c => new F_HRD_EMPLOYEE
                {
                    EMPID = c.EMPID,
                    FIRST_NAME = $"{c.EMPNO} - {c.FIRST_NAME}"
                }).ToListAsync();

                fFsFabricClearance2NdBeamViewModel.LBTESTList = await DenimDbContext.RND_FABTEST_BULK.ToListAsync();
                fFsFabricClearance2NdBeamViewModel.LGTESTList = await DenimDbContext.RND_FABTEST_GREY.ToListAsync();

                fFsFabricClearance2NdBeamViewModel.ORDERNONavigationList = await DenimDbContext.RND_PRODUCTION_ORDER
                    .Include(c => c.SO)
                    .Include(c => c.RS)
                    .Select(c => new RND_PRODUCTION_ORDER
                    {
                        POID = c.POID,
                        ORDERNO = c.ORDERNO,
                        OPT1 = c.SO != null ? c.SO.SO_NO : c.RS != null ? c.RS.DYEINGCODE : "N/A"
                    }).OrderByDescending(e => e.OPT1).ToListAsync();

                fFsFabricClearance2NdBeamViewModel.SETList = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Include(c => c.PROG_)
                    .Select(c => new PL_PRODUCTION_SETDISTRIBUTION
                    {
                        SETID = c.SETID,
                        PROG_ = new PL_BULK_PROG_SETUP_D
                        {
                            PROG_NO = c.PROG_ != null ? c.PROG_.PROG_NO : "N/A"
                        }
                    }).ToListAsync();
                fFsFabricClearance2NdBeamViewModel.TROLLYNONavigationList = await DenimDbContext.F_PR_FIN_TROLLY.ToListAsync();

                fFsFabricClearance2NdBeamViewModel.FinishMachineList = await DenimDbContext.F_PR_FN_MACHINE_INFO
                    .Select(c => new TypeTableViewModel
                    {
                        ID = c.FN_MACHINEID,
                        Name = c.NAME
                    }).ToListAsync();

                fFsFabricClearance2NdBeamViewModel.TTList = await DenimDbContext.F_FS_FABRIC_TYPE.ToListAsync();

                return fFsFabricClearance2NdBeamViewModel;
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
                var rndProductionOrderDetailViewModel = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
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
                    .ThenInclude(e => e.WV_BEAM)
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation)
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.LOOM)
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FINISHMC)
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c => c.COUNT)
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER)
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BRAND)
                    .Where(e => e.SETID.Equals(setId))
                    .Select(e => new RndProductionOrderDetailViewModel
                    {
                        PlProductionSetDistribution = new PL_PRODUCTION_SETDISTRIBUTION
                        {
                            SETID = e.SETID,
                            OPT3 = e.OPT3,
                            PROG_ = new PL_BULK_PROG_SETUP_D
                            {
                                SET_QTY = e.PROG_.SET_QTY,
                                BLK_PROG_ = new PL_BULK_PROG_SETUP_M
                                {
                                    RndProductionOrder = new RND_PRODUCTION_ORDER
                                    {
                                        SO = new COM_EX_PI_DETAILS
                                        {
                                            STYLE = new COM_EX_FABSTYLE
                                            {
                                                FABCODENavigation = new RND_FABRICINFO
                                                {
                                                    STYLE_NAME = e.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME,
                                                    WIGRBW = e.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.WIGRBW,
                                                    WGGRBW = e.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.WGGRBW,
                                                    SRGRWARP = e.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.SRGRWARP,
                                                    SRGRWEFT = e.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.SRGRWEFT,
                                                    STGRWARP = e.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STGRWARP,
                                                    STGRWEFT = e.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STGRWEFT,
                                                    FINISH_ROUTE = e.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FINISH_ROUTE,
                                                    FNEPI = e.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FNEPI,
                                                    FNPPI = e.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FNPPI,
                                                    WIDEC = e.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.WIDEC,
                                                    WGDEC = e.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.WGDEC,
                                                    SRDECWARP = e.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.SRDECWARP,
                                                    SRDECWEFT = e.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.SRDECWEFT,
                                                    DENT = e.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.DENT,
                                                    REED_COUNT = e.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.REED_COUNT,
                                                    TOTALENDS = e.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.TOTALENDS,
                                                    RND_FINISHMC = new RND_FINISHMC
                                                    {
                                                        NAME = e.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FINISHMC.NAME
                                                    },
                                                    LOOM = new LOOM_TYPE
                                                    {
                                                        LOOM_TYPE_NAME = e.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.LOOM.LOOM_TYPE_NAME
                                                    }
                                                }
                                            },
                                            PIMASTER = new COM_EX_PIMASTER
                                            {
                                                PINO = e.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.PINO,
                                                PI_QTY = e.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.PI_QTY,
                                                BUYER = new BAS_BUYERINFO
                                                {
                                                    BUYER_NAME = e.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER.BUYER_NAME
                                                },
                                                BRAND = new BAS_BRANDINFO
                                                {
                                                    BRANDNAME = e.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BRAND.BRANDNAME
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            SUBGROUP = new PL_PRODUCTION_PLAN_DETAILS
                            {
                                GROUP = new PL_PRODUCTION_PLAN_MASTER
                                {
                                    DYEING_REFERANCE = e.SUBGROUP.GROUP.DYEING_REFERANCE,
                                    RND_DYEING_TYPE = new RND_DYEING_TYPE
                                    {
                                        DTYPE = e.SUBGROUP.GROUP.RND_DYEING_TYPE.DTYPE
                                    },
                                    F_DYEING_PROCESS_ROPE_MASTER = e.SUBGROUP.GROUP.F_DYEING_PROCESS_ROPE_MASTER.Select(f => new F_DYEING_PROCESS_ROPE_MASTER
                                    {
                                        GROUP_LENGTH = f.GROUP_LENGTH
                                    }).ToList()
                                }
                            },
                            F_PR_WEAVING_PROCESS_MASTER_B = e.F_PR_WEAVING_PROCESS_MASTER_B.Select(f => new F_PR_WEAVING_PROCESS_MASTER_B
                            {
                                F_PR_WEAVING_PROCESS_BEAM_DETAILS_B = f.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B.Select(g => new F_PR_WEAVING_PROCESS_BEAM_DETAILS_B
                                {

                                    F_PR_SIZING_PROCESS_ROPE_DETAILS = g.F_PR_SIZING_PROCESS_ROPE_DETAILS==null? g.F_PR_SIZING_PROCESS_ROPE_DETAILS: new F_PR_SIZING_PROCESS_ROPE_DETAILS
                                    {
                                        OPT1 = $"{g.F_PR_SIZING_PROCESS_ROPE_DETAILS.OPT1}",
                                        SDID = g.F_PR_SIZING_PROCESS_ROPE_DETAILS.SDID,
                                        W_BEAM = new F_WEAVING_BEAM
                                        {
                                            BEAM_NO = $"{g.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM.BEAM_NO}"
                                        }
                                    },
                                    F_PR_SLASHER_DYEING_DETAILS = g.F_PR_SLASHER_DYEING_DETAILS == null ? g.F_PR_SLASHER_DYEING_DETAILS : new F_PR_SLASHER_DYEING_DETAILS
                                    {
                                        OPT1 = $"{g.F_PR_SLASHER_DYEING_DETAILS.OPT1}",
                                        SLDID = g.F_PR_SLASHER_DYEING_DETAILS.SLDID,
                                        W_BEAM = new F_WEAVING_BEAM
                                        {
                                            BEAM_NO = $"{g.F_PR_SLASHER_DYEING_DETAILS.W_BEAM.BEAM_NO}"
                                        }
                                    },
                                    F_PR_WEAVING_PROCESS_DETAILS_B = g.F_PR_WEAVING_PROCESS_DETAILS_B.Select(h => new F_PR_WEAVING_PROCESS_DETAILS_B
                                    {
                                        TRNSID = h.TRNSID,
                                        OTHERS_DOFF = h.OTHERS_DOFF,
                                        LENGTH_BULK = h.LENGTH_BULK,
                                        WV_BEAM = new F_PR_WEAVING_PROCESS_BEAM_DETAILS_B
                                        {
                                            WV_BEAMID = h.WV_BEAM.WV_BEAMID,
                                            BEAMID = h.WV_BEAM.BEAMID,
                                            SBEAMID = h.WV_BEAM.SBEAMID
                                        }
                                    }).ToList()
                                }).ToList(),
                            }).ToList(),

                            //F_PR_SIZING_PROCESS_ROPE_MASTER = e.F_PR_SIZING_PROCESS_ROPE_MASTER.Select(f => new F_PR_SIZING_PROCESS_ROPE_MASTER
                            //{
                            //    F_PR_SIZING_PROCESS_ROPE_DETAILS = f.F_PR_SIZING_PROCESS_ROPE_DETAILS.Select(g => new F_PR_SIZING_PROCESS_ROPE_DETAILS
                            //    {
                            //        OPT1 = g.OPT1,
                            //        W_BEAM = new F_WEAVING_BEAM
                            //        {
                            //            BEAM_NO = g.W_BEAM.BEAM_NO
                            //        }
                            //    }).ToList()
                            //}).ToList()
                            F_PR_SIZING_PROCESS_ROPE_MASTER = e.F_PR_SIZING_PROCESS_ROPE_MASTER.Select(e => new F_PR_SIZING_PROCESS_ROPE_MASTER
                            {
                                F_PR_SIZING_PROCESS_ROPE_DETAILS = e.F_PR_SIZING_PROCESS_ROPE_DETAILS.ToList()
                            }).ToList(),
                            F_PR_SLASHER_DYEING_MASTER = e.F_PR_SLASHER_DYEING_MASTER.Select(e => new F_PR_SLASHER_DYEING_MASTER
                            {
                                F_PR_SLASHER_DYEING_DETAILS = e.F_PR_SLASHER_DYEING_DETAILS.ToList()
                            }).ToList()

                        }
                    }).FirstOrDefaultAsync();

                rndProductionOrderDetailViewModel.PlProductionSetDistribution.OPT3 = (
                    (rndProductionOrderDetailViewModel.PlProductionSetDistribution.PROG_.SET_QTY * rndProductionOrderDetailViewModel.PlProductionSetDistribution.SUBGROUP
                        .GROUP.F_DYEING_PROCESS_ROPE_MASTER.FirstOrDefault()
                        ?.DYEING_LENGTH) /
                    rndProductionOrderDetailViewModel.PlProductionSetDistribution.SUBGROUP.GROUP.F_DYEING_PROCESS_ROPE_MASTER.FirstOrDefault()?.GROUP_LENGTH).ToString();

                if (rndProductionOrderDetailViewModel.PlProductionSetDistribution.F_PR_SIZING_PROCESS_ROPE_MASTER.Count != 0)
                {
                    var beamList = rndProductionOrderDetailViewModel.PlProductionSetDistribution.F_PR_SIZING_PROCESS_ROPE_MASTER
                        .SelectMany(c => c.F_PR_SIZING_PROCESS_ROPE_DETAILS).ToList();


                    var index = beamList
                        .OrderBy(r => r.SDID)
                        .Select((r, i) => new { index = i + 1, r.SDID });

                    foreach (var item in index)
                    {
                        rndProductionOrderDetailViewModel.PlProductionSetDistribution
                            .F_PR_SIZING_PROCESS_ROPE_MASTER
                            .SelectMany(c => c.F_PR_SIZING_PROCESS_ROPE_DETAILS).FirstOrDefault(d => d.SDID == item.SDID).OPT1 = item.index.ToString();


                        if (rndProductionOrderDetailViewModel.PlProductionSetDistribution.F_PR_WEAVING_PROCESS_MASTER_B
                            .Any(c => c.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B.Any(d =>
                                d.F_PR_SIZING_PROCESS_ROPE_DETAILS.SDID == item.SDID))
                        )
                        {
                            rndProductionOrderDetailViewModel.PlProductionSetDistribution.F_PR_WEAVING_PROCESS_MASTER_B
                                .SelectMany(c => c.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B)
                                .FirstOrDefault(d => d.F_PR_SIZING_PROCESS_ROPE_DETAILS.SDID == item.SDID).F_PR_SIZING_PROCESS_ROPE_DETAILS.OPT1 = item.index.ToString();
                        }
                    }
                }


                if (rndProductionOrderDetailViewModel.PlProductionSetDistribution.F_PR_SLASHER_DYEING_MASTER.Count != 0)
                {
                    var beamList = rndProductionOrderDetailViewModel.PlProductionSetDistribution.F_PR_SLASHER_DYEING_MASTER
                        .SelectMany(c => c.F_PR_SLASHER_DYEING_DETAILS).ToList();


                    var index = beamList
                        .OrderBy(r => r.SLDID)
                        .Select((r, i) => new { index = i + 1, r.SLDID });

                    foreach (var item in index)
                    {
                        rndProductionOrderDetailViewModel.PlProductionSetDistribution
                            .F_PR_SLASHER_DYEING_MASTER
                            .SelectMany(c => c.F_PR_SLASHER_DYEING_DETAILS).First(d => d.SLDID == item.SLDID).OPT1 = item.index.ToString();


                        if (rndProductionOrderDetailViewModel.PlProductionSetDistribution.F_PR_WEAVING_PROCESS_MASTER_B
                            .Any(c => c.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B.Any(d =>
                                d.F_PR_SLASHER_DYEING_DETAILS.SLDID == item.SLDID))
                        )
                        {
                            rndProductionOrderDetailViewModel.PlProductionSetDistribution.F_PR_WEAVING_PROCESS_MASTER_B
                                .SelectMany(c => c.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B)
                                .FirstOrDefault(d => d.F_PR_SLASHER_DYEING_DETAILS.SLDID == item.SLDID).F_PR_SLASHER_DYEING_DETAILS.OPT1 = item.index.ToString();
                        }


                    }
                }

                //foreach (var item in rndProductionOrderDetailViewModel.PlProductionSetDistribution.F_PR_WEAVING_PROCESS_MASTER_B.SelectMany(c=>c.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B))
                //{

                //    var x = rndProductionOrderDetailViewModel.PlProductionSetDistribution.F_PR_SIZING_PROCESS_ROPE_MASTER;
                //     var a= item.OPT1;



                //     if (rndProductionOrderDetailViewModel.PlProductionSetDistribution.F_PR_SLASHER_DYEING_MASTER.Count != 0)
                //     {
                //    }

                return rndProductionOrderDetailViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
