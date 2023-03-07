using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_SIZING_MACHINE_Repository:BaseRepository<F_SIZING_MACHINE>, IF_SIZING_MACHINE
    {
        public SQLF_SIZING_MACHINE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
