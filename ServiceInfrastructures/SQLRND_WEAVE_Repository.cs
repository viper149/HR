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
    public class SQLRND_WEAVE_Repository: BaseRepository<RND_WEAVE>, IRND_WEAVE
    {
        public SQLRND_WEAVE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
        public async Task<bool> FindByTypeName(string name)
        {
            try
            {
                return await DenimDbContext.RND_WEAVE.Where(sc => sc.NAME.Equals(name)).AnyAsync();
            }
            catch (Exception ex)
            {
                throw new System.InvalidOperationException(ex.Message);
            }
        }

        public async Task<IEnumerable<RND_WEAVE>> GetRndWeaveWithPaged(int pageNumber, int pageSize)
        {
            try
            {
                int ExcludeResult = (pageSize * pageNumber) - pageSize;

                var result = await DenimDbContext.RND_WEAVE
                    .OrderByDescending(sc => sc.WID)
                    .Skip(ExcludeResult)
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
