using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.Emp;

namespace DenimERP.ServiceInfrastructures.Emp
{
    public class SQLEmployee_Repository : BaseRepository<Employee>, IEmployee
    {
        public SQLEmployee_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
