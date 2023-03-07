using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_YS_YARN_RECEIVE_REPORT_Repository : BaseRepository<F_YS_YARN_RECEIVE_REPORT>, IF_YS_YARN_RECEIVE_REPORT
    {
        public SQLF_YS_YARN_RECEIVE_REPORT_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<int> GetLastMrrNo()
        {
            try
            {
                var result = await DenimDbContext.F_YS_YARN_RECEIVE_REPORT.Select(e => e.MRRNO).OrderByDescending(e => e.Value).FirstOrDefaultAsync();
                return result ?? 1000 + 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
