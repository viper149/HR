using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.SampleGarments.HReceive;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.SampleGarments.HReceive
{
    public class SQLH_SAMPLE_RECEIVING_D_Repository : BaseRepository<H_SAMPLE_RECEIVING_D>, IH_SAMPLE_RECEIVING_D
    {
        public SQLH_SAMPLE_RECEIVING_D_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<double?> GetAvailableQty(int rcvdId)
        {
            var hSampleDespatchDs = DenimDbContext.H_SAMPLE_DESPATCH_D.Where(e => e.RCVDID.Equals(rcvdId));
            var hSampleReceivingDs = DenimDbContext.H_SAMPLE_RECEIVING_D.Where(e => e.RCVDID.Equals(rcvdId));

            var queryable = await hSampleReceivingDs.SumAsync(e => e.QTY) - await hSampleDespatchDs.SumAsync(f => f.QTY);

            return queryable;
        }
    }
}
