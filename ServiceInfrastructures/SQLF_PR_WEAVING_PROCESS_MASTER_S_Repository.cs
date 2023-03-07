using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_WEAVING_PROCESS_MASTER_S_Repository:BaseRepository<F_PR_WEAVING_PROCESS_MASTER_S>, IF_PR_WEAVING_PROCESS_MASTER_S
    {
        public SQLF_PR_WEAVING_PROCESS_MASTER_S_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
