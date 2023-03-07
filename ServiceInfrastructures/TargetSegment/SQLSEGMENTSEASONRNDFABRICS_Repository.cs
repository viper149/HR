using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.TargetSegment;

namespace DenimERP.ServiceInfrastructures.TargetSegment
{
    public class SQLSEGMENTSEASONRNDFABRICS_Repository : BaseRepository<SEGMENTSEASONRNDFABRICS>, ISEGMENTSEASONRNDFABRICS
    {
        public SQLSEGMENTSEASONRNDFABRICS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
