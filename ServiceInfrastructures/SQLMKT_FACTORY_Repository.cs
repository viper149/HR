using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLMKT_FACTORY_Repository:BaseRepository<MKT_FACTORY>, IMKT_FACTORY
    {
        public SQLMKT_FACTORY_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
