using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_YS_YARN_RECEIVE_REPORT_S_Repository : BaseRepository<F_YS_YARN_RECEIVE_REPORT_S>, IF_YS_YARN_RECEIVE_REPORT_S
    {
        public SQLF_YS_YARN_RECEIVE_REPORT_S_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<int> GetLastMrrNo()
        {
            var result = await DenimDbContext.F_YS_YARN_RECEIVE_REPORT_S.Select(e => e.MRRNO)
                .OrderByDescending(e => e.Value).FirstOrDefaultAsync();
            return result ?? 1000 + 1;
        }
    }
}
