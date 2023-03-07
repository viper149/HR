using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOM_IMP_CSRAT_DETAILS_Repository : BaseRepository<COM_IMP_CSRAT_DETAILS>, ICOM_IMP_CSRAT_DETAILS
    {
        public SQLCOM_IMP_CSRAT_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
