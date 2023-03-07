using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInfrastructures.SampleGarments.HDispatch;

namespace DenimERP.ServiceInterfaces.SampleGarments.HDispatch
{
    public class SQLH_SAMPLE_TEAM_DETAILS_Repository : BaseRepository<H_SAMPLE_TEAM_DETAILS>, IH_SAMPLE_TEAM_DETAILS
    {
        public SQLH_SAMPLE_TEAM_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
