using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.TargetSegment;

namespace DenimERP.ServiceInfrastructures.TargetSegment
{
    public class SQLAGEGROUPRNDFABRICS_Repository : BaseRepository<AGEGROUPRNDFABRICS>, IAGEGROUPRNDFABRICS
    {
        public SQLAGEGROUPRNDFABRICS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
