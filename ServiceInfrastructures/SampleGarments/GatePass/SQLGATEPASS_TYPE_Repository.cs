using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.SampleGarments.GatePass;

namespace DenimERP.ServiceInfrastructures.SampleGarments.GatePass
{
    public class SQLGATEPASS_TYPE_Repository : BaseRepository<GATEPASS_TYPE>, IGATEPASS_TYPE
    {
        public SQLGATEPASS_TYPE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
