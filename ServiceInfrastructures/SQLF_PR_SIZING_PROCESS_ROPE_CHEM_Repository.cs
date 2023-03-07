using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_SIZING_PROCESS_ROPE_CHEM_Repository:BaseRepository<F_PR_SIZING_PROCESS_ROPE_CHEM>, IF_PR_SIZING_PROCESS_ROPE_CHEM
    {
        public SQLF_PR_SIZING_PROCESS_ROPE_CHEM_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
