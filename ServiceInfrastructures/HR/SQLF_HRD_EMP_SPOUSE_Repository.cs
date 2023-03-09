using HRMS.Data;
using HRMS.Models;
using HRMS.ServiceInfrastructures.BaseInfrastructures;
using HRMS.ServiceInterfaces.HR;

namespace HRMS.ServiceInfrastructures.HR
{
    public class SQLF_HRD_EMP_SPOUSE_Repository : BaseRepository<F_HRD_EMP_SPOUSE>, IF_HRD_EMP_SPOUSE
    {
        public SQLF_HRD_EMP_SPOUSE_Repository(HRDbContext hrDbContext) : base(hrDbContext)
        {
        }
    }
}
