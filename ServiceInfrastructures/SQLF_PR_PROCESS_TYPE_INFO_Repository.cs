using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_PROCESS_TYPE_INFO_Repository:BaseRepository<F_PR_PROCESS_TYPE_INFO>, IF_PR_PROCESS_TYPE_INFO
    {
        public SQLF_PR_PROCESS_TYPE_INFO_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
