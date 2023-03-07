using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.HR;

namespace DenimERP.ServiceInfrastructures.HR
{
    public class SQLF_HRD_EMP_SPOUSE_Repository : BaseRepository<F_HRD_EMP_SPOUSE>, IF_HRD_EMP_SPOUSE
    {
        public SQLF_HRD_EMP_SPOUSE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
