using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_LCB_BEAM_Repository:BaseRepository<F_LCB_BEAM>, IF_LCB_BEAM
    {
        public SQLF_LCB_BEAM_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
