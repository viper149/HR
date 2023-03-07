using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.ProcWorkOrder;

namespace DenimERP.ServiceInfrastructures.ProcWorkOrder
{
    public class SQLPROC_WORKORDER_DETAILS_Repository : BaseRepository<PROC_WORKORDER_DETAILS>, IPROC_WORKORDER_DETAILS
    {
        public SQLPROC_WORKORDER_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
