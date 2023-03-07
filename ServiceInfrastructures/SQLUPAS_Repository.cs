using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLUPAS_Repository : BaseRepository<UPAS>, IUPAS
    {
        public SQLUPAS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
