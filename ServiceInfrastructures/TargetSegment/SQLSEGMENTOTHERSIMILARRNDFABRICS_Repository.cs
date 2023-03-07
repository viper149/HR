using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.TargetSegment;

namespace DenimERP.ServiceInfrastructures.TargetSegment
{
    public class SQLSEGMENTOTHERSIMILARRNDFABRICS_Repository : BaseRepository<SEGMENTOTHERSIMILARRNDFABRICS>, ISEGMENTOTHERSIMILARRNDFABRICS
    {
        public SQLSEGMENTOTHERSIMILARRNDFABRICS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
