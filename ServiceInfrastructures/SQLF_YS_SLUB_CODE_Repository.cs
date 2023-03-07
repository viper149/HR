using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_YS_SLUB_CODE_Repository:BaseRepository<F_YS_SLUB_CODE>, IF_YS_SLUB_CODE
    {
        public SQLF_YS_SLUB_CODE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
