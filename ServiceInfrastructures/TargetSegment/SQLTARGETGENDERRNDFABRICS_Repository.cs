using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.TargetSegment;

namespace DenimERP.ServiceInfrastructures.TargetSegment
{
    public class SQLTARGETGENDERRNDFABRICS_Repository : BaseRepository<TARGETGENDERRNDFABRICS>, ITARGETGENDERRNDFABRICS
    {
        public SQLTARGETGENDERRNDFABRICS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
