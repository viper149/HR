using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.TargetSegment;

namespace DenimERP.ServiceInfrastructures.TargetSegment
{
    public class SQLSEGMENTOTHERSIMILARNAME_Repository : BaseRepository<SEGMENTOTHERSIMILARNAME>, ISEGMENTOTHERSIMILARNAME
    {
        public SQLSEGMENTOTHERSIMILARNAME_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
