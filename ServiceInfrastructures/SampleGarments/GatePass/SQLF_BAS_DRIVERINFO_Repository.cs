using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.SampleGarments.GatePass;

namespace DenimERP.ServiceInfrastructures.SampleGarments.GatePass
{
    public class SQLF_BAS_DRIVERINFO_Repository : BaseRepository<F_BAS_DRIVERINFO>, IF_BAS_DRIVERINFO
    {
        public SQLF_BAS_DRIVERINFO_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
