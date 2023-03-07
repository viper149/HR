using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_FS_FABRIC_TYPE_Repository:BaseRepository<F_FS_FABRIC_TYPE>, IF_FS_FABRIC_TYPE
    {
        public SQLF_FS_FABRIC_TYPE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
