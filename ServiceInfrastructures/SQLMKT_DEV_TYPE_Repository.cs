using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLMKT_DEV_TYPE_Repository: BaseRepository<MKT_DEV_TYPE>, IMKT_DEV_TYPE
    {
        public SQLMKT_DEV_TYPE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
