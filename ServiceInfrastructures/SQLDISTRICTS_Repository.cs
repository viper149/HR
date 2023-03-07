using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLDISTRICTS_Repository: BaseRepository<DISTRICTS>,IDISTRICTS
    {
        public SQLDISTRICTS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
