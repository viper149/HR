using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_SLASHER_MACHINE_INFO_Repository: BaseRepository<F_PR_SLASHER_MACHINE_INFO>, IF_PR_SLASHER_MACHINE_INFO
    {
        public SQLF_PR_SLASHER_MACHINE_INFO_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
