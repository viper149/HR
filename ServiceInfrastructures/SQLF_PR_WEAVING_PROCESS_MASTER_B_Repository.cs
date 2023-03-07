using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Factory.Production;
using DenimERP.ViewModels.Rnd;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_WEAVING_PROCESS_MASTER_B_Repository : BaseRepository<F_PR_WEAVING_PROCESS_MASTER_B>, IF_PR_WEAVING_PROCESS_MASTER_B
    {
        private readonly IDataProtector _protector;

        public SQLF_PR_WEAVING_PROCESS_MASTER_B_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }


        public async Task<IEnumerable<F_PR_WEAVING_PROCESS_MASTER_B>> GetAllAsync()
        {
            try
            {
                var result =  await DenimDbContext.F_PR_WEAVING_PROCESS_MASTER_B
                    .Include(c => c.SET)
                    .ThenInclude(c => c.PROG_)
                    .Select(x => new F_PR_WEAVING_PROCESS_MASTER_B
                    {
                        EncryptedId = _protector.Protect(x.WV_PROCESSID.ToString()),
                        WV_PROCESSID = x.WV_PROCESSID,
                        WV_PROCESS_DATE = x.WV_PROCESS_DATE,
                        REMARKS = x.REMARKS,
                        SET = new PL_PRODUCTION_SETDISTRIBUTION()
                        {

                            PROG_ = new PL_BULK_PROG_SETUP_D()
                            {
                                PROG_NO = x.SET.PROG_.PROG_NO,
                                BLK_PROG_ = new PL_BULK_PROG_SETUP_M()
                                {
                                    RndProductionOrder = new RND_PRODUCTION_ORDER()
                                    {
                                        SO = new COM_EX_PI_DETAILS()
                                        {
                                            SO_NO = x.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.SO_NO,
                                            STYLE = new COM_EX_FABSTYLE()
                                            {
                                                //STYLENAME = x.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.STYLENAME,
                                                FABCODENavigation = new RND_FABRICINFO()
                                                {
                                                    STYLE_NAME = x.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME
                                                }
                                                
                                            }
                                        },
                                        RS = new RND_SAMPLE_INFO_DYEING()
                                        {
                                            RSOrder = x.SET.PROG_.BLK_PROG_.RndProductionOrder.RS.RSOrder,
                                            DYEING_REF = x.SET.PROG_.BLK_PROG_.RndProductionOrder.DYENG_TYPE
                                            
                                        }
                                    }
                                }
                            }
                        }
                    }).OrderByDescending(x=>x.WV_PROCESSID).ToListAsync();
                return result;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<PrWeavingProcessBulkViewModel> GetInitObjects(PrWeavingProcessBulkViewModel prWeavingProcessBulkViewModel)
        {
            try
            {

                if (prWeavingProcessBulkViewModel.FPrWeavingProcessMasterB != null)
                {
                    prWeavingProcessBulkViewModel.PlProductionSetDistributions = await DenimDbContext
                        .PL_PRODUCTION_SETDISTRIBUTION
                        .Include(c => c.PROG_)
                        .Where(c => DenimDbContext.F_PR_WEAVING_PROCESS_MASTER_B.Any(f => f.SETID.Equals(c.SETID)))
                        .Select(c => new PL_PRODUCTION_SETDISTRIBUTION
                        {
                            SETID = c.SETID,
                            PROG_ = c.PROG_
                        }).ToListAsync();
                    //prWeavingProcessBulkViewModel.PlProductionSetDistributions = await _denimDbContext
                    //    .PL_PRODUCTION_SETDISTRIBUTION
                    //    .Include(c => c.PROG_)
                    //    .Where(c => _denimDbContext.F_PR_WEAVING_BEAM_RECEIVING.Any(e => e.SETID.Equals(c.SETID)) && _denimDbContext.F_PR_WEAVING_PROCESS_MASTER_B.Any(f => f.SETID.Equals(c.SETID)))
                    //    .Select(c => new PL_PRODUCTION_SETDISTRIBUTION
                    //    {
                    //        SETID = c.SETID,
                    //        PROG_ = c.PROG_
                    //    }).ToListAsync();
                }
                else
                {
                    prWeavingProcessBulkViewModel.PlProductionSetDistributions = await DenimDbContext
                        .PL_PRODUCTION_SETDISTRIBUTION
                        .Include(c => c.PROG_)
                        .Where(c => (DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER.Any(e => e.SETID.Equals(c.SETID)) || DenimDbContext.F_PR_WARPING_PROCESS_DW_MASTER.Any(e => e.SETID.Equals(c.SETID))) && !DenimDbContext.F_PR_WEAVING_PROCESS_MASTER_B.Any(f => f.SETID.Equals(c.SETID)))
                        .Select(c => new PL_PRODUCTION_SETDISTRIBUTION
                        {
                            SETID = c.SETID,
                            PROG_ = c.PROG_
                        }).ToListAsync();
                    //prWeavingProcessBulkViewModel.PlProductionSetDistributions = await _denimDbContext
                    //    .PL_PRODUCTION_SETDISTRIBUTION
                    //    .Include(c => c.PROG_)
                    //    .Where(c => _denimDbContext.F_PR_WEAVING_BEAM_RECEIVING.Any(e => e.SETID.Equals(c.SETID)) && !_denimDbContext.F_PR_WEAVING_PROCESS_MASTER_B.Any(f => f.SETID.Equals(c.SETID)))
                    //    .Select(c => new PL_PRODUCTION_SETDISTRIBUTION
                    //    {
                    //        SETID = c.SETID,
                    //        PROG_ = c.PROG_
                    //    }).ToListAsync();
                }



                var FHrEmployees = await DenimDbContext.F_HR_EMP_OFFICIALINFO
                    .Include(c => c.EMP)
                    .Where(c => c.SECID.Equals(165) && !c.OPN2.Equals("Y"))
                    .ToListAsync();

                prWeavingProcessBulkViewModel.FHrEmployees = FHrEmployees.Select(c => new F_HRD_EMPLOYEE
                {
                    EMPID = c.EMP.EMPID,
                    FIRST_NAME = c.EMP.FIRST_NAME + " " + c.EMP.LAST_NAME + '-' + c.EMP.EMPNO
                }).ToList();

                prWeavingProcessBulkViewModel.FWeavingBeams = await DenimDbContext.F_WEAVING_BEAM.ToListAsync();
                prWeavingProcessBulkViewModel.FLoomMachineNo = await DenimDbContext.F_LOOM_MACHINE_NO.ToListAsync();
                prWeavingProcessBulkViewModel.OtherDoffs = await DenimDbContext.F_PR_WEAVING_OTHER_DOFF.ToListAsync();
                prWeavingProcessBulkViewModel.LoomTypes = await DenimDbContext.LOOM_TYPE.ToListAsync();
                prWeavingProcessBulkViewModel.Weavings = await DenimDbContext.RND_SAMPLE_INFO_WEAVING.ToListAsync();

                return prWeavingProcessBulkViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> InsertAndGetIdAsync(F_PR_WEAVING_PROCESS_MASTER_B fPrWeavingProcessMaster)
        {
            try
            {
                await DenimDbContext.F_PR_WEAVING_PROCESS_MASTER_B.AddAsync(fPrWeavingProcessMaster);
                await SaveChanges();
                return fPrWeavingProcessMaster.WV_PROCESSID;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<PrWeavingProcessBulkViewModel> FindAllByIdAsync(int wvId)
        {
            try
            {
                var result = await DenimDbContext.F_PR_WEAVING_PROCESS_MASTER_B
                    .Include(c => c.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B)
                    .ThenInclude(c => c.F_PR_WEAVING_PROCESS_DETAILS_B)
                    .ThenInclude(c => c.LOOM_NONavigation)

                    .Include(c => c.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B)
                    .ThenInclude(c => c.F_PR_WEAVING_PROCESS_DETAILS_B)
                    .ThenInclude(c => c.LOOM_TYPENavigation)

                    .Include(c => c.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B)
                    .ThenInclude(c => c.F_PR_WEAVING_PROCESS_DETAILS_B)
                    .ThenInclude(c => c.OTHER_DOFF)

                    .Include(c => c.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B)
                    .ThenInclude(c => c.F_PR_WEAVING_PROCESS_DETAILS_B)
                    .ThenInclude(c => c.DOFFER_NAMENavigation)

                    .Include(c => c.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B)
                    .ThenInclude(c => c.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM)

                    .Include(c => c.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B)
                    .ThenInclude(c => c.F_PR_SLASHER_DYEING_DETAILS.W_BEAM)

                    .Include(c => c.F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS)
                    .ThenInclude(c => c.COUNT.COUNT)

                    .Include(c => c.F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS)
                    .ThenInclude(c => c.LOT)

                    .Include(c => c.F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS)
                    .ThenInclude(c => c.SUPP)

                    .FirstOrDefaultAsync(c => c.WV_PROCESSID.Equals(wvId));

                var prWeavingProcessBulkViewModel = new PrWeavingProcessBulkViewModel
                {
                    FPrWeavingProcessMasterB = result,
                    FPrWeavingProcessBeamDetailsBList = result.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B.ToList(),
                    FPrWeavingWeftYarnConsumDetailsList = result.F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS.ToList()
                };

                foreach (var item in prWeavingProcessBulkViewModel.FPrWeavingProcessBeamDetailsBList)
                {
                    item.FPrWeavingProcessDetailsBList = item.F_PR_WEAVING_PROCESS_DETAILS_B.ToList();
                }

                return prWeavingProcessBulkViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<dynamic> GetSetDetails(int setId)
        {
            try
            {
                var result = DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Include(c => c.SUBGROUP.GROUP.RND_DYEING_TYPE)

                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder)

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

                    .Include(c => c.F_PR_WEAVING_PROCESS_MASTER_B)
                    .ThenInclude(c => c.F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS)

                    .Include(c => c.F_PR_WEAVING_PROCESS_MASTER_B)
                    .ThenInclude(c => c.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B)
                    .ThenInclude(c => c.F_PR_WEAVING_PROCESS_DETAILS_B)

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
                    .Where(c => c.SETID.Equals(setId))
                    .Select(c => new
                    {
                        c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME,
                        c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.CRIMP_PERCENTAGE,
                        c.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER.BUYER_NAME,
                        c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation.COLOR,
                        LOOMTYPE = c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.LOOM.LOOM_TYPE_NAME,
                        ORDERNO = c.PROG_.BLK_PROG_.RndProductionOrder.SO.SO_NO,
                        c.SUBGROUP.RATIO,
                        TOTALENDS = c.PROG_.BLK_PROG_.RndProductionOrder.TOTAL_ENOS,
                        SETLENGTH = c.PROG_.SET_QTY,
                        PONO = c.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.PINO,
                        c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.LOOMID,
                        DYEING_TYPE = c.SUBGROUP.GROUP.RND_DYEING_TYPE.DTYPE,
                        c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.REED_SPACE,
                        c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.GRPPI,
                        SIZING_BEAM = c.F_PR_SIZING_PROCESS_ROPE_MASTER != null ? c.F_PR_SIZING_PROCESS_ROPE_MASTER.Select(d => new F_PR_SIZING_PROCESS_ROPE_MASTER
                        {
                            F_PR_SIZING_PROCESS_ROPE_DETAILS = d.F_PR_SIZING_PROCESS_ROPE_DETAILS.Select(m => new F_PR_SIZING_PROCESS_ROPE_DETAILS
                            {
                                IS_DELIVERABLE = m.IS_DELIVERABLE,
                                W_BEAM = new F_WEAVING_BEAM
                                {
                                    BEAM_NO = m.W_BEAM.BEAM_NO
                                },
                                SDID = m.SDID,
                                LENGTH_PER_BEAM = m.LENGTH_PER_BEAM
                            }).ToList(),
                            //OPT1 = d.F_PR_SIZING_PROCESS_ROPE_DETAILS.Sum(p => p.LENGTH_PER_BEAM ?? 0).ToString(CultureInfo.InvariantCulture)
                        }).ToList() : c.F_PR_SIZING_PROCESS_ROPE_MASTER,
                        SLASHER_BEAM = c.F_PR_SLASHER_DYEING_MASTER != null ? c.F_PR_SLASHER_DYEING_MASTER.Select(d => new F_PR_SLASHER_DYEING_MASTER
                        {
                            F_PR_SLASHER_DYEING_DETAILS = d.F_PR_SLASHER_DYEING_DETAILS.Select(m => new F_PR_SLASHER_DYEING_DETAILS
                            {
                                IS_DELIVERABLE = m.IS_DELIVERABLE,
                                W_BEAM = new F_WEAVING_BEAM
                                {
                                    BEAM_NO = m.W_BEAM.BEAM_NO
                                },
                                SLDID = m.SLDID,
                                LENGTH_PER_BEAM = m.LENGTH_PER_BEAM
                            }).ToList(),
                            //OPT1 = d.F_PR_SLASHER_DYEING_DETAILS.Sum(p => p.LENGTH_PER_BEAM ?? 0).ToString(CultureInfo.InvariantCulture)
                        }).ToList() : c.F_PR_SLASHER_DYEING_MASTER,
                        RndFabricCountInfoViewModels = c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO
                                    .Select(e => new RndFabricCountInfoViewModel
                                    {
                                        RndFabricCountinfo = new RND_FABRIC_COUNTINFO
                                        {
                                            COUNT = new BAS_YARN_COUNTINFO
                                            {
                                                COUNTNAME = e.COUNT.COUNTNAME
                                            },
                                            COUNTID = e.COUNTID,
                                            LOT = new BAS_YARN_LOTINFO
                                            {
                                                LOTNO = e.LOT.LOTNO
                                            },
                                            LOTID = e.LOTID,
                                            SUPP = new BAS_SUPPLIERINFO
                                            {
                                                SUPPNAME = e.SUPP.SUPPNAME
                                            },
                                            SUPPID = e.SUPPID,
                                            TRNSID = e.TRNSID,
                                            FABCODE = e.FABCODE,
                                            YARNTYPE = e.YARNTYPE,
                                            DESCRIPTION = e.DESCRIPTION,
                                            COLORCODE = e.COLORCODE,
                                            RATIO = e.RATIO,
                                            YARNFOR = e.YARNFOR,
                                            YarnFor = e.YarnFor,
                                            NE = e.NE
                                        },
                                        YARNFOR = e.YARNFOR,
                                        AMOUNT = c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(d => d.FABCODE.Equals(e.FABCODE) && d.COLOR.Equals(e.COLORCODE) && d.COUNTID.Equals(e.COUNTID)).AMOUNT
                                    }).ToList(),
                        F_PR_WEAVING_PROCESS_MASTER_B = c.F_PR_WEAVING_PROCESS_MASTER_B.Select(m => new F_PR_WEAVING_PROCESS_MASTER_B
                        {
                            F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS = m.F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS.ToList()
                        }).ToList()
                    })
                    .FirstOrDefault();


                foreach (var item in result.RndFabricCountInfoViewModels.Where(c => c.YARNFOR == 2))
                {
                    foreach (var i in result.F_PR_WEAVING_PROCESS_MASTER_B)
                    {
                        item.FPrWeavingWeftYarnConsumDetails = i.F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS.FirstOrDefault(c => c.COUNTID.Equals(item.RndFabricCountinfo.TRNSID));
                        if (item.FPrWeavingWeftYarnConsumDetails == null) continue;
                        if (item.FPrWeavingWeftYarnConsumDetails != null) item.IsConsumption = true;
                        break;
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<PrWeavingProcessBulkViewModel> GetConsumpDetails(
            PrWeavingProcessBulkViewModel prWeavingProcessBulkViewModel)
        {
            try
            {
                foreach (var item in prWeavingProcessBulkViewModel.FPrWeavingWeftYarnConsumDetailsList)
                {
                    item.COUNT = await DenimDbContext.RND_FABRIC_COUNTINFO.Include(c => c.COUNT)
                        .Select(c => new RND_FABRIC_COUNTINFO
                        {
                            TRNSID = c.TRNSID,
                            COUNT = new BAS_YARN_COUNTINFO
                            {
                                COUNTNAME = c.COUNT.COUNTNAME
                            }
                        })
                        .FirstOrDefaultAsync(c => c.TRNSID.Equals(item.COUNTID));
                    item.LOT = await DenimDbContext.BAS_YARN_LOTINFO.Where(c => c.LOTID == item.LOTID)
                        .FirstOrDefaultAsync();
                    item.SUPP = await DenimDbContext.BAS_SUPPLIERINFO.Where(c => c.SUPPID.Equals(item.SUPPID))
                        .FirstOrDefaultAsync();
                }

                return prWeavingProcessBulkViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}







//var result = _denimDbContext.PL_PRODUCTION_SETDISTRIBUTION
//    .Include(c => c.SUBGROUP.GROUP.RND_DYEING_TYPE)

//    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder)

//    .Include(c => c.F_PR_SIZING_PROCESS_ROPE_MASTER)
//    .ThenInclude(c => c.F_PR_SIZING_PROCESS_ROPE_DETAILS)
//    .ThenInclude(c => c.S_M)

//    .Include(c => c.F_PR_SIZING_PROCESS_ROPE_MASTER)
//    .ThenInclude(c => c.F_PR_SIZING_PROCESS_ROPE_DETAILS)
//    .ThenInclude(c => c.W_BEAM)

//    .Include(c => c.F_PR_SLASHER_DYEING_MASTER)
//    .ThenInclude(c => c.F_PR_SLASHER_DYEING_DETAILS)
//    .ThenInclude(c => c.W_BEAM)

//    .Include(c => c.F_PR_SLASHER_DYEING_MASTER)
//    .ThenInclude(c => c.F_PR_SLASHER_DYEING_DETAILS)
//    .ThenInclude(c => c.SL_M)

//    .Include(c => c.F_PR_WEAVING_PROCESS_MASTER_B)
//    .ThenInclude(c => c.F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS)

//    .Include(c => c.F_PR_WEAVING_PROCESS_MASTER_B)
//    .ThenInclude(c => c.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B)
//    .ThenInclude(c => c.F_PR_WEAVING_PROCESS_DETAILS_B)

//    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.WV)
//    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation)
//    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.LOOM)

//    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
//    .ThenInclude(c => c.LOT)

//    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
//    .ThenInclude(c => c.SUPP)

//    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
//    .ThenInclude(c => c.COUNT)

//    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_YARNCONSUMPTION)

//    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER)
//    .Where(c => c.SETID.Equals(setId) )
//    .Select(c => new RndProductionOrderDetailViewModel
//    {
//        ComExPiDetails = c.PROG_.BLK_PROG_.RndProductionOrder.SO,
//        PlProductionSetDistribution = c,
//        RndFabricCountInfoViewModels = c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO
//            .Select(e => new RndFabricCountInfoViewModel
//            {
//                RndFabricCountinfo = e,
//                AMOUNT = c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(d => d.FABCODE.Equals(e.FABCODE) && d.COLOR.Equals(e.COLORCODE) && d.COUNTID.Equals(e.COUNTID)).AMOUNT
//            }).ToList()
//    })
//    .FirstOrDefault();

//foreach (var item in result.RndFabricCountInfoViewModels.Where(c=>c.YARNFOR==2))
//{
//    foreach (var i in result.PlProductionSetDistribution.F_PR_WEAVING_PROCESS_MASTER_B)
//    {
//        item.FPrWeavingWeftYarnConsumDetails = i.F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS.FirstOrDefault(c => c.COUNTID.Equals(item.RndFabricCountinfo.TRNSID));
//        if (item.FPrWeavingWeftYarnConsumDetails == null) continue;
//        if (item.FPrWeavingWeftYarnConsumDetails != null) item.IsConsumption = true;
//        break;
//    }
//}

//foreach (var item in result.PlProductionSetDistribution.F_PR_SIZING_PROCESS_ROPE_MASTER)
//{
//    item.SET = null;
//}
//foreach (var item in result.PlProductionSetDistribution.F_PR_SLASHER_DYEING_MASTER)
//{
//    item.SET = null;
//}
//foreach (var item in result.PlProductionSetDistribution.F_PR_WEAVING_PROCESS_MASTER_B)
//{
//    item.SET = null;
//    foreach (var i in item.F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS)
//    {
//        i.WEAVING = null;
//    }
//    foreach (var i in item.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B)
//    {
//        i.WV_PROCESS = null;
//    }
//}