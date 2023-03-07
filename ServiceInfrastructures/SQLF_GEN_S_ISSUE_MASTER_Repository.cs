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
    public class SQLF_GEN_S_ISSUE_MASTER_Repository : BaseRepository<F_GEN_S_ISSUE_MASTER>, IF_GEN_S_ISSUE_MASTER
    {
        private readonly IDataProtector _protector;

        public SQLF_GEN_S_ISSUE_MASTER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<FGenSIssueViewModel> GetInitObjByAsync(FGenSIssueViewModel fGenSIssueViewModel)
        {
            var ignores = new[]
            {
                "Recone(LCB)"
            };

            fGenSIssueViewModel.IssueFHrEmployees = await DenimDbContext.F_HRD_EMPLOYEE
                .Select(d => new F_HRD_EMPLOYEE
                {
                    EMPID = d.EMPID,
                    FIRST_NAME = $"{(d.EMPNO != null ? d.EMPNO + " -" : "")} {d.FIRST_NAME} {d.LAST_NAME}"
                }).OrderBy(d => d.FIRST_NAME).ToListAsync();

            fGenSIssueViewModel.ReceiveFHrEmployees = await DenimDbContext.F_HRD_EMPLOYEE
                .Select(d => new F_HRD_EMPLOYEE
                {
                    EMPID = d.EMPID,
                    FIRST_NAME = $"{(d.EMPNO != null ? d.EMPNO + " -" : "")} {d.FIRST_NAME} {d.LAST_NAME}"
                }).OrderBy(d => d.FIRST_NAME).ToListAsync();

            fGenSIssueViewModel.FBasIssueTypesList = await DenimDbContext.F_BAS_ISSUE_TYPE
                .Where(e => !ignores.Any(f => f.ToLower().Contains(e.ISSUTYPE.ToLower())))
                .Select(e => new F_BAS_ISSUE_TYPE
                {
                    ISSUID = e.ISSUID,
                    ISSUTYPE = e.ISSUTYPE
                }).OrderBy(e => e.ISSUTYPE).ToListAsync();

            //fGenSIssueViewModel.FGsProductInformationsList = await _denimDbContext.F_GS_PRODUCT_INFORMATION
            //    .Select(d => new F_GS_PRODUCT_INFORMATION
            //    {
            //        PRODID = d.PRODID,
            //        PRODNAME = $"{d.PRODID} - {d.PRODNAME} {(d.PARTNO != "" ? " - " + d.PARTNO : "")}"
            //    })
            //    .OrderBy(d => d.PRODNAME).ToListAsync();

            fGenSIssueViewModel.FGenSReqMastersList = await DenimDbContext.F_GEN_S_REQ_MASTER
                .Select(d => new F_GEN_S_REQ_MASTER
                {
                    GSRID = d.GSRID,
                    GSRNO = d.GSRNO
                }).OrderByDescending(d => d.GSRID).ToListAsync();

            return fGenSIssueViewModel;
        }

        public async Task<IEnumerable<F_GEN_S_ISSUE_MASTER>> GetAllFGenSIssueMasterList()
        {
            return await DenimDbContext.F_GEN_S_ISSUE_MASTER
                .Include(d => d.ISSUE)
                .Include(d => d.GSR)
                .Select(d => new F_GEN_S_ISSUE_MASTER
                {
                    GISSUEID = d.GISSUEID,
                    EncryptedId = _protector.Protect(d.GISSUEID.ToString()),
                    GSRID = d.GSRID,
                    GSR = new F_GEN_S_REQ_MASTER
                    {
                        GSRNO = d.GSR.GSRNO
                    },
                    GISSUEDATE = d.GISSUEDATE,
                    ISSUE = new F_BAS_ISSUE_TYPE
                    {
                        ISSUTYPE = d.ISSUE.ISSUTYPE
                    },
                    ISSUETO = d.ISSUETO,
                    PURPOSE = d.PURPOSE,
                    ISRETURNABLE = d.ISRETURNABLE,
                    REMARKS = d.REMARKS,
                    IsLocked = d.CREATED_AT < DateTime.Now.AddDays(-2)
                }).ToListAsync();
        }

        public async Task<FGenSIssueViewModel> FindByIdIncludeAllAsync(int gsIssueId, bool edit = false)
        {
            return await DenimDbContext.F_GEN_S_ISSUE_MASTER
                .Include(d => d.ISSUE)
                .Include(d => d.GSR)
                //.ThenInclude(d => d.F_GEN_S_REQ_DETAILS)
                //.ThenInclude(d => d.PRODUCT)
                .Include(d => d.F_GEN_S_ISSUE_DETAILS)
                .ThenInclude(d => d.PRODUCT.UNITNavigation)
                .Where(d => d.GISSUEID.Equals(gsIssueId) && (!edit || d.CREATED_AT.Value.AddDays(2) > DateTime.Now))
                .Select(d => new FGenSIssueViewModel
                {
                    FGenSIssueMaster = new F_GEN_S_ISSUE_MASTER
                    {
                        GISSUEID = d.GISSUEID,
                        EncryptedId = _protector.Protect(d.GISSUEID.ToString()),
                        GISSUEDATE = d.GISSUEDATE,
                        ISSUEID = d.ISSUEID,
                        ISSUEBY = d.ISSUEBY,
                        RECEIVEBY = d.RECEIVEBY,
                        GSRID = d.GSRID,
                        GSR = d.GSR,
                        ISSUETO = d.ISSUETO,
                        PURPOSE = d.PURPOSE,
                        ISRETURNABLE = d.ISRETURNABLE,
                        REMARKS = d.REMARKS,
                        ISSUE = new F_BAS_ISSUE_TYPE
                        {
                            ISSUTYPE = d.ISSUE.ISSUTYPE
                        },
                        ISSUEBYNavigation = new F_HRD_EMPLOYEE
                        {
                            FIRST_NAME = $"{(d.ISSUEBYNavigation.EMPNO != null ? d.ISSUEBYNavigation.EMPNO + " -" : "")} {d.ISSUEBYNavigation.FIRST_NAME} {d.ISSUEBYNavigation.LAST_NAME}"
                        },
                        RECEIVEBYNavigation = new F_HRD_EMPLOYEE
                        {
                            FIRST_NAME = $"{(d.RECEIVEBYNavigation.EMPNO != null ? d.RECEIVEBYNavigation.EMPNO + " -" : "")} {d.RECEIVEBYNavigation.FIRST_NAME} {d.RECEIVEBYNavigation.LAST_NAME}"
                        }
                    },
                    FGenSReqMaster = new F_GEN_S_REQ_MASTER
                    {
                        GSRID = d.GSR.GSRID,
                        GSRNO = d.GSR.GSRNO,
                        GSRDATE = d.GSR.GSRDATE,
                        //F_GEN_S_REQ_DETAILS = d.GSR.F_GEN_S_REQ_DETAILS.Select(f => new F_GEN_S_REQ_DETAILS
                        //{
                        //    PRODUCT = new F_GS_PRODUCT_INFORMATION
                        //    {
                        //        PRODID = f.PRODUCT.PRODID,
                        //        PRODNAME = $"{d.PRODID} - {d.PRODNAME} {(d.PARTNO != "" ? " - " + d.PARTNO : "")}"
                        //    }
                        //}).ToList()
                    },
                    FGenSIssueDetailsesList = d.F_GEN_S_ISSUE_DETAILS.Select(f => new F_GEN_S_ISSUE_DETAILS
                    {
                        GISSDID = f.GISSDID,
                        GISSDDATE = f.GISSDDATE,
                        GRCVIDD = f.GRCVIDD,
                        GISSUEID = f.GISSUEID,
                        GREQ_DET_ID = f.GREQ_DET_ID,
                        PRODUCTID = f.PRODUCTID,
                        ADJ_PRO_AGNST = f.ADJ_PRO_AGNST,
                        ISSUE_QTY = f.ISSUE_QTY,
                        REMARKS = f.REMARKS,
                        PRODUCT = new F_GS_PRODUCT_INFORMATION
                        {
                            PRODID = f.PRODUCT.PRODID,
                            PRODNAME = $"{f.PRODUCT.PRODID} - {f.PRODUCT.PRODNAME} {(f.PRODUCT.PARTNO != "" ? " - " + f.PRODUCT.PARTNO : "")}",
                            UNITNavigation = new F_BAS_UNITS
                            {
                                UNAME = f.PRODUCT.UNITNavigation.UNAME
                            }
                        }
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<FGenSIssueViewModel> GetInitObjForDetailsByAsync(FGenSIssueViewModel fGenSIssueViewModel)
        {
            foreach (var item in fGenSIssueViewModel.FGenSIssueDetailsesList)
            {
                item.PRODUCT = await DenimDbContext.F_GS_PRODUCT_INFORMATION
                    .Include(d => d.UNITNavigation)
                    .Where(d => d.PRODID.Equals(item.PRODUCTID))
                    .Select(d => new F_GS_PRODUCT_INFORMATION
                    {
                        PRODNAME = $"{d.PRODID} - {d.PRODNAME} - {d.PARTNO}",
                        UNITNavigation = new F_BAS_UNITS
                        {
                            UNAME = d.UNITNavigation.UNAME
                        }
                    }).FirstOrDefaultAsync();

                //item.GRCVIDDNavigation = await _denimDbContext.F_GEN_S_RECEIVE_DETAILS.FirstOrDefaultAsync(e => e.TRNSID.Equals(item.GRCVIDD));
            }

            return fGenSIssueViewModel;
        }
    }
}
