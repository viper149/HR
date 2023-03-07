using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.TargetSegment;

namespace DenimERP.ServiceInfrastructures.TargetSegment
{
    public class SQLTARGETPRICESEGMENTRNDFABRICS_Repository : BaseRepository<TARGETPRICESEGMENTRNDFABRICS>, ITARGETPRICESEGMENTRNDFABRICS
    {
        public SQLTARGETPRICESEGMENTRNDFABRICS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
