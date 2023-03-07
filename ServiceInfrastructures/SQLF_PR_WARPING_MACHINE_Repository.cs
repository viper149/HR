using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_WARPING_MACHINE_Repository:BaseRepository<F_PR_WARPING_MACHINE>, IF_PR_WARPING_MACHINE
    {
        public SQLF_PR_WARPING_MACHINE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
