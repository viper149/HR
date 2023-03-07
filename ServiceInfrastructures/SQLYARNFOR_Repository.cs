using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLYARNFOR_Repository : BaseRepository<YARNFOR>, IYARNFOR
    {
        public SQLYARNFOR_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }


    }
}
