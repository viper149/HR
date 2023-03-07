using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_PROCESS_MACHINEINFO_Repository:BaseRepository<F_PR_PROCESS_MACHINEINFO>, IF_PR_PROCESS_MACHINEINFO
    {
        public SQLF_PR_PROCESS_MACHINEINFO_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
