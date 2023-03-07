using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_GEN_S_MRR_Repository : BaseRepository<F_GEN_S_MRR>, IF_GEN_S_MRR
    {
        public SQLF_GEN_S_MRR_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<int> GetLastMrrNo()
        {
            var result = await DenimDbContext.F_GEN_S_MRR.Select(d => d.GSMRRNO).OrderByDescending(d => d.Value)
                .FirstOrDefaultAsync();
            return result ?? 20000;
        }

        public async Task<F_GEN_S_MRR> GetMrrDetails()
        {
            return await DenimDbContext.F_GEN_S_MRR
                .Select(d => new F_GEN_S_MRR
                {
                    GSMRRID = d.GSMRRID
                }).OrderByDescending(d => d.GSMRRID).FirstOrDefaultAsync();
        }
    }
}
