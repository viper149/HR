using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_FS_LOCATION_Repository : BaseRepository<F_FS_LOCATION>, IF_FS_LOCATION
    {
        public SQLF_FS_LOCATION_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
