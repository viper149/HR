using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOS_FIXEDCOST_Repository : BaseRepository<COS_FIXEDCOST>, ICOS_FIXEDCOST
    {
        public SQLCOS_FIXEDCOST_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
