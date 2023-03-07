using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLPL_DYEING_MACHINE_TYPE_Repository:BaseRepository<PL_DYEING_MACHINE_TYPE>, IPL_DYEING_MACHINE_TYPE
    {
        public SQLPL_DYEING_MACHINE_TYPE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
