using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOM_IMP_LCTYPE_Repository : BaseRepository<COM_IMP_LCTYPE>, ICOM_IMP_LCTYPE
    {
        public SQLCOM_IMP_LCTYPE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
