using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.YarnStore.Receive;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_YARN_QC_APPROVE_Repository : BaseRepository<F_YARN_QC_APPROVE>, IF_YARN_QC_APPROVE
    {
        public SQLF_YARN_QC_APPROVE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {

        }

        public async Task<GetQcAndReceiveReportViewModel> FindByTrnsIdAsync(int trnsId)
        {
            return await DenimDbContext.F_YS_YARN_RECEIVE_DETAILS
                .Where(e => e.TRNSID.Equals(trnsId))
                .Include(e => e.F_YARN_QC_APPROVE)
                .Include(e => e.F_YS_YARN_RECEIVE_REPORT)
                .Select(e => new GetQcAndReceiveReportViewModel
                {
                    FYarnQcApproves = e.F_YARN_QC_APPROVE,
                    FYsYarnReceiveReports = e.F_YS_YARN_RECEIVE_REPORT
                }).FirstOrDefaultAsync();
        }
    }
}
