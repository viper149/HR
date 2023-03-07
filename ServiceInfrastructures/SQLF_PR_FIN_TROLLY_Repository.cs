using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_FIN_TROLLY_Repository:BaseRepository<F_PR_FIN_TROLLY>, IF_PR_FIN_TROLLY
    {
        public SQLF_PR_FIN_TROLLY_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
