using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOS_CERTIFICATION_COST_Repository:BaseRepository<COS_CERTIFICATION_COST>, ICOS_CERTIFICATION_COST
    {
        public SQLCOS_CERTIFICATION_COST_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
