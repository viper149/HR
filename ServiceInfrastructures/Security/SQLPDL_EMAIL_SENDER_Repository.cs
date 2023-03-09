using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.Security
{
    public class SQLPDL_EMAIL_SENDER_Repository : BaseRepository<PDL_EMAIL_SENDER>, IPDL_EMAIL_SENDER
    {
        public SQLPDL_EMAIL_SENDER_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<PDL_EMAIL_SENDER> GetTop1Async()
        {
            //return await DenimDbContext.PDL_EMAIL_SENDER.FirstOrDefaultAsync();
            return null;
        }
    }
}
