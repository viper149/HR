using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.PostCosting;
using DenimERP.ViewModels;
using DenimERP.ViewModels.PostCostingMaster;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.PostCosting
{
    public class SQLCOS_POSTCOSTING_MASTER_Repository:BaseRepository<COS_POSTCOSTING_MASTER>, ICOS_POSTCOSTING_MASTER
    {

        private readonly IDataProtector _protector;

        public SQLCOS_POSTCOSTING_MASTER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)


        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<PostCostingViewModel> GetInitObj(PostCostingViewModel postCostingViewModel)
        {
            try
            {
                postCostingViewModel.ChemicalList = await DenimDbContext.F_CHEM_STORE_PRODUCTINFO.ToListAsync();
                postCostingViewModel.CountList = await DenimDbContext.BAS_YARN_COUNTINFO.ToListAsync();
                postCostingViewModel.LotList = await DenimDbContext.BAS_YARN_LOTINFO.ToListAsync();
                postCostingViewModel.SOList = await DenimDbContext.RND_PRODUCTION_ORDER
                    .Include(c => c.SO)
                    .Where(c=>c.ORDERNO!=null)
                    .Select(c => new TypeTableViewModel
                    {
                        ID = c.POID,
                        Name = $"{c.SO.SO_NO}({c.SO.STYLE.FABCODENavigation.STYLE_NAME})"
                    }).ToListAsync();
                postCostingViewModel.UnitList = await DenimDbContext.F_BAS_UNITS.ToListAsync();

                return postCostingViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<PostCostingViewModel> GetInfoAsync(PostCostingViewModel postCostingViewModel)
        {
            try
            {
                foreach (var item in postCostingViewModel.CosPostCostingYarnDetailsList)
                {
                    item.COUNT = await DenimDbContext.BAS_YARN_COUNTINFO.FindAsync(item.COUNTID);
                    item.LOT = await DenimDbContext.BAS_YARN_LOTINFO.FindAsync(item.LOTID);
                    item.YarnFor = await DenimDbContext.YARNFOR.FindAsync(item.YARNFOR);
                }
                foreach (var item in postCostingViewModel.CosPostCostingChemDetailsList)
                {
                    item.CHEM_PROD = await DenimDbContext.F_CHEM_STORE_PRODUCTINFO.FindAsync(item.CHEM_PRODID);
                    item.UNITNavigation = await DenimDbContext.F_BAS_UNITS.FindAsync(item.UNIT);
                }
                return postCostingViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<COS_POSTCOSTING_MASTER>> GetAllAsync()
        {
            return await DenimDbContext.COS_POSTCOSTING_MASTER
                    .Include(c => c.RndProductionOrder.SO.STYLE.FABCODENavigation)
                    .Include(c => c.RndProductionOrder.SO.PIMASTER.BUYER)
                     .Select(c => new COS_POSTCOSTING_MASTER
                     {
                         PCOSTID = c.PCOSTID,
                         EncryptedId = _protector.Protect(c.PCOSTID.ToString()),
                         PRODUCTION_QTY = c.PRODUCTION_QTY,
                         TRNSDATE=c.TRNSDATE,
                         REMARKS = c.REMARKS,
                         RndProductionOrder = new RND_PRODUCTION_ORDER
                         {
                             SO = new COM_EX_PI_DETAILS
                             {
                                 SO_NO = c.RndProductionOrder.SO.SO_NO,
                                 QTY = c.RndProductionOrder.SO.QTY,
                                 STYLE = new COM_EX_FABSTYLE
                                 {
                                     FABCODENavigation = new RND_FABRICINFO
                                     {
                                         STYLE_NAME = c.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME
                                     }
                                 },
                                 PIMASTER = new COM_EX_PIMASTER
                                 {
                                     PINO = c.RndProductionOrder.SO.PIMASTER.PINO,
                                     BUYER = new BAS_BUYERINFO
                                     {
                                         BUYER_NAME = c.RndProductionOrder.SO.PIMASTER.BUYER.BUYER_NAME
                                     }
                                 }
                             }
                         }
                     }).ToListAsync();
                    
        }


        public async Task<PostCostingViewModel> FindAllByIdAsync(int id)
        {
            try
            {
                var result = new PostCostingViewModel
                {
                    CosPostcostingMaster = await DenimDbContext.COS_POSTCOSTING_MASTER
                        .AsNoTracking()
                        .Where(c => c.PCOSTID.Equals(id)).FirstOrDefaultAsync(),
                    CosPostCostingYarnDetailsList = await DenimDbContext.COS_POSTCOSTING_YARNDETAILS
                        .Include(c=>c.YarnFor)
                        .Include(c=>c.COUNT)
                        .Include(c=>c.LOT)
                        .AsNoTracking()
                        .Where(c => c.PCOSTID.Equals(id)).ToListAsync(),
                    CosPostCostingChemDetailsList = await DenimDbContext
                        .COS_POSTCOSTING_CHEMDETAILS
                        .Include(c=>c.CHEM_PROD)
                        .Include(c=>c.UNITNavigation)
                        .AsNoTracking()
                        .Where(c => c.PCOSTID.Equals(id)).ToListAsync()
                };
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<double> GetSoEliteQty(int id)
        {
            try
            {
                var elite = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                    .Include(c => c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder)
                    .Where(c => c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.POID.Equals(id) && c.PROCESS_TYPE==1 && c.FAB_GRADE =="A" ).ToListAsync();
                return elite.Sum(c=>c.LENGTH_YDS)??0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
