using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLRND_FINISHMC_Repository : BaseRepository<RND_FINISHMC>, IRND_FINISHMC
    {
        public SQLRND_FINISHMC_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
        public async Task<bool> FindByTypeName(string name)
        {
            try
            {
                return await DenimDbContext.RND_FINISHMC.Where(sc => sc.NAME.Equals(name)).AnyAsync();
            }
            catch (Exception ex)
            {
                throw new System.InvalidOperationException(ex.Message);
            }
        }

        public async Task<IEnumerable<RND_SAMPLE_INFO_WEAVING>> GetRndSampleInfoWeavingsByAsync()
        {
            return await DenimDbContext.RND_SAMPLE_INFO_WEAVING
                .Select(e => new RND_SAMPLE_INFO_WEAVING
                {
                    WVID = e.WVID,
                    FABCODE = e.FABCODE
                }).OrderBy(e => e.FABCODE).ToListAsync();
        }

        public async Task<IEnumerable<RND_FINISHMC>> GetRndFinishMcWithPaged(int pageNumber, int pageSize)
        {
            try
            {
                var excludeResult = (pageSize * pageNumber) - pageSize;

                var result = await DenimDbContext.RND_FINISHMC
                    .OrderByDescending(sc => sc.MCID)
                    .Skip(excludeResult)
                    .Take(pageSize)
                    .ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new System.InvalidOperationException(ex.Message);
            }
        }
    }
}
