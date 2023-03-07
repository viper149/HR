using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_YS_YARN_ISSUE_MASTER_S_Repository : BaseRepository<F_YS_YARN_ISSUE_MASTER_S>, IF_YS_YARN_ISSUE_MASTER_S
    {
        private readonly IDataProtector _protector;

        public SQLF_YS_YARN_ISSUE_MASTER_S_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<dynamic> GetYarnReqMaster(int ysrId)
        {
            return await DenimDbContext.F_YARN_REQ_DETAILS_S
                .Where(c => c.YSRID.Equals(ysrId))
                .Select(e => new
                {
                    TRNSID = e.TRNSID,
                    POID = e.ORDERNONavigation.POID,
                    SO_NO = $"{e.ORDERNONavigation.SO.SO_NO}",
                    REQ_QTY = e.REQ_QTY,
                    COUNTID = e.COUNT.COUNT.COUNTID,
                    COUNTNAME = e.COUNT.COUNT.COUNTNAME,
                    RND_COUNTNAME = e.COUNT.COUNT.RND_COUNTNAME
                }).ToListAsync();
        }

        public async Task<IEnumerable<F_YS_YARN_ISSUE_MASTER_S>> GetAllIssueMasterList()
        {
            return await DenimDbContext.F_YS_YARN_ISSUE_MASTER_S
                .Include(e => e.ISSUE)
                .Include(e => e.YSR)
                .Select(e => new F_YS_YARN_ISSUE_MASTER_S
                {
                    YISSUEID = e.YISSUEID,
                    EncryptedId = _protector.Protect(e.YISSUEID.ToString()),
                    YISSUEDATE = e.YISSUEDATE,
                    ISSUEID = e.ISSUEID,
                    YSRID = e.YSRID,
                    ISSUETO = e.ISSUETO,
                    PURPOSE = e.PURPOSE,
                    ISREMARKABLE = e.ISREMARKABLE,
                    REMARKS = e.REMARKS,
                    ISSUE = e.ISSUE,
                    YSR = e.YSR
                }).ToListAsync();
        }

        public async Task<FYsYarnIssueSViewModel> FindByIdIncludeAllAsync(int yIssueId)
        {
            return await DenimDbContext.F_YS_YARN_ISSUE_MASTER_S
                   .Include(e => e.ISSUE)
                   .Include(e => e.YSR)
                   .Include(e => e.F_YS_YARN_ISSUE_DETAILS_S)
                   .Include(e => e.YSR)
                   .Include(e => e.F_YS_YARN_ISSUE_DETAILS_S)
                   .ThenInclude(e => e.TRANS)
                   .Include(e => e.F_YS_YARN_ISSUE_DETAILS_S)
                   .ThenInclude(e => e.TRANS.ORDERNONavigation.SO)
                   .Include(e => e.F_YS_YARN_ISSUE_DETAILS_S)
                   .ThenInclude(e => e.TRANS.UNITNavigation)
                   .Include(e => e.F_YS_YARN_ISSUE_DETAILS_S)
                   .ThenInclude(e => e.LOT)
                   .Include(e => e.F_YS_YARN_ISSUE_DETAILS_S)
                   .ThenInclude(e => e.COUNT)
                   .Include(e => e.F_YS_YARN_ISSUE_DETAILS_S)
                   .ThenInclude(e => e.RefBasYarnCountinfo)
                   .Where(c => c.YISSUEID.Equals(yIssueId))
                   .Select(e => new FYsYarnIssueSViewModel
                   {
                       YarnIssueMasterS = new F_YS_YARN_ISSUE_MASTER_S
                       {
                           YISSUEID = e.YISSUEID,
                           EncryptedId = _protector.Protect(e.YISSUEID.ToString()),
                           YISSUEDATE = e.YISSUEDATE,
                           ISSUEID = e.ISSUEID,
                           YSRID = e.YSRID,
                           ISSUETO = e.ISSUETO,
                           PURPOSE = e.PURPOSE,
                           ISREMARKABLE = e.ISREMARKABLE,
                           REMARKS = e.REMARKS,
                           ISSUE = e.ISSUE,
                           YSR = e.YSR
                       },
                       YarnIssueDetailsSList = e.F_YS_YARN_ISSUE_DETAILS_S.Select(f => new F_YS_YARN_ISSUE_DETAILS_S
                       {
                           TRNSDATE = f.TRNSDATE,
                           UNIT = f.UNIT,
                           ISSUE_QTY = f.ISSUE_QTY,
                           COUNTID = f.COUNTID,
                           MAIN_COUNTID = f.MAIN_COUNTID,
                           REQ_DET_ID = f.REQ_DET_ID,
                           LOTID = f.LOTID,
                           TRANSID = f.TRANSID,
                           UNITNavigation = f.UNITNavigation,
                           COUNT = f.COUNT,
                           RefBasYarnCountinfo = f.RefBasYarnCountinfo,
                           TRANS = new F_YARN_REQ_DETAILS_S
                           {
                               REQ_QTY = f.TRANS.REQ_QTY
                           },
                           YISSUE = new F_YS_YARN_ISSUE_MASTER_S
                           {
                               YSR = f.YISSUE.YSR
                           },
                           LOT = f.LOT,
                           REMARKS = f.REMARKS
                       }).ToList()
                   }).FirstOrDefaultAsync();
        }

        public async Task<FYsYarnIssueSViewModel> GetInitObjectsAsync(FYsYarnIssueSViewModel fYsYarnIssueSViewModel)
        {
            fYsYarnIssueSViewModel.BasUnits = await DenimDbContext.F_BAS_UNITS
                .Select(e => new F_BAS_UNITS
                {
                    UID = e.UID,
                    UNAME = e.UNAME
                }).OrderByDescending(e => e.UNAME.ToLower().Contains("kg")).ThenBy(e => e.UNAME).ToListAsync();

            fYsYarnIssueSViewModel.IssueTypeList = await DenimDbContext.F_BAS_ISSUE_TYPE.Select(e => new F_BAS_ISSUE_TYPE
            {
                ISSUID = e.ISSUID,
                ISSUTYPE = e.ISSUTYPE
            }).OrderBy(e => e.ISSUTYPE).ToListAsync();

            fYsYarnIssueSViewModel.FYsLocationList = await DenimDbContext.F_YS_LOCATION
                .Select(e => new F_YS_LOCATION
                {
                    LOCID = e.LOCID,
                    LOCNAME = e.LOCNAME
                }).OrderBy(e => e.LOCNAME).ToListAsync();

            fYsYarnIssueSViewModel.CountNameList = await DenimDbContext.BAS_YARN_COUNTINFO
                .Select(e => new BAS_YARN_COUNTINFO
                {
                    COUNTID = e.COUNTID,
                    COUNTNAME = e.COUNTNAME
                }).OrderBy(e => e.COUNTNAME).ToListAsync();
            fYsYarnIssueSViewModel.FYarnReqMasterSList = await DenimDbContext.F_YARN_REQ_MASTER_S
                .Select(c => new F_YARN_REQ_MASTER_S
                {
                    YSRID = c.YSRID,
                    YSRNO = c.YSRNO
                }).ToListAsync();
            fYsYarnIssueSViewModel.FYsYarnReceiveDetailsList = await DenimDbContext.F_YS_YARN_RECEIVE_DETAILS_S
                .Include(d => d.YRCV)
                .Select(d => new F_YS_YARN_RECEIVE_DETAILS_S
                {
                    TRNSID = d.TRNSID,
                    YRCV = new F_YS_YARN_RECEIVE_MASTER_S
                    {
                        CHALLANNO = $"{d.YRCV.CHALLANNO} - {d.RCV_QTY}"
                    }
                }).ToListAsync();

            return fYsYarnIssueSViewModel;
        }
    }
}
