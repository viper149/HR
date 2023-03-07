using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.SampleGarments.Fabric;

namespace DenimERP.ServiceInfrastructures.SampleGarments.Fabric
{
    public class SQLH_SAMPLE_FABRIC_DISPATCH_DETAILS_Repository : BaseRepository<H_SAMPLE_FABRIC_DISPATCH_DETAILS>, IH_SAMPLE_FABRIC_DISPATCH_DETAILS
    {
        public SQLH_SAMPLE_FABRIC_DISPATCH_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
