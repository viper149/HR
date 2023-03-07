using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.TargetSegment;

namespace DenimERP.ServiceInfrastructures.TargetSegment
{
    public class SQLTARGETPRICESEGMENT_Repository : BaseRepository<TARGETPRICESEGMENT>, ITARGETPRICESEGMENT
    {
        public SQLTARGETPRICESEGMENT_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
