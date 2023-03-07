using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_LCB_MACHINE_Repository:BaseRepository<F_LCB_MACHINE>, IF_LCB_MACHINE
    {
        public SQLF_LCB_MACHINE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
