using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_FN_PROCESS_TYPEINFO_Repository:BaseRepository<F_PR_FN_PROCESS_TYPEINFO>, IF_PR_FN_PROCESS_TYPEINFO
    {
        public SQLF_PR_FN_PROCESS_TYPEINFO_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
