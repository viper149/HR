using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_GS_GATEPASS_RETURN_RCV_DETAILS_Repository : BaseRepository<F_GS_GATEPASS_RETURN_RCV_DETAILS>, IF_GS_GATEPASS_RETURN_RCV_DETAILS
    {
        public SQLF_GS_GATEPASS_RETURN_RCV_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
