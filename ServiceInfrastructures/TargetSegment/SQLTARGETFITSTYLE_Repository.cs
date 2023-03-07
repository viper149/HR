using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.TargetSegment;

namespace DenimERP.ServiceInfrastructures.TargetSegment
{
    public class SQLTARGETFITSTYLE_Repository : BaseRepository<TARGETFITSTYLE>, ITARGETFITSTYLE
    {
        public SQLTARGETFITSTYLE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
