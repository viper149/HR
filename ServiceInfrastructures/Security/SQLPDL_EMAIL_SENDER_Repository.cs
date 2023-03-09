using System.Threading.Tasks;
using HRMS.Data;
using HRMS.Models;
using HRMS.ServiceInfrastructures.BaseInfrastructures;
using HRMS.ServiceInterfaces;

namespace HRMS.ServiceInfrastructures.Security
{
    public class SQLPDL_EMAIL_SENDER_Repository : BaseRepository<PDL_EMAIL_SENDER>, IPDL_EMAIL_SENDER
    {
        public SQLPDL_EMAIL_SENDER_Repository(HRDbContext hrDbContext) : base(hrDbContext)
        {
        }

        public async Task<PDL_EMAIL_SENDER> GetTop1Async()
        {
            //return await DenimDbContext.PDL_EMAIL_SENDER.FirstOrDefaultAsync();
            return null;
        }
    }
}
