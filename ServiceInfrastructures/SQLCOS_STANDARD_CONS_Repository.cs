using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOS_STANDARD_CONS_Repository: BaseRepository<COS_STANDARD_CONS>, ICOS_STANDARD_CONS
    {
        public SQLCOS_STANDARD_CONS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
