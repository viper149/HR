using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_INSPECTION_MACHINE_Repository:BaseRepository<F_PR_INSPECTION_MACHINE>, IF_PR_INSPECTION_MACHINE
    {
        public SQLF_PR_INSPECTION_MACHINE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
