using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOM_EX_ADV_DELIVERY_SCH_DETAILS_Repository : BaseRepository<COM_EX_ADV_DELIVERY_SCH_DETAILS>, ICOM_EX_ADV_DELIVERY_SCH_DETAILS
    {
        public SQLCOM_EX_ADV_DELIVERY_SCH_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
