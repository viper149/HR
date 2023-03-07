using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLRND_SAMPLE_INFO_WEAVING_DETAILS_Repository : BaseRepository<RND_SAMPLE_INFO_WEAVING_DETAILS>, IRND_SAMPLE_INFO_WEAVING_DETAILS
    {
        public SQLRND_SAMPLE_INFO_WEAVING_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
