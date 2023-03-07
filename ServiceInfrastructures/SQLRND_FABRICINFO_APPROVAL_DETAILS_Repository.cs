using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLRND_FABRICINFO_APPROVAL_DETAILS_Repository : BaseRepository<RND_FABRICINFO_APPROVAL_DETAILS>, IRND_FABRICINFO_APPROVAL_DETAILS
    {
        public SQLRND_FABRICINFO_APPROVAL_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<bool> FindByFabCodeAsync(int fabCode)
        {
            return await DenimDbContext.RND_FABRICINFO_APPROVAL_DETAILS.AnyAsync(e => e.FABCODE.Equals(fabCode));
        }
    }
}
