using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_YS_GP_DETAILS_Repository : BaseRepository<F_YS_GP_DETAILS>, IF_YS_GP_DETAILS
    {
        public SQLF_YS_GP_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}





