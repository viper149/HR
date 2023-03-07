using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_BAS_BALL_INFO_Repository : BaseRepository<F_BAS_BALL_INFO>, IF_BAS_BALL_INFO
    {
        public SQLF_BAS_BALL_INFO_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
