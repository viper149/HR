using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLYARNFROM_Repository : BaseRepository<YARNFROM>, IYARNFROM
    {
        public SQLYARNFROM_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
