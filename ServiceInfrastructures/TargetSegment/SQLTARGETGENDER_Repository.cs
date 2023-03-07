using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.TargetSegment;

namespace DenimERP.ServiceInfrastructures.TargetSegment
{
    public class SQLTARGETGENDER_Repository : BaseRepository<TARGETGENDER>, ITARGETGENDER
    {
        public SQLTARGETGENDER_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
