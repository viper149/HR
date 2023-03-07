using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.SampleGarments.HDispatch;

namespace DenimERP.ServiceInfrastructures.SampleGarments.HDispatch
{
    public class SQLH_SAMPLE_PARTY_Repository : BaseRepository<H_SAMPLE_PARTY>, IH_SAMPLE_PARTY
    {
        public SQLH_SAMPLE_PARTY_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
