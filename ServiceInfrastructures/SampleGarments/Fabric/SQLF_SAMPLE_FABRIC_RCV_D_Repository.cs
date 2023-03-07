using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.SampleGarments.Fabric;

namespace DenimERP.ServiceInfrastructures.SampleGarments.Fabric
{
    public class SQLF_SAMPLE_FABRIC_RCV_D_Repository : BaseRepository<F_SAMPLE_FABRIC_RCV_D>, IF_SAMPLE_FABRIC_RCV_D
    {
        public SQLF_SAMPLE_FABRIC_RCV_D_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
