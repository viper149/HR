using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_GS_RETURNABLE_GP_RCV_D_Repository : BaseRepository<F_GS_RETURNABLE_GP_RCV_D>, IF_GS_RETURNABLE_GP_RCV_D
    {
        public SQLF_GS_RETURNABLE_GP_RCV_D_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
