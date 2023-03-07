using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.Hubs;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.Hubs
{
    public class SQLMESSAGE_Repository : BaseRepository<MESSAGE>, IMESSAGE
    {
        public SQLMESSAGE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<MESSAGE>> GetAllIncludeOtherObjects()
        {
            return await DenimDbContext.MESSAGE.Include(e => e.Receiver).ToListAsync();
        }
    }
}
