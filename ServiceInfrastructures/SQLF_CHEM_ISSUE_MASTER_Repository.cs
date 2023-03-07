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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_CHEM_ISSUE_MASTER_Repository : BaseRepository<F_CHEM_ISSUE_MASTER>, IF_CHEM_ISSUE_MASTER
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtector _protector;

        public SQLF_CHEM_ISSUE_MASTER_Repository(DenimDbContext denimDbContext,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_CHEM_ISSUE_MASTER>> GetAllChemIssueMasterList()
        {
            return await DenimDbContext.F_CHEM_ISSUE_MASTER
                    .Include(e => e.CISSUE)
                    .Include(e => e.CSR)
                    .Select(e => new F_CHEM_ISSUE_MASTER
                    {
                        CISSUEID = e.CISSUEID,
                        EncryptedId = _protector.Protect(e.CISSUEID.ToString()),
                        CSRID = e.CSRID,
                        CSR = new F_CHEM_REQ_MASTER
                        {
                            CSRNO = e.CSR.CSRNO
                        },
                        CISSUEDATE = e.CISSUEDATE,
                        CISSUE = new F_BAS_ISSUE_TYPE
                        {
                            ISSUTYPE = e.CISSUE.ISSUTYPE
                        },
                        ISSUETO = e.ISSUETO,
                        PURPOSE = e.PURPOSE,
                        ISRETURNABLE = e.ISRETURNABLE,
                        REMARKS = e.REMARKS,
                        IsLocked = e.CREATED_AT < DateTime.Now.AddDays(-2)
                    }).ToListAsync();
        }

        public async Task<FChemIssueViewModel> GetInitObjByAsync(FChemIssueViewModel fChemIssueViewModel)
        {
            var ignores = new[]
            {
                "inspection",
                "Recone(LCB)"
            };

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            fChemIssueViewModel.IssueFHrEmployees = await DenimDbContext.F_HR_EMP_OFFICIALINFO
                .Where(e => DenimDbContext.F_HR_EMP_OFFICIALINFO
                    .Where(f => f.EMPID.Equals(user.EMPID)).Any(f => f.DEPTID.Equals(e.DEPTID)))
                .Include(e => e.EMP)
                .Select(e => new F_HRD_EMPLOYEE
                {
                    EMPID = e.EMP.EMPID,
                    FIRST_NAME = $"{e.EMP.EMPNO}, {e.EMP.FIRST_NAME} {e.EMP.LAST_NAME}"
                }).ToListAsync();

            fChemIssueViewModel.ReceiveFHrEmployees = await DenimDbContext.F_CHEM_PURCHASE_REQUISITION_MASTER
                .Include(e => e.Employee)
                .Select(e => new F_HRD_EMPLOYEE
                {
                    EMPID = e.Employee.EMPID,
                    FIRST_NAME = $"{e.Employee.EMPNO}, {e.Employee.FIRST_NAME} {e.Employee.LAST_NAME}"
                }).OrderBy(e => e.FIRST_NAME).ToListAsync();

            fChemIssueViewModel.FBasIssueTypesList = await DenimDbContext.F_BAS_ISSUE_TYPE
                .Where(e => !ignores.Any(f => f.ToLower().Contains(e.ISSUTYPE.ToLower())))
                .Select(e => new F_BAS_ISSUE_TYPE
                {
                    ISSUID = e.ISSUID,
                    ISSUTYPE = e.ISSUTYPE

                }).OrderBy(e => e.ISSUTYPE).ToListAsync();

            fChemIssueViewModel.FChemReqMastersList = await DenimDbContext.F_CHEM_REQ_MASTER
                .Select(e => new F_CHEM_REQ_MASTER
                {
                    CSRID = e.CSRID,
                    CSRNO = e.CSRNO
                }).OrderByDescending(e => e.CSRNO).ToListAsync();

            fChemIssueViewModel.FChemStoreProductinfos = fChemIssueViewModel.FChemIssueMaster.ISSUEID switch
            {
                300001 => DenimDbContext.F_CHEM_TRANSECTION.Include(e => e.PRODUCT)
                    .Include(e => e.CRCV)
                    .Where(e => e.BALANCE > 0)
                    .Select(e => new F_CHEM_STORE_PRODUCTINFO
                    {
                        PRODUCTID = e.CRCV.TRNSID,
                        PRODUCTNAME = $"{e.PRODUCT.PRODUCTNAME}, Batch no: {e.CRCV.BATCHNO}"
                    }).ToLookup(e => e.PRODUCTID).Select(e => e.FirstOrDefault()).ToList(),
                _ => await DenimDbContext.F_CHEM_STORE_PRODUCTINFO
                    .Select(e => new F_CHEM_STORE_PRODUCTINFO {PRODUCTID = e.PRODUCTID, PRODUCTNAME = e.PRODUCTNAME})
                    .OrderBy(e => e.PRODUCTNAME).ToListAsync()
            };

            return fChemIssueViewModel;
        }

        public async Task<FChemIssueViewModel> GetInitObjForDetailsByAsync(FChemIssueViewModel fChemIssueViewModel)
        {
            foreach (var item in fChemIssueViewModel.FChemIssueDetailsList)
            {
                item.PRODUCT = fChemIssueViewModel.FChemIssueMaster.ISSUEID switch
                {
                    300001 => await DenimDbContext.F_CHEM_STORE_RECEIVE_DETAILS
                        .Where(e => e.TRNSID.Equals(item.PRODUCTID))
                        .Select(e => e.FChemStoreProductinfo)
                        .FirstOrDefaultAsync(),
                    _ => await DenimDbContext.F_CHEM_STORE_PRODUCTINFO.FirstOrDefaultAsync(e =>
                        e.PRODUCTID.Equals(item.PRODUCTID))
                };

                item.CRCVIDDNavigation = await DenimDbContext.F_CHEM_STORE_RECEIVE_DETAILS.FirstOrDefaultAsync(e => e.TRNSID.Equals(item.CRCVIDD));
            }

            return fChemIssueViewModel;
        }

        public async Task<FChemIssueViewModel> FindByIdIncludeAllAsync(int cIssueId, bool edit = false)
        {
            if (await DenimDbContext.F_CHEM_ISSUE_MASTER.AnyAsync(e => e.CISSUEID.Equals(cIssueId) && e.ISSUEID.Equals(300001)))
            {
                var fChemIssueViewModel = await DenimDbContext.F_CHEM_ISSUE_MASTER
                    .Include(e => e.IssueFHrdEmployee)
                    .Include(e => e.ReceiveFHrdEmployee)
                    .Include(e => e.F_CHEM_ISSUE_DETAILS)
                    .ThenInclude(e => e.PRODUCT)
                    .Include(e => e.F_CHEM_ISSUE_DETAILS)
                    .ThenInclude(e => e.CRCVIDDNavigation)
                    .Where(e => e.CISSUEID.Equals(cIssueId))
                    .Select(e => new FChemIssueViewModel
                    {
                        FChemIssueMaster = new F_CHEM_ISSUE_MASTER
                        {
                            CISSUEID = e.CISSUEID,
                            EncryptedId = _protector.Protect(e.CISSUEID.ToString()),
                            CISSUEDATE = e.CISSUEDATE,
                            ISSUEID = e.ISSUEID,
                            ISSUEBY = e.ISSUEBY,
                            RECEIVEBY = e.RECEIVEBY,
                            CSRID = e.CSRID,
                            ISSUETO = e.ISSUETO,
                            PURPOSE = e.PURPOSE,
                            ISRETURNABLE = e.ISRETURNABLE,
                            REMARKS = e.REMARKS,
                            OPT1 = e.OPT1,
                            OPT2 = e.OPT2,
                            CISSUE = e.CISSUE,
                            IssueFHrdEmployee = e.IssueFHrdEmployee,
                            ReceiveFHrdEmployee = e.ReceiveFHrdEmployee,
                            IsLocked = e.CREATED_AT.Value.AddDays(2) > DateTime.Now
                        },
                        FChemIssueDetailsList = e.F_CHEM_ISSUE_DETAILS.Select(f => new F_CHEM_ISSUE_DETAILS
                        {
                            CISSDID = f.CISSDID,
                            CISSDDATE = f.CISSDDATE,
                            CRCVIDD = f.CRCVIDD,
                            CISSUEID = f.CISSUEID,
                            CREQ_DET_ID = f.CREQ_DET_ID,
                            PRODUCTID = f.PRODUCTID,
                            ADJ_PRO_AGNST = f.ADJ_PRO_AGNST,
                            ISSUE_QTY = f.ISSUE_QTY,
                            REMARKS = f.REMARKS,
                            OTP1 = f.OTP1,
                            OTP2 = f.OTP2,
                            PRODUCT = new F_CHEM_STORE_PRODUCTINFO
                            {
                                PRODUCTID = f.PRODUCT.PRODUCTID,
                                PRODUCTNAME = f.PRODUCT.PRODUCTNAME
                            },
                            CRCVIDDNavigation = new F_CHEM_STORE_RECEIVE_DETAILS
                            {
                                BATCHNO = f.CRCVIDDNavigation.BATCHNO
                            }
                        }).ToList()
                    }).FirstOrDefaultAsync();

                return fChemIssueViewModel;
            }
            else
            {
                var fChemIssueViewModel = await DenimDbContext.F_CHEM_ISSUE_MASTER
                .Include(e => e.CSR.DEPT)
                .Include(e => e.CSR.F_CHEM_REQ_DETAILS)
                .ThenInclude(e => e.PRODUCT)
                .Include(e => e.CISSUE)
                .Include(e => e.IssueFHrdEmployee)
                .Include(e => e.ReceiveFHrdEmployee)
                .Include(e => e.F_CHEM_ISSUE_DETAILS)
                .ThenInclude(e => e.CRCVIDDNavigation)
                .Include(e => e.F_CHEM_ISSUE_DETAILS)
                .ThenInclude(e => e.PRODUCT)
                .Where(e => e.CISSUEID.Equals(cIssueId))
                .Select(e => new FChemIssueViewModel
                {
                    FChemIssueMaster = new F_CHEM_ISSUE_MASTER
                    {
                        CISSUEID = e.CISSUEID,
                        EncryptedId = _protector.Protect(e.CISSUEID.ToString()),
                        CISSUEDATE = e.CISSUEDATE,
                        ISSUEID = e.ISSUEID,
                        ISSUEBY = e.ISSUEBY,
                        RECEIVEBY = e.RECEIVEBY,
                        CSRID = e.CSRID,
                        CSR = e.CSR,
                        ISSUETO = e.ISSUETO,
                        PURPOSE = e.PURPOSE,
                        ISRETURNABLE = e.ISRETURNABLE,
                        REMARKS = e.REMARKS,
                        OPT1 = e.OPT1,
                        OPT2 = e.OPT2,
                        CISSUE = e.CISSUE,
                        IsLocked = e.CREATED_AT.Value.AddDays(2) > DateTime.Now
                    },
                    FChemReqMaster = new F_CHEM_REQ_MASTER
                    {
                        CSRDATE = e.CSR.CSRDATE,
                        DEPT = new F_BAS_DEPARTMENT
                        {
                            DEPTID = e.CSR.DEPT.DEPTID,
                            DEPTNAME = e.CSR.DEPT.DEPTNAME
                        },
                        F_CHEM_REQ_DETAILS = e.CSR.F_CHEM_REQ_DETAILS.Select(f => new F_CHEM_REQ_DETAILS
                        {
                            PRODUCT = new F_CHEM_STORE_PRODUCTINFO
                            {
                                PRODUCTID = f.PRODUCT.PRODUCTID,
                                PRODUCTNAME = f.PRODUCT.PRODUCTNAME
                            }
                        }).ToList()
                    },
                    FChemIssueDetailsList = e.F_CHEM_ISSUE_DETAILS.Select(f => new F_CHEM_ISSUE_DETAILS
                    {
                        CISSDID = f.CISSDID,
                        CISSDDATE = f.CISSDDATE,
                        CRCVIDD = f.CRCVIDD,
                        CISSUEID = f.CISSUEID,
                        CREQ_DET_ID = f.CREQ_DET_ID,
                        PRODUCTID = f.PRODUCTID,
                        ADJ_PRO_AGNST = f.ADJ_PRO_AGNST,
                        ISSUE_QTY = f.ISSUE_QTY,
                        REMARKS = f.REMARKS,
                        OTP1 = f.OTP1,
                        OTP2 = f.OTP2,
                        PRODUCT = new F_CHEM_STORE_PRODUCTINFO
                        {
                            PRODUCTID = f.PRODUCT.PRODUCTID,
                            PRODUCTNAME = f.PRODUCT.PRODUCTNAME
                        },
                        CRCVIDDNavigation = new F_CHEM_STORE_RECEIVE_DETAILS
                        {
                            BATCHNO = f.CRCVIDDNavigation.BATCHNO
                        }
                    }).ToList()
                }).FirstOrDefaultAsync();

                return fChemIssueViewModel;
            }
        }
    }
}
