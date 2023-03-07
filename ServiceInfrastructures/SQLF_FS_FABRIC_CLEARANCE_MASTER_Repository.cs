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
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_FS_FABRIC_CLEARANCE_MASTER_Repository : BaseRepository<F_FS_FABRIC_CLEARANCE_MASTER>, IF_FS_FABRIC_CLEARANCE_MASTER
    {
        private readonly IDataProtector _protector;

        public SQLF_FS_FABRIC_CLEARANCE_MASTER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_FS_FABRIC_CLEARANCE_MASTER>> GetAllAsync()
        {
            return await DenimDbContext.F_FS_FABRIC_CLEARANCE_MASTER
                .Include(c => c.FABCODENavigation.COLORCODENavigation)
                .Include(c => c.PO.SO)
                .Select(e => new F_FS_FABRIC_CLEARANCE_MASTER
                {
                    CLID = e.CLID,
                    EncryptedId = _protector.Protect(e.CLID.ToString()),
                    WASH_CODE = e.WASH_CODE,
                    REMARKS = e.REMARKS,
                    FABCODENavigation = new RND_FABRICINFO
                    {
                        STYLE_NAME = $"{e.FABCODENavigation.STYLE_NAME}",
                        COLORCODENavigation = new BAS_COLOR
                        {
                            COLOR = $"{e.FABCODENavigation.COLORCODENavigation.COLOR}"
                        }
                    },
                    PO = new RND_PRODUCTION_ORDER
                    {
                        SO = new COM_EX_PI_DETAILS
                        {
                            SO_NO = $"{e.PO.SO.SO_NO}"
                        }
                    }
                })
                .OrderByDescending(c=>c.FABCODENavigation.STYLE_NAME).ThenByDescending(c=>c.WASH_CODE)
                .ToListAsync();
        }


        public async Task<FFsFabricClearanceViewModel> GetInitObjects(FFsFabricClearanceViewModel fFsFabricClearanceViewModel)
        {
            try
            {
                fFsFabricClearanceViewModel.RndFabricInfoList = await DenimDbContext.RND_FABRICINFO.Select(c =>
                    new RND_FABRICINFO
                    {
                        FABCODE = c.FABCODE,
                        STYLE_NAME = c.STYLE_NAME
                    }).ToListAsync();

                fFsFabricClearanceViewModel.BasBuyerInfos = await DenimDbContext.BAS_BUYERINFO.Select(c =>
                    new BAS_BUYERINFO
                    {
                        BUYERID = c.BUYERID,
                        BUYER_NAME = c.BUYER_NAME
                    }).ToListAsync();

                fFsFabricClearanceViewModel.RndProductionOrders = await DenimDbContext.RND_PRODUCTION_ORDER.Include(c => c.SO.STYLE.FABCODENavigation).Select(c =>
                      new RND_PRODUCTION_ORDER()
                      {
                          POID = c.POID,
                          OPT1 = $"{c.SO.SO_NO} ({c.SO.STYLE.FABCODENavigation.STYLE_NAME})"
                      }).ToListAsync();

                return fFsFabricClearanceViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<bool> WashcodeAnyAsync(FFsFabricClearanceViewModel fFsFabricClearanceViewModel)
        {
            try
            {
                var flag = await DenimDbContext.F_FS_FABRIC_CLEARANCE_MASTER.AnyAsync(c =>
                    c.FABCODE.Equals(fFsFabricClearanceViewModel.FFsFabricClearanceMaster.FABCODE) &&
                    c.WASH_CODE.Equals(fFsFabricClearanceViewModel.FFsFabricClearanceMaster.WASH_CODE));

                return flag;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<dynamic> GetOrderDetaiils(int id)
        {
            try
            {
                var result = await DenimDbContext.RND_PRODUCTION_ORDER
                    .Include(c => c.SO.STYLE.FABCODENavigation.COLORCODENavigation)
                    .Include(c => c.SO.PIMASTER)
                    .Where(c => c.POID.Equals(id))
                    .Select(c => new
                    {
                        StyleName = c.SO.STYLE.FABCODENavigation.STYLE_NAME,
                        FabWidth = c.SO.STYLE.FABCODENavigation.WIDEC,
                        Color = c.SO.STYLE.FABCODENavigation.COLORCODENavigation.COLOR,
                        SoNo = c.SO.SO_NO,
                        Buyer = c.SO.PIMASTER.BUYERID,
                        Fabcode = c.SO.STYLE.FABCODENavigation.FABCODE
                    }).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FFsFabricClearanceViewModel> FindAllByIdAsync(int clId)
        {
            try
            {
                var result = await DenimDbContext.F_FS_FABRIC_CLEARANCE_MASTER
                    .Include(c => c.F_FS_FABRIC_CLEARANCE_DETAILS)
                    .ThenInclude(c => c.ROLL_.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation)
                    .Where(c => c.CLID.Equals(clId))
                    .Select(c => new FFsFabricClearanceViewModel
                    {
                        FFsFabricClearanceMaster = new F_FS_FABRIC_CLEARANCE_MASTER
                        {
                            CLID = c.CLID,
                            BUYERID = c.BUYERID,
                            FACTORYID = c.FACTORYID,
                            TRNSDATE = c.TRNSDATE,
                            FABCODE = c.FABCODE,
                            WASH_CODE = c.WASH_CODE,
                            PACKING_LIST_DATE = c.PACKING_LIST_DATE,
                            ORDER_NO = c.ORDER_NO,
                            DATE_FROM = c.DATE_FROM,
                            DATE_TO = c.DATE_TO,
                            ROLE_QTY = c.ROLE_QTY,
                            ROLE_FROM = c.ROLE_FROM,
                            ROLE_TO = c.ROLE_TO,
                            SHIFT = c.SHIFT,
                            REMARKS = c.REMARKS,
                            OPT5 = c.OPT5,
                            OPT4 = c.OPT4,
                            OPT3 = c.OPT3,
                            OPT2 = c.OPT2,
                            OPT1 = c.OPT1,
                            CREATED_AT = c.CREATED_AT,
                            CREATED_BY = c.CREATED_BY,
                            UPDATED_AT = c.UPDATED_AT,
                            UPDATED_BY = c.UPDATED_BY
                        },
                        FFsFabricClearanceDetailsList = c.F_FS_FABRIC_CLEARANCE_DETAILS.OrderBy(cm => cm.ROLL_.ROLLNO).Select(d => new F_FS_FABRIC_CLEARANCE_DETAILS
                        {
                            CL_D_ID = d.CL_D_ID,
                            CLID = d.CLID,
                            ROLL_ = d.ROLL_,
                            ROLL_ID = d.ROLL_ID,
                            PROD_DATE = d.PROD_DATE,
                            SHADE_GROUP = d.SHADE_GROUP,
                            SHRINKAGE_WARP = d.SHRINKAGE_WARP,
                            SHRINKAGE_WEFT = d.SHRINKAGE_WEFT,
                            WGBW = d.WGBW,
                            WGAW = d.WGAW,
                            PICK_AW = d.PICK_AW,
                            PICK_BW = d.PICK_BW,
                            STATUS = d.STATUS,
                            REMARKS = d.REMARKS,
                            INSPECTION_REMARKS = d.INSPECTION_REMARKS
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

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
