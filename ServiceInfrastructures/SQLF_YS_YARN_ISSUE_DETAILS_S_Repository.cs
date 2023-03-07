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
    public class SQLF_YS_YARN_ISSUE_DETAILS_S_Repository : BaseRepository<F_YS_YARN_ISSUE_DETAILS_S>, IF_YS_YARN_ISSUE_DETAILS_S
    {
        public SQLF_YS_YARN_ISSUE_DETAILS_S_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<int> InsertAndGetIdAsync(F_YS_YARN_ISSUE_DETAILS_S fYsYarnIssueDetailsS)
        {
            await DenimDbContext.F_YS_YARN_ISSUE_DETAILS_S.AddAsync(fYsYarnIssueDetailsS);
            await SaveChanges();
            return fYsYarnIssueDetailsS.TRANSID;
        }

        public async Task<FYsYarnIssueSViewModel> GetInitObjectsByAsync(FYsYarnIssueSViewModel fYsYarnIssueSViewModel)
        {
            foreach (var item in fYsYarnIssueSViewModel.YarnIssueDetailsSList)
            {
                item.COUNT = await DenimDbContext.BAS_YARN_COUNTINFO
                    .FirstOrDefaultAsync(c => c.COUNTID.Equals(item.COUNTID));

                item.RefBasYarnCountinfo = await DenimDbContext.BAS_YARN_COUNTINFO
                    .FirstOrDefaultAsync(c => c.COUNTID.Equals(item.MAIN_COUNTID));

                item.LOT = await DenimDbContext.BAS_YARN_LOTINFO
                    .FirstOrDefaultAsync(c => c.LOTID.Equals(item.LOTID));

                item.TRANS = await DenimDbContext.F_YARN_REQ_DETAILS_S
                    .Include(e => e.ORDERNONavigation.SO)
                    .Include(e => e.COUNT.COUNT)
                    .FirstOrDefaultAsync(e => e.TRNSID.Equals(item.COUNTID) && e.ORDERNO.Equals(item.TRANS.ORDERNO));

                item.UNITNavigation = await DenimDbContext.F_BAS_UNITS.FirstOrDefaultAsync(e => e.UID.Equals(item.UNIT));

                item.REQ_DET_ID = fYsYarnIssueSViewModel.YarnIssueMasterS.YSRID;
                //item.TRANS = await _denimDbContext.F_YARN_REQ_DETAILS_S
                //    .Include(e => e.PO)
                //    .FirstOrDefaultAsync(e => e.ORDERNONavigation.POID.Equals(item.TRANS.ORDERNO));
            }

            return fYsYarnIssueSViewModel;
        }

        public async Task<int> GetReqId(int COUNTID, double REQ_QTY)
        {
            return await DenimDbContext.F_YARN_REQ_DETAILS
                .AsNoTracking()
                .Where(c => c.COUNTID.Equals(COUNTID) && c.REQ_QTY.Equals(REQ_QTY)).Select(c => c.TRNSID)
                .FirstOrDefaultAsync();
        }
    }
}
