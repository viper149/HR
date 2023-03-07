using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_YS_LEDGER_Repository : BaseRepository<F_YS_LEDGER>, IF_YS_LEDGER
    {
        public SQLF_YS_LEDGER_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {

        }
    }
}
