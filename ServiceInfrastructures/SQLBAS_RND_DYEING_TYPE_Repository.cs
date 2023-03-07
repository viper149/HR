using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLBAS_RND_DYEING_TYPE_Repository : BaseRepository<RND_DYEING_TYPE>, IRND_DYEING_TYPE
    {
        public SQLBAS_RND_DYEING_TYPE_Repository(DenimDbContext denimDbContext) 
            : base(denimDbContext)
        {
        }
    }
}
