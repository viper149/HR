using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOS_PRECOSTING_MASTER_Repository : BaseRepository<COS_PRECOSTING_MASTER>, ICOS_PRECOSTING_MASTER
    {
        public SQLCOS_PRECOSTING_MASTER_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
        public async Task<int> InsertByAndReturnIdAsync(COS_PRECOSTING_MASTER cosPreCostingMaster)
        {
            try
            {
                cosPreCostingMaster.COLORCODE = DenimDbContext.RND_FABRICINFO.FirstOrDefaultAsync(c => c.FABCODE.Equals(cosPreCostingMaster.FABCODE)).Result.COLORCODE;

                var entityEntry = await DenimDbContext.COS_PRECOSTING_MASTER.AddAsync(cosPreCostingMaster);
                await SaveChanges();
                return entityEntry.Entity.CSID;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<COS_PRECOSTING_MASTER> FindByIdAllAsync(int csId)
        {
            try
            {
                return await DenimDbContext.COS_PRECOSTING_MASTER
                    .Include(c => c.Color)
                    .Include(c => c.Weave)
                    .Include(c => c.FinishType)
                    .Include(c => c.FABCODENavigation)
                    .ThenInclude(c => c.WV)
                    .Include(c => c.C)
                    .Where(c => c.CSID == csId).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<CosPreCostingMasterViewModel> GetInitObjects(CosPreCostingMasterViewModel cosPreCostingMasterViewModel)
        {
            try
            {
                cosPreCostingMasterViewModel.Colors = await DenimDbContext.BAS_COLOR.OrderBy(e => e.COLOR).ToListAsync();
                cosPreCostingMasterViewModel.FabricInfos = await DenimDbContext.RND_FABRICINFO
                    .Select(c => new RND_FABRICINFO
                    {
                        FABCODE = c.FABCODE,
                        STYLE_NAME = c.STYLE_NAME
                    }).OrderBy(e => e.STYLE_NAME).ToListAsync();

                cosPreCostingMasterViewModel.Weaves = await DenimDbContext.RND_WEAVE.ToListAsync();
                cosPreCostingMasterViewModel.LoomTypes = await DenimDbContext.LOOM_TYPE.ToListAsync();
                cosPreCostingMasterViewModel.FinishTypes = await DenimDbContext.RND_FINISHTYPE.Select(e => new RND_FINISHTYPE
                {
                    FINID = e.FINID,
                    TYPENAME = e.TYPENAME
                }).OrderBy(e => e.TYPENAME).ToListAsync();

                cosPreCostingMasterViewModel.YarnCountInfos = await DenimDbContext.BAS_YARN_COUNTINFO.Where(c => !c.YARN_CAT_ID.Equals(8102699)).ToListAsync();
                cosPreCostingMasterViewModel.ComTenors = await DenimDbContext.COM_TENOR.ToListAsync();
                cosPreCostingMasterViewModel.CosCertificationCosts = await DenimDbContext.COS_CERTIFICATION_COST.ToListAsync();
                cosPreCostingMasterViewModel.SupplierInfos = await DenimDbContext.BAS_SUPPLIERINFO.ToListAsync();
                cosPreCostingMasterViewModel.YarnFors = await DenimDbContext.YARNFOR.ToListAsync();
                cosPreCostingMasterViewModel.StandardCons = await DenimDbContext.COS_STANDARD_CONS.OrderByDescending(c => c.CREATED_AT).FirstOrDefaultAsync(c => c.STATUS == true);
                cosPreCostingMasterViewModel.FixedCost = await DenimDbContext.COS_FIXEDCOST.OrderByDescending(c => c.CREATED_AT).FirstOrDefaultAsync(c => c.STATUS == true);

                return cosPreCostingMasterViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<COS_PRECOSTING_MASTER>> GetAllAsync()
        {
            try
            {
                var result = await DenimDbContext.COS_PRECOSTING_MASTER
                    .Include(c => c.FABCODENavigation)
                    .ThenInclude(c => c.WV)
                    .Include(c => c.COM_EX_PI_DETAILS)
                    .Where(c => !c.CSID.Equals(250023))
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
