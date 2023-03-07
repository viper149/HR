using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_INSPECTION_DEFECT_POINT_Repository:BaseRepository<F_PR_INSPECTION_DEFECT_POINT>, IF_PR_INSPECTION_DEFECT_POINT
    {
        public SQLF_PR_INSPECTION_DEFECT_POINT_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
