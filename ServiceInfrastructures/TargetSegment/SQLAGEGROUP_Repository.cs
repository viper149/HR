using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.TargetSegment;

namespace DenimERP.ServiceInfrastructures.TargetSegment
{
    public class SQLAGEGROUP_Repository : BaseRepository<AGEGROUP>, IAGEGROUP
    {
        public SQLAGEGROUP_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
