using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_WEAVING_BEAM_Repository:BaseRepository<F_WEAVING_BEAM>, IF_WEAVING_BEAM
    {
        public SQLF_WEAVING_BEAM_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
