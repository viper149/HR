using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.TargetSegment;

namespace DenimERP.ServiceInfrastructures.TargetSegment
{
    public class SQLTARGETCHARACTER_Repository : BaseRepository<TARGETCHARACTER>, ITARGETCHARACTER
    {
        public SQLTARGETCHARACTER_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
