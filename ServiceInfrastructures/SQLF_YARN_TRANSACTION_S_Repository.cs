using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_YARN_TRANSACTION_S_Repository : BaseRepository<F_YARN_TRANSACTION_S>, IF_YARN_TRANSACTION_S
    {
        public SQLF_YARN_TRANSACTION_S_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<double?> GetLastBalanceByCountIdAsync(int? countId, int lotId)
        {
            var result = await DenimDbContext.F_YARN_TRANSACTION_S
                .Where(d => d.COUNTID.Equals(countId) && d.LOTID.Equals(lotId))
                .OrderBy(d => d.YTRNID)
                .Select(c => c.BALANCE)
                .LastOrDefaultAsync();

            return result ?? 0;
        }
    }
}
