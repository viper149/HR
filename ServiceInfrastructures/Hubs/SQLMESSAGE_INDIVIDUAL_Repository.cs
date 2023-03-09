using HRMS.Data;
using HRMS.Models;
using HRMS.ServiceInfrastructures.BaseInfrastructures;
using HRMS.ServiceInterfaces.Hubs;

namespace HRMS.ServiceInfrastructures.Hubs
{
    public class SQLMESSAGE_INDIVIDUAL_Repository : BaseRepository<MESSAGE_INDIVIDUAL>, IMESSAGE_INDIVIDUAL
    {
        public SQLMESSAGE_INDIVIDUAL_Repository(HRDbContext hrDbContext) : base(hrDbContext)
        {
        }
    }
}
