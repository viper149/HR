using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_INSPECTION_DEFECTINFO_Repository:BaseRepository<F_PR_INSPECTION_DEFECTINFO>, IF_PR_INSPECTION_DEFECTINFO
    {
        public SQLF_PR_INSPECTION_DEFECTINFO_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
