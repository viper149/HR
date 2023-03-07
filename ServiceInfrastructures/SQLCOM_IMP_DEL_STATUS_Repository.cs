using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOM_IMP_DEL_STATUS_Repository:BaseRepository<COM_IMP_DEL_STATUS>, ICOM_IMP_DEL_STATUS
    {
        public SQLCOM_IMP_DEL_STATUS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
