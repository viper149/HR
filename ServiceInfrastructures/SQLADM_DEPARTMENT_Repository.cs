using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLADM_DEPARTMENT_Repository : BaseRepository<ADM_DEPARTMENT>, IADM_DEPARTMENT
    {
        public SQLADM_DEPARTMENT_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
