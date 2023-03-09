using HRMS.Data;
using HRMS.Models;
using HRMS.ServiceInfrastructures.BaseInfrastructures;
using HRMS.ServiceInterfaces.IdentityUser;

namespace HRMS.ServiceInfrastructures.IdentityUser
{
    public class SQLAspNetUserTypes_Repository : BaseRepository<AspNetUserTypes>, IAspNetUserTypes
    {
        public SQLAspNetUserTypes_Repository(HRDbContext hrDbContext) : base(hrDbContext)
        {
        }
    }
}
