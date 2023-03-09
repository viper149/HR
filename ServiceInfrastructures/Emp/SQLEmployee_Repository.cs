using HRMS.Data;
using HRMS.Models;
using HRMS.ServiceInfrastructures.BaseInfrastructures;
using HRMS.ServiceInterfaces.Emp;

namespace HRMS.ServiceInfrastructures.Emp
{
    public class SQLEmployee_Repository : BaseRepository<Employee>, IEmployee
    {
        public SQLEmployee_Repository(HRDbContext hrDbContext) : base(hrDbContext)
        {
        }
    }
}
