using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_LCB_PRODUCTION_ROPE_PROCESS_INFO_Repository:BaseRepository<F_LCB_PRODUCTION_ROPE_PROCESS_INFO>, IF_LCB_PRODUCTION_ROPE_PROCESS_INFO
    {
        public SQLF_LCB_PRODUCTION_ROPE_PROCESS_INFO_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
