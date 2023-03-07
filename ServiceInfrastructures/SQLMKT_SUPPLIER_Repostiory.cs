using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLMKT_SUPPLIER_Repostiory : BaseRepository<MKT_SUPPLIER>, IMKT_SUPPLIER
    {
        public SQLMKT_SUPPLIER_Repostiory(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
