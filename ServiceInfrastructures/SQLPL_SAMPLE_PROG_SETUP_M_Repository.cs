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
using DenimERP.ViewModels.Planning;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLPL_SAMPLE_PROG_SETUP_M_Repository: BaseRepository<PL_BULK_PROG_SETUP_M>, IPL_SAMPLE_PROG_SETUP_M
    {
        private readonly IDataProtector _protector;

        public SQLPL_SAMPLE_PROG_SETUP_M_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<PlBulkProgSetupViewModel> GetInitObjects(PlBulkProgSetupViewModel plBulkProgSetupViewModel)
        {
            try
            {
                plBulkProgSetupViewModel.Yarnfors = await DenimDbContext.YARNFOR.Select(e => new YARNFOR
                {
                    YARNID = e.YARNID,
                    YARNNAME = e.YARNNAME
                }).OrderBy(e => e.YARNNAME).ToListAsync();

                plBulkProgSetupViewModel.RndFabricCountInfos = await DenimDbContext.RND_FABRIC_COUNTINFO.Select(c => new RND_FABRIC_COUNTINFO
                {
                    TRNSID = c.TRNSID,
                    COUNT = new BAS_YARN_COUNTINFO
                    {
                        COUNTNAME = c.COUNT.RND_COUNTNAME
                    }
                }).ToListAsync();

                plBulkProgSetupViewModel.BasYarnLotInfos = await DenimDbContext.BAS_YARN_LOTINFO.Select(c => new BAS_YARN_LOTINFO
                {
                    LOTID = c.LOTID,
                    LOTNO = $"-{c.LOTNO}"
                }).ToListAsync();

                plBulkProgSetupViewModel.PiDetailsList = await DenimDbContext.COM_EX_PI_DETAILS.Select(c => new COM_EX_PI_DETAILS
                {
                    TRNSID = c.TRNSID,
                    SO_NO = c.SO_NO
                }).ToListAsync();

                plBulkProgSetupViewModel.RndFabricInfos = await DenimDbContext.RND_FABRICINFO.Select(c => new RND_FABRICINFO
                {
                    FABCODE = c.FABCODE,
                    STYLE_NAME = c.STYLE_NAME
                }).ToListAsync();

                if (plBulkProgSetupViewModel.PlBulkProgSetupM != null)
                {
                    plBulkProgSetupViewModel.ProductionOrderList = await DenimDbContext.RND_PRODUCTION_ORDER
                            .Include(c => c.SO)
                            .Include(c => c.RS)
                            .Where(c => DenimDbContext.PL_BULK_PROG_SETUP_M.Any(e => e.ORDERNO.Equals(c.POID)))
                            .OrderByDescending(c => c.ORDERNO)
                            .Select(c => new TypeTableViewModel
                            {
                                Name = c.SO != null ? c.SO.SO_NO : c.RS != null ? c.RS.RSOrder ?? c.RS.DYEINGCODE : null,
                                ID = c.POID
                            }).ToListAsync();
                }
                else
                {
                    plBulkProgSetupViewModel.ProductionOrderList = await DenimDbContext.RND_PRODUCTION_ORDER
                            .Include(c => c.SO)
                            .Include(c => c.RS)
                            .Where(c => !DenimDbContext.PL_BULK_PROG_SETUP_M.Any(e => e.ORDERNO.Equals(c.POID)))
                            .OrderByDescending(c => c.ORDERNO)
                            .Select(c => new TypeTableViewModel
                            {
                                Name = c.SO != null ? c.SO.SO_NO : c.RS != null ? c.RS.RSOrder ?? c.RS.DYEINGCODE : null,
                                ID = c.POID
                            }).ToListAsync();
                }

                return plBulkProgSetupViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<PlBulkProgSetupViewModel> GetInitData(PlBulkProgSetupViewModel plBulkProgSetupViewModel)
        {
            try
            {
                foreach (var item in plBulkProgSetupViewModel.PlBulkProgSetupDList)
                {
                    item.YARNFOR = await DenimDbContext.YARNFOR.FirstOrDefaultAsync(c => c.YARNID.Equals(item.YARN_TYPE));

                    foreach (var i in item.PlBulkProgYarnDList)
                    {
                        if (i.COUNTID != null)
                        {
                            i.COUNT = await DenimDbContext.RND_FABRIC_COUNTINFO.Include(c => c.COUNT).Select(c => new RND_FABRIC_COUNTINFO
                            {
                                TRNSID = c.TRNSID,
                                COUNT = new BAS_YARN_COUNTINFO
                                {
                                    COUNTNAME = c.COUNT.RND_COUNTNAME
                                }
                            }).FirstOrDefaultAsync(c => c.TRNSID.Equals(i.COUNTID));
                        }

                        if (i.SCOUNTID != null)
                        {
                            i.SCOUNT = await DenimDbContext.RND_SAMPLE_INFO_DETAILS.Include(c => c.COUNT).Select(c => new RND_SAMPLE_INFO_DETAILS
                            {
                                TRNSID = c.TRNSID,
                                COUNT = new BAS_YARN_COUNTINFO
                                {
                                    COUNTNAME = c.COUNT.RND_COUNTNAME
                                }
                            }).FirstOrDefaultAsync(c => c.TRNSID.Equals(i.SCOUNTID));
                        }

                        i.LOT = await DenimDbContext.BAS_YARN_LOTINFO.Select(c => new BAS_YARN_LOTINFO
                        {
                            LOTID = c.LOTID,
                            LOTNO = c.LOTNO
                        }).FirstOrDefaultAsync(c => c.LOTID.Equals(i.LOTID));
                    }
                }

                plBulkProgSetupViewModel.BasYarnLotInfos = await DenimDbContext.BAS_YARN_LOTINFO
                    .Select(c => new BAS_YARN_LOTINFO
                    {
                        LOTNO = $"Lot: {c.LOTNO} - {c.BRAND}",
                        LOTID = c.LOTID
                    })
                    .ToListAsync();
                return plBulkProgSetupViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<PL_ORDERWISE_LOTINFO> GetLotDetailsFromLotwiseTable(string lotId)
        {
            try
            {
                var result = await DenimDbContext.PL_ORDERWISE_LOTINFO
                    .Include(c => c.SUPP)
                    .Include(c => c.YARNFOR)
                    .Select(c => new PL_ORDERWISE_LOTINFO
                    {
                        SUPP = c.SUPP,
                        LOTID = c.LOTID,
                        POID = c.POID,
                        YARNFOR = c.YARNFOR
                    })
                    .FirstOrDefaultAsync(c => c.LOTID.Equals(int.Parse(lotId)));
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<int> InsertAndGetIdAsync(PL_BULK_PROG_SETUP_M plBulkProgSetupM)
        {
            try
            {
                await DenimDbContext.PL_BULK_PROG_SETUP_M.AddAsync(plBulkProgSetupM);
                await SaveChanges();
                return plBulkProgSetupM.BLK_PROGID;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<PlBulkProgSetupViewModel> FindAllByIdAsync(int id)
        {
            try
            {
                var result = await DenimDbContext.PL_BULK_PROG_SETUP_M

                    .Include(c => c.PL_BULK_PROG_SETUP_D)
                    .ThenInclude(c => c.PL_BULK_PROG_YARN_D)
                    .ThenInclude(c => c.COUNT)

                    .Include(c => c.PL_BULK_PROG_SETUP_D)
                    .ThenInclude(c => c.PL_BULK_PROG_YARN_D)
                    .ThenInclude(c => c.LOT)

                    .Include(c => c.PL_BULK_PROG_SETUP_D)
                    .ThenInclude(c => c.PL_PRODUCTION_SETDISTRIBUTION)

                    .Include(c => c.PL_BULK_PROG_SETUP_D)
                    .ThenInclude(c => c.YARNFOR)

                    .Include(c => c.RndProductionOrder)
                    .FirstOrDefaultAsync(c => c.BLK_PROGID.Equals(id));

                var plBulkProgSetupViewModel = new PlBulkProgSetupViewModel
                {
                    PlBulkProgSetupM = result,
                    PlBulkProgSetupDList = result.PL_BULK_PROG_SETUP_D.ToList()
                };

                foreach (var item in result.PL_BULK_PROG_SETUP_D)
                {
                    item.PlBulkProgYarnDList = item.PL_BULK_PROG_YARN_D.ToList();
                }

                return plBulkProgSetupViewModel;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<PL_BULK_PROG_SETUP_M>> GetAllAsync()
        {
            try
            {
                var result = await DenimDbContext.PL_BULK_PROG_SETUP_M
                   .Include(c => c.RndProductionOrder.SO.STYLE.FABCODENavigation.LOOM)
                   .Include(c => c.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation)
                   .Include(c => c.RndProductionOrder.SO.PIMASTER.BRAND)
                   .Include(c => c.RndProductionOrder.SO.PIMASTER.BUYER)
                   .Include(c => c.RndProductionOrder.RS.SDRF)
                   .Include(c => c.PL_BULK_PROG_SETUP_D)
                   .Where(x=>x.RndProductionOrder.RSNO != null)
                   .Select(c => new PL_BULK_PROG_SETUP_M
                   {
                       BLK_PROGID = c.BLK_PROGID,
                       EncryptedId = _protector.Protect(c.BLK_PROGID.ToString()),
                       OPT1 = c.RndProductionOrder != null ? c.RndProductionOrder.SO != null ? c.RndProductionOrder.SO.SO_NO : c.RndProductionOrder.RS != null ? c.RndProductionOrder.RS.RSOrder : "No Order No" : "No PO",
                       WARP_QTY = c.WARP_QTY,
                       OPT2 = c.PL_BULK_PROG_SETUP_D.Select(e => e.PROCESS_TYPE).FirstOrDefault() ?? "No Set Assigned Yet",
                       RndProductionOrder = new RND_PRODUCTION_ORDER
                       {
                           ORDER_QTY_YDS = c.RndProductionOrder.ORDER_QTY_YDS,
                           ORDER_QTY_MTR = c.RndProductionOrder.ORDER_QTY_MTR,
                           OPT1 = c.RndProductionOrder != null ? c.RndProductionOrder.SO != null ? c.RndProductionOrder.SO.STYLE != null ? c.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME ?? "" : "" : "" : "",
                           OPT4 = c.RndProductionOrder != null ? c.RndProductionOrder.SO != null ? c.RndProductionOrder.SO.STYLE != null ? c.RndProductionOrder.SO.STYLE.FABCODENavigation.PROGNO ?? "" : "" : "" : "",
                           OPT2 = c.RndProductionOrder != null ? c.RndProductionOrder.SO != null ? c.RndProductionOrder.SO.STYLE.FABCODENavigation.LOOM.LOOM_TYPE_NAME : c.RndProductionOrder.RS != null && c.RndProductionOrder.RS.LOOM != null ? c.RndProductionOrder.RS.LOOM.LOOM_TYPE_NAME : "" : "",
                           OPT3 = c.RndProductionOrder != null ? c.RndProductionOrder.SO != null ? c.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation.COLOR : c.RndProductionOrder.RS != null && c.RndProductionOrder.RS.COLOR != null ? c.RndProductionOrder.RS.COLOR.COLOR : "" : "",
                           OPT5 = c.RndProductionOrder != null ? c.RndProductionOrder.SO != null ? c.RndProductionOrder.SO.PIMASTER != null ? c.RndProductionOrder.SO.PIMASTER.BUYER != null ? c.RndProductionOrder.SO.PIMASTER.BUYER.BUYER_NAME : "" : "" : "" : "",
                           OPT6 = c.RndProductionOrder != null ? c.RndProductionOrder.SO != null ? c.RndProductionOrder.SO.PIMASTER != null ? c.RndProductionOrder.SO.PIMASTER.BRAND != null ? c.RndProductionOrder.SO.PIMASTER.BRAND.BRANDNAME : "" : "" : "" : "",
                       },
                       PL_BULK_PROG_SETUP_D = c.PL_BULK_PROG_SETUP_D.Select(e => new PL_BULK_PROG_SETUP_D
                       {
                           PROCESS_TYPE = e.PROCESS_TYPE ?? "No Set Assigned Yet"
                       }).ToList()

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

        public async Task<IEnumerable<PL_BULK_PROG_SETUP_D>> GetAllSetAsync()
        {
            try
            {
                var result = await DenimDbContext.PL_BULK_PROG_SETUP_D
                    .Include(c => c.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.LOOM)
                    .Include(c => c.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation)
                    .Include(c => c.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BRAND)
                    .Include(c => c.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER)
                    .Include(c => c.BLK_PROG_.RndProductionOrder.RS.SDRF)
                    .Include(c => c.BLK_PROG_.PL_BULK_PROG_SETUP_D)
                    .Select(c => new PL_BULK_PROG_SETUP_D
                    {
                        PROG_ID = c.PROG_ID,
                        SET_QTY = c.SET_QTY,
                        PROG_NO = c.PROG_NO,
                        OPT1 = c.BLK_PROG_.WARP_QTY.ToString(),
                        OPT2 = c.BLK_PROG_.RndProductionOrder.SO.SO_NO,
                        OPT3 = c.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME,
                        OPT4 = c.BLK_PROG_.RndProductionOrder.SO.PIMASTER.PINO,
                        PROCESS_TYPE = c.PROCESS_TYPE ?? "No Set Assigned Yet",
                        REMARKS = c.REMARKS
                    })
                    .OrderByDescending(c => c.PROG_ID)
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
