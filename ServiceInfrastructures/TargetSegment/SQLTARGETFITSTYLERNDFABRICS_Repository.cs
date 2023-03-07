using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.TargetSegment;

namespace DenimERP.ServiceInfrastructures.TargetSegment
{
    public class SQLTARGETFITSTYLERNDFABRICS_Repository : BaseRepository<TARGETFITSTYLERNDFABRICS>, ITARGETFITSTYLERNDFABRICS
    {
        public SQLTARGETFITSTYLERNDFABRICS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
