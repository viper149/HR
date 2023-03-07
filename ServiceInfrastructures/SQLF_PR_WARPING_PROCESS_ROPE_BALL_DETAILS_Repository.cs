using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_WARPING_PROCESS_ROPE_BALL_DETAILS_Repository:BaseRepository<F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS>, IF_PR_WARPING_PROCESS_ROPE_BALL_DETAILS
    {
        public SQLF_PR_WARPING_PROCESS_ROPE_BALL_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
