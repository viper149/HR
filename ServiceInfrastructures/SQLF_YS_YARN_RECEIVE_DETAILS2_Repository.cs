using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_YS_YARN_RECEIVE_DETAILS2_Repository : BaseRepository<F_YS_YARN_RECEIVE_DETAILS2>, IF_YS_YARN_RECEIVE_DETAILS2
    {
        public SQLF_YS_YARN_RECEIVE_DETAILS2_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }

 }

