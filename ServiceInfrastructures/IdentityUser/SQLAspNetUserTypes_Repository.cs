using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.IdentityUser;

namespace DenimERP.ServiceInfrastructures.IdentityUser
{
    public class SQLAspNetUserTypes_Repository : BaseRepository<AspNetUserTypes>, IAspNetUserTypes
    {
        public SQLAspNetUserTypes_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
