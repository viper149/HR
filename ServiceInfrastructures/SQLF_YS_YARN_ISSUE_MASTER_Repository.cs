using System;
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
    public class SQLF_YS_YARN_ISSUE_MASTER_Repository : BaseRepository<F_YS_YARN_ISSUE_MASTER>, IF_YS_YARN_ISSUE_MASTER
    {
        private readonly IDataProtector _protector;

        public SQLF_YS_YARN_ISSUE_MASTER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<dynamic> GetYarnReqMaster(int ysrId)
        {
            try
            {
         
                var fYarnReqDetailses = await DenimDbContext.F_YARN_REQ_DETAILS
                    .Include(c=> c.PO.SO)
                    .Include(c=> c.COUNT.COUNT)
                    .Include(c=> c.LOT)
                    .Include(c=>c.RS)
                    .Where(c => c.YSRID.Equals(ysrId))
                    .Select(e => new
                    {
                        TRNSID = e.TRNSID,
                        SO_NO = e.RSID == null ? e.PO.SO.SO_NO : e.RS.RSOrder,
                        REQ_QTY =  e.REQ_QTY,
                        COUNTID = e.RSID == null ? e.COUNT.COUNT.COUNTID : e.RSCOUNT.COUNTID,
                        //COUNTNAME = e.COUNT!=null ? e.COUNT.COUNT.COUNTNAME : "",
                        RND_COUNTNAME = e.RSID == null ? e.COUNT != null ? e.COUNT.COUNT.RND_COUNTNAME : "" : e.RSCOUNT.RND_COUNTNAME,
                        LOT = e.LOT == null ? "" : e.LOT.LOTNO,
                        LOTNO = e.LOT.LOTNO,


                    }).ToListAsync();

               

                return fYarnReqDetailses;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<F_YS_YARN_ISSUE_MASTER>> GetAllIssueMasterList()
        {
            return await DenimDbContext.F_YS_YARN_ISSUE_MASTER
                .Include(e => e.ISSUE)
                .Include(e => e.YSR)
                .Select(e => new F_YS_YARN_ISSUE_MASTER
                {
                    YISSUEID = e.YISSUEID,
                    EncryptedId = _protector.Protect(e.YISSUEID.ToString()),
                    YISSUEDATE = e.YISSUEDATE,
                    ISSUETO = e.ISSUETO,
                    PURPOSE = e.PURPOSE,
                    ISREMARKABLE = e.ISREMARKABLE,
                    REMARKS = e.REMARKS,
                    ISSUE = new F_BAS_ISSUE_TYPE
                    {
                        ISSUTYPE = e.ISSUE.ISSUTYPE
                    },
                    YSR = new F_YARN_REQ_MASTER
                    {
                        YSRNO = e.YSR.YSRNO
                    }
                }).ToListAsync();
        }

        public async Task<FYsYarnIssueViewModel> FindByIdIncludeAllAsync(int yIssueId)
        {
            try
            {
                var fYsYarnIssueViewModel = await DenimDbContext.F_YS_YARN_ISSUE_MASTER
                    .Include(e => e.ISSUE)
                    .Include(e => e.YSR)
                    .Include(e => e.F_YS_YARN_ISSUE_DETAILS)
                    .Include(e => e.YSR)
                    .Include(e => e.F_YS_YARN_ISSUE_DETAILS)
                    .ThenInclude(e => e.TRANS)
                    .Include(e => e.F_YS_YARN_ISSUE_DETAILS)
                    .ThenInclude(e => e.TRANS.PO.SO)
                    .Include(e => e.F_YS_YARN_ISSUE_DETAILS)
                    .ThenInclude(e => e.TRANS.FBasUnits)
                    .Include(e => e.F_YS_YARN_ISSUE_DETAILS)
                    .ThenInclude(e => e.LOT)
                    .Include(e => e.F_YS_YARN_ISSUE_DETAILS)
                    .ThenInclude(e => e.BasYarnCountinfo)
                    .Include(e => e.F_YS_YARN_ISSUE_DETAILS)
                    .ThenInclude(e => e.RefBasYarnCountinfo)
                    .Include(e => e.F_YS_YARN_ISSUE_DETAILS)
                    .ThenInclude(e => e.RCVD.BasYarnLotinfo)
                    .Where(c=>c.YISSUEID.Equals(yIssueId))
                    .Select(e => new FYsYarnIssueViewModel
                    {
                        YarnIssueMaster = new F_YS_YARN_ISSUE_MASTER
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
                        YarnIssueDetailsList = e.F_YS_YARN_ISSUE_DETAILS.Select(f => new F_YS_YARN_ISSUE_DETAILS
                        {
                            TRNSDATE = f.TRNSDATE,
                            UNIT = f.UNIT,
                            ISSUE_QTY = f.ISSUE_QTY,
                            COUNTID = f.COUNTID,
                            MAIN_COUNTID = f.MAIN_COUNTID,
                            REQ_DET_ID = f.REQ_DET_ID,
                            LOTID = f.LOTID,
                            LOT = f.LOT,
                            TRANSID = f.TRANSID,
                            FBasUnits = f.FBasUnits,
                            BasYarnCountinfo = f.BasYarnCountinfo,
                            RefBasYarnCountinfo = f.RefBasYarnCountinfo,
                            TRANS = new F_YARN_REQ_DETAILS
                            {
                                REQ_QTY = f.TRANS.REQ_QTY,
                                PO = new RND_PRODUCTION_ORDER
                                {
                                    SO = new COM_EX_PI_DETAILS
                                    {
                                        SO_NO = f.TRANS.PO.SO.SO_NO
                                    }
                                }
                            },
                            YISSUE = new F_YS_YARN_ISSUE_MASTER
                            {
                                YSR = f.YISSUE.YSR
                            },
                            REMARKS = f.REMARKS,
                            BAG = f.BAG,
                            
                            RCVD = new F_YS_YARN_RECEIVE_DETAILS
                            {
                                BasYarnLotinfo = new BAS_YARN_LOTINFO
                                {
                                    LOTNO = f.RCVD.BasYarnLotinfo.LOTNO
                                }
                            }
                        }).ToList()
                    }).FirstOrDefaultAsync();

                return fYsYarnIssueViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FYsYarnIssueViewModel> GetInitObjectsAsync(FYsYarnIssueViewModel yarnIssueViewModel)
        {
            yarnIssueViewModel.BasUnits = await DenimDbContext.F_BAS_UNITS
                .Select(e => new F_BAS_UNITS
                {
                    UID = e.UID,
                    UNAME = e.UNAME
                }).OrderByDescending(e => e.UNAME.ToLower().Contains("kg")).ThenBy(e => e.UNAME).ToListAsync();

            yarnIssueViewModel.RndProductionOrders = await DenimDbContext.RND_PRODUCTION_ORDER  
                .Include(c=>c.SO)
                .Select(e => new RND_PRODUCTION_ORDER()
                {
                    POID = e.POID,
                    OPT1 = e.SO!=null?e.SO.SO_NO:""
                }).OrderByDescending(e => e.OPT1).ToListAsync();

            yarnIssueViewModel.IssueTypeList = await DenimDbContext.F_BAS_ISSUE_TYPE.Select(e => new F_BAS_ISSUE_TYPE
            {
                ISSUID = e.ISSUID,
                ISSUTYPE = e.ISSUTYPE
            }).OrderBy(e => e.ISSUTYPE).ToListAsync();

            yarnIssueViewModel.FYsLocationList = await DenimDbContext.F_YS_LOCATION
                .Select(e => new F_YS_LOCATION
                {
                    LOCID = e.LOCID,
                    LOCNAME = e.LOCNAME
                }).OrderBy(e => e.LOCNAME).ToListAsync();

            yarnIssueViewModel.CountNameList = await DenimDbContext.BAS_YARN_COUNTINFO
                .Select(e => new BAS_YARN_COUNTINFO
                {
                    COUNTID = e.COUNTID,
                    RND_COUNTNAME = e.RND_COUNTNAME
                }).OrderBy(e => e.COUNTNAME).ToListAsync();
            yarnIssueViewModel.FYarnReqMasterList = await DenimDbContext.F_YARN_REQ_MASTER
                .Select(c => new F_YARN_REQ_MASTER
                {
                 YSRID   = c.YSRID,
                 YSRNO = c.YSRNO
                }).ToListAsync();
            yarnIssueViewModel.FYsYarnReceiveDetailsList = await DenimDbContext.F_YS_YARN_RECEIVE_DETAILS
                .Include(d=>d.YRCV)
                .Select(d => new F_YS_YARN_RECEIVE_DETAILS
                {
                    TRNSID = d.TRNSID,
                    YRCV = new F_YS_YARN_RECEIVE_MASTER
                    {
                        CHALLANNO = $"{d.YRCV.CHALLANNO} - {d.RCV_QTY}"
                    }
                }).ToListAsync();

            return yarnIssueViewModel;
        }
    }
}
