using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_HR_SHIFT_INFO_Repository : BaseRepository<F_HR_SHIFT_INFO>, IF_HR_SHIFT_INFO
    {
        public SQLF_HR_SHIFT_INFO_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
