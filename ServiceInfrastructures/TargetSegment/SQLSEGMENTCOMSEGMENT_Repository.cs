using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.TargetSegment;

namespace DenimERP.ServiceInfrastructures.TargetSegment
{
    public class SQLSEGMENTCOMSEGMENT_Repository : BaseRepository<SEGMENTCOMSEGMENT>, ISEGMENTCOMSEGMENT
    {
        public SQLSEGMENTCOMSEGMENT_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
