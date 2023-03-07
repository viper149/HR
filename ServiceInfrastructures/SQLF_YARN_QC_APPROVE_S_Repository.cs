using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_YARN_QC_APPROVE_S_Repository : BaseRepository<F_YARN_QC_APPROVE_S>, IF_YARN_QC_APPROVE_S
    {
        public SQLF_YARN_QC_APPROVE_S_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<FYarnQCAppproveSViewModel> FindByTrnsIdAsync(int trnsId)
        {
            return await DenimDbContext.F_YS_YARN_RECEIVE_DETAILS_S
                .Where(e => e.TRNSID.Equals(trnsId))
                .Include(e => e.F_YARN_QC_APPROVE_S)
                .Include(e => e.F_YS_YARN_RECEIVE_REPORT_S)
                .Select(e => new FYarnQCAppproveSViewModel
                {
                    FYarnQcApproves = e.F_YARN_QC_APPROVE_S,
                    FYsYarnReceiveReports = e.F_YS_YARN_RECEIVE_REPORT_S
                }).FirstOrDefaultAsync();
        }
    }
}
