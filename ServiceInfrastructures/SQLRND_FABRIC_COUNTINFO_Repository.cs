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
    public class SQLRND_FABRIC_COUNTINFO_Repository : BaseRepository<RND_FABRIC_COUNTINFO>, IRND_FABRIC_COUNTINFO
    {
        public SQLRND_FABRIC_COUNTINFO_Repository(DenimDbContext denimDbContext)
            : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<RndFabricCountInfoViewModel>> FindByFabCodeIAllAsync(int fabCode)
        {
            var result = await DenimDbContext.RND_FABRIC_COUNTINFO
               .Include(c => c.COUNT)
               .Include(c => c.LOT)
               .Include(c => c.SUPP)
               .Include(c => c.FABCODENavigation)
               .ThenInclude(c => c.RND_YARNCONSUMPTION)
               .Select(c => new RndFabricCountInfoViewModel
               {
                   RndFabricCountinfo = c,
                   YARNFOR = c.YARNFOR,
                   AMOUNT = Math.Round(c.FABCODENavigation.RND_YARNCONSUMPTION.Where(e => e.COUNTID.Equals(c.COUNTID) && e.FABCODE.Equals(c.FABCODE) && e.YARNFOR.Equals(c.YARNFOR) && e.COLOR.Equals(c.COLORCODE)).Select(e => e.AMOUNT ?? 0).FirstOrDefault(),4)
               }).Where(e => e.RndFabricCountinfo.FABCODE.Equals(fabCode)).ToListAsync();

            return result;
        }

        public async Task<RND_FABRIC_COUNTINFO> GetLotFromRNDFCI(int count)
        {
            var result = await DenimDbContext.RND_FABRIC_COUNTINFO
                .Include(e => e.LOT)
                .Select(e => new RND_FABRIC_COUNTINFO
                {
                    TRNSID = e.TRNSID,
                    LOT = new BAS_YARN_LOTINFO
                    {
                        LOTNO = e.LOT.LOTNO
                    }
                }).FirstOrDefaultAsync(e => e.TRNSID.Equals(count));

            return result;
        }

        public async Task<IEnumerable<RND_FABRIC_COUNTINFO>> FindByFabCodeIAsync(int fabCode)
        {
            return await DenimDbContext.RND_FABRIC_COUNTINFO
                .Where(rfc => rfc.FABCODE.Equals(fabCode)).AsNoTracking()
                .ToListAsync();
        }

        public async Task<RndFabricInfoViewModel> GetCountDetailsInfo(RndFabricInfoViewModel rndFabricInfoViewModel)
        {
            try
            {
                foreach (var item in rndFabricInfoViewModel.RndFabricCountInfos)
                {
                    item.COUNT = await DenimDbContext.BAS_YARN_COUNTINFO.FirstOrDefaultAsync(c=>c.COUNTID.Equals(item.COUNTID));
                    item.LOT = await DenimDbContext.BAS_YARN_LOTINFO.FirstOrDefaultAsync(c=>c.LOTID.Equals(item.LOTID));
                    item.SUPP = await DenimDbContext.BAS_SUPPLIERINFO.FirstOrDefaultAsync(c=>c.SUPPID.Equals(item.SUPPID));
                    item.YarnFor = await DenimDbContext.YARNFOR.FirstOrDefaultAsync(c=>c.YARNID.Equals(item.YARNFOR));
                    item.Color = await DenimDbContext.BAS_COLOR.FirstOrDefaultAsync(c=>c.COLORCODE.Equals(item.COLORCODE));
                }

                return rndFabricInfoViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
