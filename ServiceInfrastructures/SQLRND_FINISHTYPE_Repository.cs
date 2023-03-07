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
    public class SQLRND_FINISHTYPE_Repository: BaseRepository<RND_FINISHTYPE>, IRND_FINISHTYPE
    {
        public SQLRND_FINISHTYPE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
        
        public async Task<bool> FindByTypeName(string typeName)
        {
            try
            {
                return await DenimDbContext.RND_FINISHTYPE.Where(sc => sc.TYPENAME.Equals(typeName)).AnyAsync();

            }
            catch (Exception ex)
            {
                throw new System.InvalidOperationException(ex.Message);
            }
        }

        public async Task<IEnumerable<RND_FINISHTYPE>> GetRndFinishTypeWithPaged(int pageNumber, int pageSize)
        {
            try
            {
                int ExcludeResult = (pageSize * pageNumber) - pageSize;

                var result = await DenimDbContext.RND_FINISHTYPE
                    .OrderByDescending(sc => sc.FINID)
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
