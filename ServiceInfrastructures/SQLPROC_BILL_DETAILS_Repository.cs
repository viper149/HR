using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLPROC_BILL_DETAILS_Repository : BaseRepository<PROC_BILL_DETAILS>, IPROC_BILL_DETAILS
    {
        public SQLPROC_BILL_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
