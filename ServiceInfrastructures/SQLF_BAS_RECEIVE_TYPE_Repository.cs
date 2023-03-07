using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_BAS_RECEIVE_TYPE_Repository : BaseRepository<F_BAS_RECEIVE_TYPE>, IF_BAS_RECEIVE_TYPE
    {
        public SQLF_BAS_RECEIVE_TYPE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
