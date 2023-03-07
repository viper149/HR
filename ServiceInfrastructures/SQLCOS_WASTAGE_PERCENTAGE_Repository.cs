using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOS_WASTAGE_PERCENTAGE_Repository:BaseRepository<COS_WASTAGE_PERCENTAGE>, ICOS_WASTAGE_PERCENTAGE
    {
        public SQLCOS_WASTAGE_PERCENTAGE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
            
        }
    }
}
