using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.SampleGarments.Receive;

namespace DenimERP.ServiceInfrastructures.SampleGarments.Receive
{
    public class SQLF_SAMPLE_ITEM_DETAILS_Repository : BaseRepository<F_SAMPLE_ITEM_DETAILS>, IF_SAMPLE_ITEM_DETAILS
    {
        public SQLF_SAMPLE_ITEM_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
