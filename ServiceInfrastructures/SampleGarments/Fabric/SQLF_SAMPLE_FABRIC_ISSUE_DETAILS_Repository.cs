using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.SampleGarments.Fabric;

namespace DenimERP.ServiceInfrastructures.SampleGarments.Fabric
{
    public class SQLF_SAMPLE_FABRIC_ISSUE_DETAILS_Repository : BaseRepository<F_SAMPLE_FABRIC_ISSUE_DETAILS>, IF_SAMPLE_FABRIC_ISSUE_DETAILS
    {
        public SQLF_SAMPLE_FABRIC_ISSUE_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
