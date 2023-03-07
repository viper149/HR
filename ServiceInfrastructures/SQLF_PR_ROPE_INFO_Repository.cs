using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_ROPE_INFO_Repository: BaseRepository<F_PR_ROPE_INFO>, IF_PR_ROPE_INFO
    {
        public SQLF_PR_ROPE_INFO_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
