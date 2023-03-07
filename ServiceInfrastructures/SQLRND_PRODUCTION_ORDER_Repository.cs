using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Rnd;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLRND_PRODUCTION_ORDER_Repository : BaseRepository<RND_PRODUCTION_ORDER>, IRND_PRODUCTION_ORDER
    {
        private readonly IDataProtector _protector;

        public SQLRND_PRODUCTION_ORDER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<RndProductionOrderViewModel> GetInitObjects(RndProductionOrderViewModel rndProductionOrderViewModel)
        {
            rndProductionOrderViewModel.RndOrderTypes =
                await DenimDbContext.RND_ORDER_TYPE.Select(c => new RND_ORDER_TYPE
                {
                    OTYPEID = c.OTYPEID,
                    OTYPENAME = c.OTYPENAME
                }).ToListAsync();

            rndProductionOrderViewModel.RndOrderRepeats =
                await DenimDbContext.RND_ORDER_REPEAT.Select(c => new RND_ORDER_REPEAT
                {
                    ORPTID = c.ORPTID,
                    ORPTNAME = c.ORPTNAME
                }).OrderBy(c => c.ORPTNAME).ToListAsync();

            if (rndProductionOrderViewModel.RndProductionOrder == null)
            {
                rndProductionOrderViewModel.ComExPiDetailsList = await DenimDbContext.COM_EX_PI_DETAILS
                    .Where(c => !DenimDbContext.RND_PRODUCTION_ORDER.Any(e => e.ORDERNO.Equals(c.TRNSID)))
                    .Select(c => new COM_EX_PI_DETAILS
                    {
                        SO_NO = c.SO_NO,
                        TRNSID = c.TRNSID
                    }).OrderByDescending(c => c.SO_NO).ToListAsync();
            }
            else
            {
                rndProductionOrderViewModel.ComExPiDetailsList = await DenimDbContext.COM_EX_PI_DETAILS.Select(c => new COM_EX_PI_DETAILS
                {
                    SO_NO = c.SO_NO,
                    TRNSID = c.TRNSID
                }).OrderByDescending(c => c.SO_NO).ToListAsync();

            }
            rndProductionOrderViewModel.RndMstrRolls =
                await DenimDbContext.RND_MSTR_ROLL.Select(c => new RND_MSTR_ROLL
                {
                    MID = c.MID,
                    MASTER_ROLL = c.MASTER_ROLL
                }).ToListAsync();

            rndProductionOrderViewModel.BasYarnLotInfos = await DenimDbContext.BAS_YARN_LOTINFO.Select(c => new BAS_YARN_LOTINFO
            {
                LOTID = c.LOTID,
                LOTNO = c.LOTNO,
                BRAND = c.BRAND
            }).ToListAsync();

            rndProductionOrderViewModel.BasSupplierInfos = await DenimDbContext.BAS_SUPPLIERINFO.Select(c => new BAS_SUPPLIERINFO
            {
                SUPPID = c.SUPPID,
                SUPPNAME = c.SUPPNAME
            }).ToListAsync();

            rndProductionOrderViewModel.Yarnfor = await DenimDbContext.YARNFOR.Select(c => new YARNFOR
            {
                YARNID = c.YARNID,
                YARNNAME = c.YARNNAME
            }).ToListAsync();

            rndProductionOrderViewModel.BasYarnCountInfos = await DenimDbContext.BAS_YARN_COUNTINFO
                .Where(c => !c.YARN_CAT_ID.Equals(8102699))
                .Select(c => new BAS_YARN_COUNTINFO()
                {
                    COUNTID = c.COUNTID,
                    COUNTNAME = c.COUNTNAME
                }).ToListAsync();

            return rndProductionOrderViewModel;
        }

        public async Task<int> InsertAndGetIdAsync(RND_PRODUCTION_ORDER rndProductionOrder)
        {
            try
            {
                await DenimDbContext.RND_PRODUCTION_ORDER.AddAsync(rndProductionOrder);
                await SaveChanges();
                return rndProductionOrder.POID;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public RndProductionOrderDetailViewModel GetPoDetailsAsync(string orderNo)
        {
            try
            {
                var result = DenimDbContext.COM_EX_PI_DETAILS
                    .Include(c => c.STYLE)
                    .Where(c => c.TRNSID.Equals(int.Parse(orderNo)))
                    .GroupJoin(DenimDbContext.RND_FABRIC_COUNTINFO
                            .OrderBy(c => c.COUNTID)
                            .Include(c => c.LOT)
                            .Include(c => c.SUPP)
                            .Include(c => c.COUNT)
                            .Include(c => c.YarnFor)
                        ,
                        f1 => f1.STYLE.FABCODE,
                        f2 => f2.FABCODE,
                        (f1, f2) => new RndProductionOrderDetailViewModel
                        {
                            RndFabricCountInfos = f2.ToList(),
                            ComExPiDetails = f1
                        }
                    )
                    .GroupJoin(DenimDbContext.RND_YARNCONSUMPTION.OrderBy(c => c.COUNTID),
                        f3 => f3.ComExPiDetails.STYLE.FABCODE,
                        f4 => f4.FABCODE,
                        (f3, f4) => new RndProductionOrderDetailViewModel
                        {
                            RndFabricCountInfos = f3.RndFabricCountInfos,
                            ComExPiDetails = f3.ComExPiDetails,
                            RndYarnconsumptions = f4.ToList()
                        })
                    .GroupJoin(DenimDbContext.RND_FABRICINFO.OrderBy(c => c.FABCODE)
                            .Include(c => c.D)
                            .Include(c => c.LOOM)
                            .Include(c => c.COLORCODENavigation),
                        f7 => f7.ComExPiDetails.STYLE.FABCODE,
                        f8 => f8.FABCODE,
                        (f7, f8) => new RndProductionOrderDetailViewModel
                        {
                            RndFabricCountInfos = f7.RndFabricCountInfos,
                            ComExPiDetails = f7.ComExPiDetails,
                            RndYarnconsumptions = f7.RndYarnconsumptions,
                            RndFabricInfo = f8.FirstOrDefault()
                        })
                    .GroupJoin(DenimDbContext.COM_EX_PIMASTER.OrderBy(c => c.PIID)
                            .Include(c => c.BUYER),
                        f9 => f9.ComExPiDetails.PIID,
                        f10 => f10.PIID,
                        (f9, f10) => new RndProductionOrderDetailViewModel
                        {
                            RndFabricCountInfos = f9.RndFabricCountInfos,
                            ComExPiDetails = f9.ComExPiDetails,
                            RndYarnconsumptions = f9.RndYarnconsumptions,
                            RndFabricInfo = f9.RndFabricInfo,
                            ComExPimaster = f10.FirstOrDefault()
                        })
                    .GroupJoin(DenimDbContext.RND_PRODUCTION_ORDER,
                        f11 => f11.ComExPiDetails.TRNSID,
                        f12 => f12.ORDERNO,
                        (f11, f12) => new RndProductionOrderDetailViewModel
                        {
                            RndFabricCountInfos = f11.RndFabricCountInfos,
                            ComExPiDetails = f11.ComExPiDetails,
                            RndYarnconsumptions = f11.RndYarnconsumptions,
                            RndFabricInfo = f11.RndFabricInfo,
                            ComExPimaster = f11.ComExPimaster,
                            RndProductionOrder = f12.FirstOrDefault()
                        })
                    .GroupJoin(DenimDbContext.PL_ORDERWISE_LOTINFO.Include(c => c.LOT),
                        f13 => f13.RndProductionOrder.POID,
                        f14 => f14.POID,
                        (f13, f14) => new RndProductionOrderDetailViewModel
                        {
                            RndFabricCountInfos = f13.RndFabricCountInfos,
                            ComExPiDetails = f13.ComExPiDetails,
                            RndYarnconsumptions = f13.RndYarnconsumptions,
                            RndFabricInfo = f13.RndFabricInfo,
                            ComExPimaster = f13.ComExPimaster,
                            RndProductionOrder = f13.RndProductionOrder,
                            BasYarnLotInfos = f14.Select(c => new BAS_YARN_LOTINFO
                            {
                                LOTID = c.LOT.LOTID,
                                LOTNO = c.LOT.LOTNO
                            }).ToList()
                        })
                    .Select(c => new RndProductionOrderDetailViewModel
                    {
                        RndFabricCountInfos = c.RndFabricCountInfos,
                        RndYarnconsumptions = c.RndYarnconsumptions,
                        ComExPiDetails = c.ComExPiDetails,
                        BasYarnLotInfos = c.BasYarnLotInfos,
                        RndFabricInfo = new RND_FABRICINFO
                        {
                            D = c.RndFabricInfo.D,
                            LOOM = c.RndFabricInfo.LOOM,
                            COLORCODENavigation = c.RndFabricInfo.COLORCODENavigation,
                            TOTALENDS = c.RndFabricInfo.TOTALENDS,
                            FABCODE = c.RndFabricInfo.FABCODE,
                            REED_SPACE = c.RndFabricInfo.REED_SPACE,
                            GRPPI = c.RndFabricInfo.GRPPI,
                            FNPPI = c.RndFabricInfo.FNPPI,
                            LOOMID = c.RndFabricInfo.LOOMID,
                            STYLE_NAME = c.RndFabricInfo.STYLE_NAME,
                            WIDEC = c.RndFabricInfo.WIDEC,
                            WGDEC = c.RndFabricInfo.WGDEC,
                            WIFNCUT = c.RndFabricInfo.WIFNCUT,
                            WGFNBW = c.RndFabricInfo.WGFNBW,
                            WGFNAW = c.RndFabricInfo.WGFNAW,
                            BWEPI = c.RndFabricInfo.BWEPI,
                            BWPPI = c.RndFabricInfo.BWPPI,
                            SRDECWARP = c.RndFabricInfo.SRDECWARP,
                            SRDECWEFT = c.RndFabricInfo.SRDECWEFT,
                            STDECWARP = c.RndFabricInfo.STDECWARP,
                            STDECWEFT = c.RndFabricInfo.STDECWEFT,
                            CRIMP_PERCENTAGE = c.RndFabricInfo.CRIMP_PERCENTAGE
                        },
                        ComExPimaster = new COM_EX_PIMASTER
                        {
                            PINO = c.ComExPimaster.PINO,
                            PIDATE = c.ComExPimaster.PIDATE,
                            VALIDITY = c.ComExPimaster.VALIDITY,
                            DEL_START = c.ComExPimaster.DEL_START,
                            DEL_CLOSE = c.ComExPimaster.DEL_CLOSE,
                            BUYER = c.ComExPimaster.BUYER
                        },
                        RndFabricCountInfoViewModels = c.RndYarnconsumptions.GroupJoin(c.RndFabricCountInfos.ToList(),
                            f5 => new{ f5.COUNTID , FABCODE = f5.FABCODE??0, f5.YARNFOR},
                            f6 => new{ f6.COUNTID ,f6.FABCODE, f6.YARNFOR},
                            (f5, f6) => new RndFabricCountInfoViewModel
                            {
                                RndFabricCountinfo = f6.FirstOrDefault(),
                                AMOUNT = f5.AMOUNT
                            })
                            .Select(e => new RndFabricCountInfoViewModel
                            {
                                RndFabricCountinfo = e.RndFabricCountinfo,
                                AMOUNT = e.AMOUNT
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

        public double GetPoDetailsForCountBudgetAsync(string orderNo, string countId, string warpLength)
        {
            try
            {
                var result = DenimDbContext.PL_BULK_PROG_SETUP_D

                    .Include(c => c.BLK_PROG_)
                    .ThenInclude(c => c.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c => c.COUNT)

                    .Include(c => c.BLK_PROG_)
                    .ThenInclude(c => c.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_YARNCONSUMPTION)
                    .Include(c => c.BLK_PROG_)
                    .ThenInclude(c => c.RndProductionOrder.SO.STYLE.FABCODENavigation.LOOM)
                    .FirstOrDefault(c => c.PROG_ID.Equals(int.Parse(orderNo)));
                //.GroupJoin(_denimDbContext.COM_EX_PI_DETAILS
                //        .Include(c => c.STYLE),
                //    f0 => f0.BLK_PROG_.RndProductionOrder.ORDERNO,
                //    f00 => f00.TRNSID,
                //    (f0, f00) => new RndProductionOrderDetailViewModel
                //    {
                //        ComExPiDetails = f00.FirstOrDefault()
                //    })
                //.GroupJoin(_denimDbContext.RND_FABRIC_COUNTINFO.OrderBy(c => c.COUNTID)
                //        .Include(c => c.LOT)
                //        .Include(c => c.SUPP)
                //        .Include(c => c.COUNT),
                //    f1 => f1.ComExPiDetails.STYLE.FABCODE,
                //    f2 => f2.FABCODE,
                //    (f1, f2) => new RndProductionOrderDetailViewModel
                //    {
                //        RndFabricCountInfos = f2.ToList(),
                //        ComExPiDetails = f1.ComExPiDetails
                //    }
                //)
                //.GroupJoin(_denimDbContext.RND_FABRICINFO.OrderBy(c => c.FABCODE)
                //        .Include(c => c.D)
                //        .Include(c => c.LOOM)
                //        .Include(c => c.COLORCODENavigation),
                //    f7 => f7.ComExPiDetails.STYLE.FABCODE,
                //    f8 => f8.FABCODE,
                //    (f7, f8) => new RndProductionOrderDetailViewModel
                //    {
                //        RndFabricCountInfos = f7.RndFabricCountInfos,
                //        ComExPiDetails = f7.ComExPiDetails,
                //        RndFabricInfo = f8.FirstOrDefault()
                //    })
                //.FirstOrDefault();


                var reqKg = 0.0;

                var totalRatioWarp = 0.0;
                var totalRatioWeft = 0.0;

                if (result != null)
                {
                    foreach (var item in result.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation
                        .RND_FABRIC_COUNTINFO.Where(c => c.YARNFOR.Equals(1)))
                    {
                        totalRatioWarp += (double)item.RATIO;
                    }
                    foreach (var item in result.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation
                        .RND_FABRIC_COUNTINFO.Where(c => c.YARNFOR.Equals(2)))
                    {
                        totalRatioWeft += (double)item.RATIO;
                    }

                    var count = result.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation
                        .RND_FABRIC_COUNTINFO.FirstOrDefault(c => c.COUNTID.Equals(int.Parse(countId))) ?? result.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation
                        .RND_FABRIC_COUNTINFO.FirstOrDefault(c => c.TRNSID.Equals(int.Parse(countId)));

                    if (count != null)
                    {
                        switch (count.YARNFOR)
                        {
                            case 1:
                                {
                                    if (result.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.TOTALENDS != null)
                                    {
                                        reqKg = (float.Parse(warpLength) * (double)result.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.TOTALENDS * (double)count.RATIO) / ((double)count.NE * totalRatioWarp * 768 * 2.2046);
                                    }

                                    break;
                                }
                            case 2:
                                {
                                    if (result.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.REED_SPACE != null && result.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.GRPPI != null)
                                    {
                                        var greyLength = float.Parse(warpLength) * 0.92;

                                        reqKg = (greyLength * (double)result.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.REED_SPACE * (double)result.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.GRPPI * (double)count.RATIO) / ((double)count.NE * totalRatioWarp * 768 * 2.2046);
                                    }
                                    break;
                                }
                        }
                    }
                }

                return reqKg;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public RndProductionOrderDetailViewModel GetPoDetailsByPoIdAsync(string orderNo)
        {
            try
            {
                var result = DenimDbContext.RND_PRODUCTION_ORDER

                    .Include(c=>c.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c=>c.COUNT)
                    .Include(c=>c.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c=>c.SUPP)
                    .Include(c=>c.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c=>c.LOT)
                    .Include(c=>c.SO.STYLE.FABCODENavigation.RND_YARNCONSUMPTION)
                    .Include(c=>c.SO.STYLE.FABCODENavigation.D)
                    .Include(c=>c.SO.STYLE.FABCODENavigation.LOOM)
                    .Include(c=>c.SO.STYLE.FABCODENavigation.COLORCODENavigation)

                    .Include(c=>c.SO.PIMASTER.BUYER)
                    .Include(c=>c.SO.PIMASTER.BUYER)

                    .OrderByDescending(c => c.POID)
                    .Where(c => c.POID.Equals(int.Parse(orderNo)))
                    
                    .Select(c => new RndProductionOrderDetailViewModel
                    {
                        RndProductionOrder = c,
                        RndFabricCountInfos = c.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO.ToList(),
                        RndYarnconsumptions = c.SO.STYLE.FABCODENavigation.RND_YARNCONSUMPTION.ToList(),
                        //BasYarnLotInfos = c.BasYarnLotInfos,
                        ComExPiDetails = new COM_EX_PI_DETAILS
                        {
                            PIID = c.SO.PIID,
                            PINO = c.SO.PINO,
                            QTY = c.SO.QTY,
                            SO_NO = c.SO.SO_NO,
                            STYLE = c.SO.STYLE
                        },
                        RndFabricInfo = new RND_FABRICINFO
                        {
                            D = c.SO.STYLE.FABCODENavigation.D,
                            LOOM = c.SO.STYLE.FABCODENavigation.LOOM,
                            COLORCODENavigation = c.SO.STYLE.FABCODENavigation.COLORCODENavigation,
                            TOTALENDS = c.SO.STYLE.FABCODENavigation.TOTALENDS,
                            FABCODE = c.SO.STYLE.FABCODENavigation.FABCODE,
                            STYLE_NAME = c.SO.STYLE.FABCODENavigation.STYLE_NAME,
                            REED_SPACE = c.SO.STYLE.FABCODENavigation.REED_SPACE,
                            GRPPI = c.SO.STYLE.FABCODENavigation.GRPPI
                        },
                        ComExPimaster = new COM_EX_PIMASTER
                        {
                            PINO = c.SO.PIMASTER.PINO,
                            PIDATE = c.SO.PIMASTER.PIDATE,
                            VALIDITY = c.SO.PIMASTER.VALIDITY,
                            DEL_START = c.SO.PIMASTER.DEL_START,
                            DEL_CLOSE = c.SO.PIMASTER.DEL_CLOSE,
                            BUYER = c.SO.PIMASTER.BUYER
                        },
                        RndFabricCountInfoViewModels = c.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO.Select(e=> new RndFabricCountInfoViewModel
                        {
                            RndFabricCountinfo = e,
                            AMOUNT = c.SO.STYLE.FABCODENavigation.RND_YARNCONSUMPTION.Where(f=>f.COUNTID.Equals(e.COUNTID) && f.FABCODE.Equals(e.FABCODE) && f.YARNFOR.Equals(e.YARNFOR)).Select(f=>f.AMOUNT).FirstOrDefault()
                        }).ToList()                        
                            
                    })
                    .FirstOrDefault();
                
                return result;




                //var result = _denimDbContext.RND_PRODUCTION_ORDER.OrderByDescending(c => c.POID)
                //    .Where(c => c.POID.Equals(int.Parse(orderNo)))
                //    .GroupJoin(_denimDbContext.COM_EX_PI_DETAILS
                //        .Include(c => c.STYLE),
                //        f0 => f0.ORDERNO,
                //        f00 => f00.TRNSID,
                //        (f0, f00) => new RndProductionOrderDetailViewModel
                //        {
                //            ComExPiDetails = f00.FirstOrDefault(),
                //            RndProductionOrder = f0
                //        })
                //    .GroupJoin(_denimDbContext.RND_FABRIC_COUNTINFO.OrderBy(c => c.COUNTID)
                //            .Include(c => c.LOT)
                //            .Include(c => c.SUPP)
                //            .Include(c => c.COUNT),
                //        f1 => f1.ComExPiDetails.STYLE.FABCODE,
                //        f2 => f2.FABCODE,
                //        (f1, f2) => new RndProductionOrderDetailViewModel
                //        {
                //            RndFabricCountInfos = f2.ToList(),
                //            ComExPiDetails = f1.ComExPiDetails,
                //            RndProductionOrder = f1.RndProductionOrder
                //        }
                //    )
                //    .GroupJoin(_denimDbContext.RND_YARNCONSUMPTION.OrderBy(c => c.COUNTID),
                //        f3 => f3.ComExPiDetails.STYLE.FABCODE,
                //        f4 => f4.FABCODE,
                //        (f3, f4) => new RndProductionOrderDetailViewModel
                //        {
                //            RndFabricCountInfos = f3.RndFabricCountInfos,
                //            ComExPiDetails = f3.ComExPiDetails,
                //            RndProductionOrder = f3.RndProductionOrder,
                //            RndYarnconsumptions = f4.ToList()
                //        })
                //    .GroupJoin(_denimDbContext.RND_FABRICINFO.OrderBy(c => c.FABCODE)
                //            .Include(c => c.D)
                //            .Include(c => c.LOOM)
                //            .Include(c => c.COLORCODENavigation),
                //        f7 => f7.ComExPiDetails.STYLE.FABCODE,
                //        f8 => f8.FABCODE,
                //        (f7, f8) => new RndProductionOrderDetailViewModel
                //        {
                //            RndFabricCountInfos = f7.RndFabricCountInfos,
                //            ComExPiDetails = f7.ComExPiDetails,
                //            RndProductionOrder = f7.RndProductionOrder,
                //            RndYarnconsumptions = f7.RndYarnconsumptions,
                //            RndFabricInfo = f8.FirstOrDefault()
                //        })
                //    .GroupJoin(_denimDbContext.COM_EX_PIMASTER.OrderBy(c => c.PIID)
                //            .Include(c => c.BUYER),
                //        f9 => f9.ComExPiDetails.PIID,
                //        f10 => f10.PIID,
                //        (f9, f10) => new RndProductionOrderDetailViewModel
                //        {
                //            RndFabricCountInfos = f9.RndFabricCountInfos,
                //            ComExPiDetails = f9.ComExPiDetails,
                //            RndProductionOrder = f9.RndProductionOrder,
                //            RndYarnconsumptions = f9.RndYarnconsumptions,
                //            RndFabricInfo = f9.RndFabricInfo,
                //            ComExPimaster = f10.FirstOrDefault()
                //        })
                //    //.GroupJoin(_denimDbContext.RND_PRODUCTION_ORDER,
                //    //    f11 => f11.ComExPiDetails.TRNSID,
                //    //    f12 => int.Parse(f12.ORDERNO),
                //    //    (f11, f12) => new RndProductionOrderDetailViewModel
                //    //    {
                //    //        RndFabricCountInfos = f11.RndFabricCountInfos,
                //    //        ComExPiDetails = f11.ComExPiDetails,
                //    //        RndYarnconsumptions = f11.RndYarnconsumptions,
                //    //        RndFabricInfo = f11.RndFabricInfo,
                //    //        ComExPimaster = f11.ComExPimaster,
                //    //        RndProductionOrder = f12.FirstOrDefault()
                //    //    })
                //    .GroupJoin(_denimDbContext.PL_ORDERWISE_LOTINFO.Include(c => c.LOT),
                //        f13 => f13.RndProductionOrder.POID,
                //        f14 => f14.POID,
                //        (f13, f14) => new RndProductionOrderDetailViewModel
                //        {
                //            RndFabricCountInfos = f13.RndFabricCountInfos,
                //            ComExPiDetails = f13.ComExPiDetails,
                //            RndYarnconsumptions = f13.RndYarnconsumptions,
                //            RndFabricInfo = f13.RndFabricInfo,
                //            ComExPimaster = f13.ComExPimaster,
                //            RndProductionOrder = f13.RndProductionOrder,
                //            BasYarnLotInfos = f14.Select(c => new BAS_YARN_LOTINFO
                //            {
                //                LOTID = c.LOT.LOTID,
                //                LOTNO = c.LOT.LOTNO
                //            }).ToList()
                //        })
                //    .Select(c => new RndProductionOrderDetailViewModel
                //    {
                //        RndFabricCountInfos = c.RndFabricCountInfos,
                //        RndYarnconsumptions = c.RndYarnconsumptions,
                //        BasYarnLotInfos = c.BasYarnLotInfos,
                //        ComExPiDetails = new COM_EX_PI_DETAILS
                //        {
                //            PIID = c.ComExPiDetails.PIID,
                //            PINO = c.ComExPiDetails.PINO,
                //            QTY = c.ComExPiDetails.QTY,
                //            SO_NO = c.ComExPiDetails.SO_NO,
                //            STYLE = c.ComExPiDetails.STYLE
                //        },
                //        RndFabricInfo = new RND_FABRICINFO
                //        {
                //            D = c.RndFabricInfo.D,
                //            LOOM = c.RndFabricInfo.LOOM,
                //            COLORCODENavigation = c.RndFabricInfo.COLORCODENavigation,
                //            TOTALENDS = c.RndFabricInfo.TOTALENDS,
                //            FABCODE = c.RndFabricInfo.FABCODE,
                //            REED_SPACE = c.RndFabricInfo.REED_SPACE,
                //            GRPPI = c.RndFabricInfo.GRPPI
                //        },
                //        ComExPimaster = new COM_EX_PIMASTER
                //        {
                //            PINO = c.ComExPimaster.PINO,
                //            PIDATE = c.ComExPimaster.PIDATE,
                //            VALIDITY = c.ComExPimaster.VALIDITY,
                //            DEL_START = c.ComExPimaster.DEL_START,
                //            DEL_CLOSE = c.ComExPimaster.DEL_CLOSE,
                //            BUYER = c.ComExPimaster.BUYER
                //        },
                //        RndFabricCountInfoViewModels = c.RndYarnconsumptions.GroupJoin(c.RndFabricCountInfos.ToList(),
                //            f5 => f5.COUNTID,
                //            f6 => f6.COUNTID,
                //            (f5, f6) => new RndFabricCountInfoViewModel
                //            {
                //                RndFabricCountinfo = f6.FirstOrDefault(),
                //                AMOUNT = f5.AMOUNT
                //            })
                //            .Select(e => new RndFabricCountInfoViewModel
                //            {
                //                RndFabricCountinfo = e.RndFabricCountinfo,
                //                AMOUNT = e.AMOUNT
                //            }).ToList()
                //    })
                //    .FirstOrDefault();





            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }



        public RndProductionOrderDetailViewModel GetRndPoDetailsByPoIdAsync(string orderNo)
        {
            try
            {
                var result = DenimDbContext.RND_PRODUCTION_ORDER

                    .Include(c => c.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c => c.COUNT)
                    .Include(c => c.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c => c.SUPP)
                    .Include(c => c.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c => c.LOT)
                    .Include(c => c.SO.STYLE.FABCODENavigation.RND_YARNCONSUMPTION)
                    .Include(c => c.SO.STYLE.FABCODENavigation.D)
                    .Include(c => c.SO.STYLE.FABCODENavigation.LOOM)
                    .Include(c => c.SO.STYLE.FABCODENavigation.COLORCODENavigation)
                    .Include(c=>c.RS.SDRF)

                    .Include(c => c.SO.PIMASTER.BUYER)
                    .Include(c => c.SO.PIMASTER.BUYER)

                    .OrderByDescending(c => c.POID)
                    .Where(c => c.POID.Equals(int.Parse(orderNo)))

                    .Select(c => new RndProductionOrderDetailViewModel
                    {
                        RndProductionOrder = c,
                        ComExPiDetails =c.SO==null?new COM_EX_PI_DETAILS(): new COM_EX_PI_DETAILS
                        {
                            PIID = c.SO.PIID,
                            PINO = c.SO.PINO,
                            QTY = c.SO.QTY,
                            SO_NO = c.SO.SO_NO,
                            STYLE = c.SO.STYLE
                        },
                        RndSampleInfoDyeing = c.RS==null?new RND_SAMPLE_INFO_DYEING():new RND_SAMPLE_INFO_DYEING
                        {
                            SDID = c.RS.SDID,
                            RSOrder = c.RS.RSOrder,
                            DYEINGCODE = c.RS.DYEINGCODE
                        } ,
                        RndFabricInfo = c.SO == null ? new RND_FABRICINFO() : new RND_FABRICINFO
                        {
                            D = c.SO.STYLE.FABCODENavigation.D,
                            LOOM = c.SO.STYLE.FABCODENavigation.LOOM,
                            COLORCODENavigation = c.SO.STYLE.FABCODENavigation.COLORCODENavigation,
                            TOTALENDS = c.SO.STYLE.FABCODENavigation.TOTALENDS,
                            FABCODE = c.SO.STYLE.FABCODENavigation.FABCODE,
                            STYLE_NAME = c.SO.STYLE.FABCODENavigation.STYLE_NAME,
                            REED_SPACE = c.SO.STYLE.FABCODENavigation.REED_SPACE,
                            GRPPI = c.SO.STYLE.FABCODENavigation.GRPPI
                        },
                        ComExPimaster = c.SO == null ? new COM_EX_PIMASTER() : new COM_EX_PIMASTER
                        {
                            PINO = c.SO.PIMASTER.PINO,
                            PIDATE = c.SO.PIMASTER.PIDATE,
                            VALIDITY = c.SO.PIMASTER.VALIDITY,
                            DEL_START = c.SO.PIMASTER.DEL_START,
                            DEL_CLOSE = c.SO.PIMASTER.DEL_CLOSE,
                            BUYER = c.SO.PIMASTER.BUYER
                        }

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


        public async Task<RND_PRODUCTION_ORDER> GetSoNoByPoIdAsync(string orderNo)
        {
            try
            {
                var result = await DenimDbContext.RND_PRODUCTION_ORDER.OrderByDescending(c => c.POID)
                    .Where(c => c.POID.Equals(int.Parse(orderNo)))
                    .GroupJoin(DenimDbContext.COM_EX_PI_DETAILS
                        .Include(c => c.STYLE),
                        f0 => f0.ORDERNO,
                        f00 => f00.TRNSID,
                        (f0, f00) => new RND_PRODUCTION_ORDER
                        {
                            OPT1 = f00.FirstOrDefault().SO_NO,
                            POID = f0.POID
                        }).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public RndRsProductionOrderDetailsViewModel GetRsDetailsAsync(string orderNo)
        {
            try
            {
                var result = DenimDbContext.RND_SAMPLE_INFO_DYEING
                    .Include(c => c.D)
                    .Include(c => c.LOOM)
                    .Include(c => c.SDRF)
                    .ThenInclude(c => c.BUYER)
                    .Where(c => c.SDID.Equals(int.Parse(orderNo)))
                    .GroupJoin(DenimDbContext.PL_SAMPLE_PROG_SETUP.OrderBy(c => c.TRNSID),
                        f1 => f1.SDID,
                        f2 => f2.SDID,
                        (f1, f2) => new RndRsProductionOrderDetailsViewModel
                        {
                            RndSampleInfoDyeing = f1,
                            PlSampleProgSetup = f2.FirstOrDefault()
                        }
                    )
                    .GroupJoin(DenimDbContext.RND_SAMPLE_INFO_DETAILS.OrderBy(c => c.TRNSID)
                        .Include(c => c.COUNT)
                        .Include(c => c.LOT)
                        .Include(c => c.SUPP),
                        f3 => f3.RndSampleInfoDyeing.SDID,
                        f4 => f4.SDID,
                        (f3, f4) => new RndRsProductionOrderDetailsViewModel
                        {
                            RndSampleInfoDyeing = f3.RndSampleInfoDyeing,
                            PlSampleProgSetup = f3.PlSampleProgSetup,
                            RndSampleInfoDetailsList = f4.ToList()
                        }
                    )
                    .GroupJoin(DenimDbContext.RND_SAMPLE_INFO_WEAVING.OrderBy(c => c.SDID),
                        f5 => f5.RndSampleInfoDyeing.SDID,
                        f6 => f6.SDID,
                        (f5, f6) => new RndRsProductionOrderDetailsViewModel
                        {
                            RndSampleInfoDyeing = f5.RndSampleInfoDyeing,
                            RndSampleInfoDetailsList = f5.RndSampleInfoDetailsList,
                            PlSampleProgSetup = f5.PlSampleProgSetup,
                            RndSampleInfoWeaving = f6.FirstOrDefault()
                        }
                        )
                    .Select(c => new RndRsProductionOrderDetailsViewModel
                    {
                        RndSampleInfoDyeing = new RND_SAMPLE_INFO_DYEING
                        {
                            D = new RND_DYEING_TYPE
                            {
                                DID = c.RndSampleInfoDyeing.D != null ? c.RndSampleInfoDyeing.D.DID : 0,
                                DTYPE = c.RndSampleInfoDyeing.D != null ? c.RndSampleInfoDyeing.D.DTYPE : ""
                            },
                            LOOM = c.RndSampleInfoDyeing.LOOM,
                            SDRF = new MKT_SDRF_INFO
                            {
                                BUYER = c.RndSampleInfoDyeing.SDRF.BUYER,
                                COLOR = c.RndSampleInfoDyeing.SDRF.COLOR
                            },
                            LENGTH_MTR = c.RndSampleInfoDyeing.LENGTH_MTR,
                            NO_OF_ROPE = c.RndSampleInfoDyeing.NO_OF_ROPE,
                            TOTAL_ENDS = c.RndSampleInfoDyeing.TOTAL_ENDS,
                            REED_SPACE = c.RndSampleInfoDyeing.REED_SPACE,
                            SDID = c.RndSampleInfoDyeing.SDID
                        },
                        PlSampleProgSetup = new PL_SAMPLE_PROG_SETUP
                        {
                            STYLE_NAME = c.PlSampleProgSetup.STYLE_NAME
                        },
                        RndSampleInfoDetailsList = c.RndSampleInfoDetailsList,
                        RndSampleInfoWeaving = new RND_SAMPLE_INFO_WEAVING
                        {
                            GR_PPI = c.RndSampleInfoWeaving.GR_PPI,
                            FABCODE = c.RndSampleInfoWeaving.FABCODE
                        }
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

        public async Task<IEnumerable<RND_PRODUCTION_ORDER>> GetAllAsync()
        {
            try
            {
                var rollDetails = await DenimDbContext.RND_PRODUCTION_ORDER
                    .Include(c => c.ORPT)
                    .Include(c => c.OTYPE)
                    .Include(c => c.SO.STYLE.FABCODENavigation)
                    .Include(c => c.SO.PIMASTER)
                    .Include(c => c.RS)
                    .Select(c => new RND_PRODUCTION_ORDER
                    {
                        POID = c.POID,
                        EncryptedId = _protector.Protect(c.POID.ToString()),
                        OTYPE = new RND_ORDER_TYPE { OTYPENAME = c.OTYPE.OTYPENAME },
                        ORPT = new RND_ORDER_REPEAT { ORPTNAME = c.ORPT.ORPTNAME },
                        ORDERNO = c.ORDERNO,
                        RSNO = c.RSNO,
                        ORDER_QTY_YDS = c.ORDER_QTY_YDS,
                        ORDER_QTY_MTR = c.ORDER_QTY_MTR,
                        REMARKS = c.REMARKS,
                        OTYPEID = c.OTYPEID,
                        OPT2 = new[] { "Export", "Local", "Bulk" }.Any(e => e.ToLower().Equals(c.OTYPE.OTYPENAME.ToLower())) ? c.SO.STYLE.FABCODENavigation.STYLE_NAME : string.Empty,
                        SO = new COM_EX_PI_DETAILS
                        {
                            SO_NO = c.SO.SO_NO
                        },
                        RS = new RND_SAMPLE_INFO_DYEING
                        {
                            DYEINGCODE = c.RS.DYEINGCODE
                        },
                        CREATED_AT = c.CREATED_AT,
                        OPT3 = new[] { "Export", "Local", "Bulk" }.Any(e => e.ToLower().Equals(c.OTYPE.OTYPENAME.ToLower())) ? c.SO.PIMASTER.PINO : string.Empty,
                        OPT1 = new[] { "Sample(RS)", "Sample(SA)", "Trail", "Leader", "Experiment", "Re-Sample" }.Any(e => e.ToLower().Equals(c.OTYPE.OTYPENAME.ToLower())) ? $"{c.RS.RSOrder}"
                            : new[] { "Export", "Local", "Bulk" }.Any(e => e.ToLower().Equals(c.OTYPE.OTYPENAME.ToLower())) ? c.SO.SO_NO : string.Empty
                    }).OrderByDescending(c => c.CREATED_AT).ToListAsync();

                #region Obsolete
                //foreach (var item in rollDetails.Where(item => item.OTYPE.OTYPENAME == "Sample(RS)" || item.OTYPE.OTYPENAME == "Sample(SA)" || item.OTYPE.OTYPENAME == "Trail" || item.OTYPE.OTYPENAME == "Leader" || item.OTYPE.OTYPENAME == "Experiment" || item.OTYPE.OTYPENAME == "Re-Sample"))
                //{
                //    item.OPT1 = await _denimDbContext.RND_SAMPLE_INFO_DYEING
                //        .Where(c => c.SDID.Equals(item.RSNO))
                //        .Select(c => "RS-" + c.SDID)
                //        .FirstOrDefaultAsync();
                //}

                //foreach (var item in rollDetails.Where(item => item.OTYPE.OTYPENAME == "Export" || item.OTYPE.OTYPENAME == "Local" || item.OTYPE.OTYPENAME == "Bulk"))
                //{
                //    item.OPT1 = await _denimDbContext.COM_EX_PI_DETAILS
                //        .Where(c => c.TRNSID.Equals(item.ORDERNO))
                //        .Select(c => c.SO_NO)
                //        .FirstOrDefaultAsync();
                //}
                #endregion
                
                return rollDetails;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<TypeTableViewModel>> GetOrderNoDataAsync(string orderType)
        {
            try
            {
                var orderId = int.Parse(orderType);

                List<TypeTableViewModel> result;
                var orderIds = new[] { 401, 402, 419, 422 };

                if (orderIds.Contains(orderId))
                {
                    result = await DenimDbContext.COM_EX_PI_DETAILS.Select(c => new TypeTableViewModel()
                    {
                        Name = c.SO_NO,
                        ID = c.TRNSID
                    }).OrderByDescending(c => c.Name).ToListAsync();
                }
                else
                {
                    result = await DenimDbContext.RND_SAMPLE_INFO_DYEING.Select(c => new TypeTableViewModel()
                    {
                        Name = c.RSOrder,
                        ID = c.SDID
                    }).OrderByDescending(c => c.Name).ToListAsync();
                }
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
