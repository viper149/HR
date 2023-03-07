using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOUNTRIES_Repository:BaseRepository<COUNTRIES>, ICOUNTRIES
    {
        public SQLCOUNTRIES_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
