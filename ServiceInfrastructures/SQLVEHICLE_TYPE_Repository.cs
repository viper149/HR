using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLVEHICLE_TYPE_Repository : BaseRepository<VEHICLE_TYPE>, IVEHICLE_TYPE
    {
        public SQLVEHICLE_TYPE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
