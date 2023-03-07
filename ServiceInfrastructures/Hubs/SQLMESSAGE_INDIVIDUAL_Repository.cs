using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.Hubs;

namespace DenimERP.ServiceInfrastructures.Hubs
{
    public class SQLMESSAGE_INDIVIDUAL_Repository : BaseRepository<MESSAGE_INDIVIDUAL>, IMESSAGE_INDIVIDUAL
    {
        public SQLMESSAGE_INDIVIDUAL_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
