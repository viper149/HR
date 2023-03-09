using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Data;
using HRMS.Models;
using HRMS.ServiceInfrastructures.BaseInfrastructures;
using HRMS.ServiceInterfaces.Hubs;
using Microsoft.EntityFrameworkCore;

namespace HRMS.ServiceInfrastructures.Hubs
{
    public class SQLMESSAGE_Repository : BaseRepository<MESSAGE>, IMESSAGE
    {
        public SQLMESSAGE_Repository(HRDbContext hrDbContext) : base(hrDbContext)
        {
        }

        public async Task<IEnumerable<MESSAGE>> GetAllIncludeOtherObjects()
        {
            return await HrDbContext.MESSAGE.Include(e => e.Receiver).ToListAsync();
        }
    }
}
