using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_CHEM_QC_APPROVE_Repository : BaseRepository<F_CHEM_QC_APPROVE>, IF_CHEM_QC_APPROVE
    {
        public SQLF_CHEM_QC_APPROVE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {

        }
    }
}
