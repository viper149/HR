using HRMS.Data;
using HRMS.Models;
using HRMS.ServiceInfrastructures.BaseInfrastructures;
using HRMS.ServiceInterfaces;

namespace HRMS.ServiceInfrastructures
{
    public class SQLCOUNTRIES_Repository:BaseRepository<COUNTRIES>, ICOUNTRIES
    {
        public SQLCOUNTRIES_Repository(HRDbContext hrDbContext) : base(hrDbContext)
        {
        }
    }
}
