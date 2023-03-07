using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_YS_LOCATION_Repository : BaseRepository<F_YS_LOCATION>, IF_YS_LOCATION
    {
        public SQLF_YS_LOCATION_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {

        }
    }
}
