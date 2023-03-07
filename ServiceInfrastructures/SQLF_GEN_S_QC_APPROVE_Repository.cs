using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_GEN_S_QC_APPROVE_Repository : BaseRepository<F_GEN_S_QC_APPROVE>, IF_GEN_S_QC_APPROVE
    {
        public SQLF_GEN_S_QC_APPROVE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<int> GetLastQCANo()
        {
            var result = await DenimDbContext.F_GEN_S_QC_APPROVE.Select(d => d.GSQCANO).OrderByDescending(d => d.Value)
                .FirstOrDefaultAsync();
            return result ?? 10000;
        }

        public async Task<F_GEN_S_QC_APPROVE> GetQcDetails()
        {
            return await DenimDbContext.F_GEN_S_QC_APPROVE
                .Select(d => new F_GEN_S_QC_APPROVE
                {
                    GSQCAID = d.GSQCAID
                }).OrderByDescending(d=>d.GSQCAID).FirstOrDefaultAsync();
        }
    }
}
