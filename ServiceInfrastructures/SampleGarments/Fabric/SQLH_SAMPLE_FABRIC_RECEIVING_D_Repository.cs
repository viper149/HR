using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.SampleGarments.Fabric;

namespace DenimERP.ServiceInfrastructures.SampleGarments.Fabric
{
    public class SQLH_SAMPLE_FABRIC_RECEIVING_D_Repository : BaseRepository<H_SAMPLE_FABRIC_RECEIVING_D>, IH_SAMPLE_FABRIC_RECEIVING_D
    {
        public SQLH_SAMPLE_FABRIC_RECEIVING_D_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
