using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.Factory;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_HR_BLOOD_GROUP_Repository : BaseRepository<F_HR_BLOOD_GROUP>, IF_HR_BLOOD_GROUP
    {
        public SQLF_HR_BLOOD_GROUP_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
