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
    public class SQLBAS_YARN_LOTINFO_Repository : BaseRepository<BAS_YARN_LOTINFO>, IBAS_YARN_LOTINFO
    {
        public SQLBAS_YARN_LOTINFO_Repository(DenimDbContext denimDbContext)
            : base(denimDbContext)
        {
        }

        public async Task<string> FindLotNoByIdAsync(int id)
        {
            var lotNo = await DenimDbContext.BAS_YARN_LOTINFO.Where(ln => ln.LOTID == id).Select(e => e.LOTNO).ToListAsync();
            return lotNo.FirstOrDefault();
        }

        public async Task<IEnumerable<BAS_YARN_LOTINFO>> GetForSelectItemsByAsync()
        {
            return await DenimDbContext.BAS_YARN_LOTINFO.Select(e => new BAS_YARN_LOTINFO
            {
                LOTID = e.LOTID,
                LOTNO = e.LOTNO
            }).ToListAsync();
        }

        public async Task<IEnumerable<BAS_YARN_LOTINFO>> GetBasYarnLotInfoWithPaged(int pageNumber = 1, int pageSize = 5)
        {
            var excludeResult = (pageSize * pageNumber) - pageSize;

            var result = await DenimDbContext.BAS_YARN_LOTINFO
                .OrderByDescending(yl => yl.LOTID)
                .Skip(excludeResult)
                .Take(pageSize)
                .ToListAsync();

            return result;
        }

        public async Task<int> TotalNumberOfBasYarnLot()
        {
            var result = await DenimDbContext.BAS_YARN_LOTINFO.ToListAsync();
            return result.Count();
        }
    }
}
