using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLLOOM_TYPE_Repository : BaseRepository<LOOM_TYPE>, ILOOM_TYPE
    {
        public SQLLOOM_TYPE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
