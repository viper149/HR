using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLBAS_INSURANCEINFO_Repository : BaseRepository<BAS_INSURANCEINFO>, IBAS_INSURANCEINFO
    {
        public SQLBAS_INSURANCEINFO_Repository(DenimDbContext denimDbContext)
            : base(denimDbContext)
        {
        }
    }
}
