using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_INSPECTION_BATCH_Repository:BaseRepository<F_PR_INSPECTION_BATCH>, IF_PR_INSPECTION_BATCH
    {
        public SQLF_PR_INSPECTION_BATCH_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
