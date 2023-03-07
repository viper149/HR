using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Home;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_YS_YARN_ISSUE_DETAILS_Repository : BaseRepository<F_YS_YARN_ISSUE_DETAILS>, IF_YS_YARN_ISSUE_DETAILS
    {
        public SQLF_YS_YARN_ISSUE_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {

        }

        public async Task<int> InsertAndGetIdAsync(F_YS_YARN_ISSUE_DETAILS fYsYarnIssueDetails)
        {
            try
            {
                await DenimDbContext.F_YS_YARN_ISSUE_DETAILS.AddAsync(fYsYarnIssueDetails);
                await SaveChanges();
                return fYsYarnIssueDetails.TRANSID;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                throw;
            }
        }

        public async Task<FYsYarnIssueViewModel> GetInitObjectsByAsync(FYsYarnIssueViewModel fYsYarnIssueViewModel)
        {
            try
            {
                foreach (var item in fYsYarnIssueViewModel.YarnIssueDetailsList)
                {
                    item.BasYarnCountinfo = await DenimDbContext.BAS_YARN_COUNTINFO
                        .FirstOrDefaultAsync(c => c.COUNTID.Equals(item.COUNTID));

                    item.PO_EXTRA = await DenimDbContext.RND_PRODUCTION_ORDER
                        .Include(c => c.SO)
                        .Select(c => new RND_PRODUCTION_ORDER
                        {
                            POID = c.POID,
                            OPT1 = c.SO != null ? c.SO.SO_NO : ""
                        })
                        .FirstOrDefaultAsync(c => c.POID.Equals(item.SO_NO));

                    item.RefBasYarnCountinfo = await DenimDbContext.BAS_YARN_COUNTINFO
                        .FirstOrDefaultAsync(c => c.COUNTID.Equals(item.MAIN_COUNTID));

                    //item.LOT = await _denimDbContext.BAS_YARN_LOTINFO
                    //    .FirstOrDefaultAsync(c => c.LOTID.Equals(item.LOTID));

                    item.TRANS = await DenimDbContext.F_YARN_REQ_DETAILS
                        .Include(e => e.PO.SO)
                        .Include(e => e.COUNT.COUNT)
                        .FirstOrDefaultAsync(e => e.TRNSID.Equals(item.REQ_DET_ID));

                    item.FBasUnits = await DenimDbContext.F_BAS_UNITS.FirstOrDefaultAsync(e => e.UID.Equals(item.UNIT));

                    item.RCVD = await DenimDbContext.F_YS_YARN_RECEIVE_DETAILS
                        .Include(d => d.YRCV.IND.INDSL)
                        .Include(d => d.BasYarnLotinfo)
                        .FirstOrDefaultAsync(d => d.TRNSID.Equals(item.RCVDID));

                    //item.REQ_DET_ID = fYsYarnIssueViewModel.YarnIssueMaster.YSRID;
                    //item.TRANS = await _denimDbContext.F_YARN_REQ_DETAILS
                    //    .Include(e => e.PO)
                    //    .FirstOrDefaultAsync(e => e.PO.POID.Equals(item.TRANS.ORDERNO));

                }

                return fYsYarnIssueViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> GetReqId(int COUNTID, double REQ_QTY)
        {
            try
            {
                var result = await DenimDbContext.F_YARN_REQ_DETAILS
                    .AsNoTracking()
                    .Where(c => c.COUNTID.Equals(COUNTID) && c.REQ_QTY.Equals(REQ_QTY)).Select(c => c.TRNSID)

                    .FirstOrDefaultAsync();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<DashboardViewModel> GetIssuedCountData()
        {
            //DateTime dt = DateTime.Now.AddYears(-1);
            var dashBoardViewModel = new DashboardViewModel
            {
                FYsYarnIssueViewModel = new FYsYarnIssueViewModel
                {
                    TotalIssuedCount = await DenimDbContext.F_YS_YARN_ISSUE_DETAILS
                        .Where(c=>c.TRNSDATE.Equals(Convert.ToDateTime("2022-01-01")))
                        .Select(c=>c.ISSUE_QTY).SumAsync()
                }
            };
            return dashBoardViewModel;
        }

    }
}
