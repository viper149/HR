using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.TargetSegment;

namespace DenimERP.ServiceInfrastructures.TargetSegment
{
    public class SQLSEGMENTCOMSEGMENTRNDFABRICS_Repository : BaseRepository<SEGMENTCOMSEGMENTRNDFABRICS>, ISEGMENTCOMSEGMENTRNDFABRICS
    {
        public SQLSEGMENTCOMSEGMENTRNDFABRICS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
