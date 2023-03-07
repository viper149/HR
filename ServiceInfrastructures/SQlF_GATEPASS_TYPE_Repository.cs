using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_GATEPASS_TYPE_Repository : BaseRepository<F_GATEPASS_TYPE>, IF_GATEPASS_TYPE
    {
        public SQLF_GATEPASS_TYPE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
