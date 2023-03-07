using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.TargetSegment;

namespace DenimERP.ServiceInfrastructures.TargetSegment
{
    public class SQLTARGETCHARACTERRNDFABRICS_Repository : BaseRepository<TARGETCHARACTERRNDFABRICS>, ITARGETCHARACTERRNDFABRICS
    {
        public SQLTARGETCHARACTERRNDFABRICS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
