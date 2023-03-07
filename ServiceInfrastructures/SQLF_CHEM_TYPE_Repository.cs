using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_CHEM_TYPE_Repository : BaseRepository<F_CHEM_TYPE>, IF_CHEM_TYPE
    {
        public SQLF_CHEM_TYPE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
