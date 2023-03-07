using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_GS_RETURNABLE_GP_RCV_M_Repository : BaseRepository<F_GS_RETURNABLE_GP_RCV_M>, IF_GS_RETURNABLE_GP_RCV_M
    {
        public SQLF_GS_RETURNABLE_GP_RCV_M_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
