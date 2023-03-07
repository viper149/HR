using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.SampleGarments.Fabric;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.SampleGarments.Fabric
{
    public class SQLF_SAMPLE_FABRIC_DISPATCH_TRANSACTION_Repository : BaseRepository<F_SAMPLE_FABRIC_DISPATCH_TRANSACTION>, IF_SAMPLE_FABRIC_DISPATCH_TRANSACTION
    {
        public SQLF_SAMPLE_FABRIC_DISPATCH_TRANSACTION_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<double?> FindByDpIdAsync(int dpId, int trnsId)
        {
            var fSampleFabricDispatchTransaction = await DenimDbContext.F_SAMPLE_FABRIC_DISPATCH_TRANSACTION
                .Where(e => e.DPDID.Equals(dpId))
                .OrderByDescending(e => e.TID)
                .FirstOrDefaultAsync();

            switch (fSampleFabricDispatchTransaction)
            {
                case null:
                    {
                        var fSampleFabricRcvD = await DenimDbContext.F_SAMPLE_FABRIC_RCV_D.FirstOrDefaultAsync(e => e.TRNSID.Equals(trnsId));
                        return fSampleFabricRcvD.QTY;
                    }
                default:
                    return fSampleFabricDispatchTransaction.BALANCE;
            }
        }
    }
}