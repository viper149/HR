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
    public class SQLF_GEN_S_PURCHASE_REQUISITION_MASTER_Repository : BaseRepository<F_GEN_S_PURCHASE_REQUISITION_MASTER>, IF_GEN_S_PURCHASE_REQUISITION_MASTER
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;

        public SQLF_GEN_S_PURCHASE_REQUISITION_MASTER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager) : base(denimDbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<FGenSRequisitionViewModel> FindByIdIncludeAllAsync(int indslId, bool edit = false)
        {
            return await DenimDbContext.F_GEN_S_PURCHASE_REQUISITION_MASTER
                .Include(d => d.EMP)
                .Include(d => d.CN_PERSONNavigation)
                .Include(d => d.F_GEN_S_INDENTDETAILS)
                .ThenInclude(d => d.PRODUCT)
                .Where(d => d.INDSLID.Equals(indslId) && (!edit || !(DenimDbContext.F_GEN_S_INDENTMASTER.Any(f => f.INDSLID.Equals(d.INDSLID)) || d.CREATED_AT.Value.AddDays(2) <= DateTime.Now)))
                .Select(d => new FGenSRequisitionViewModel()
                {
                    FGenSPurchaseRequisitionMaster = new F_GEN_S_PURCHASE_REQUISITION_MASTER
                    {
                        INDSLID = d.INDSLID,
                        EncryptedId = _protector.Protect(d.INDSLID.ToString()),
                        INDSLDATE = d.INDSLDATE,
                        EMPID = d.EMPID,
                        CN_PERSON = d.CN_PERSON,
                        REMARKS = d.REMARKS,
                        EMP = new F_HRD_EMPLOYEE
                        {
                            EMPID = d.EMP.EMPID,
                            EMPNO = $"{(d.EMP.EMPNO != null ? d.EMP.EMPNO + " -" : "")} {d.EMP.FIRST_NAME} {d.EMP.LAST_NAME}"
                        },
                        CN_PERSONNavigation = new F_HRD_EMPLOYEE
                        {
                            EMPID = d.CN_PERSONNavigation.EMPID,
                            EMPNO = $"{(d.CN_PERSONNavigation.EMPNO != null ? d.CN_PERSONNavigation.EMPNO + " -" : "")} {d.CN_PERSONNavigation.FIRST_NAME} {d.CN_PERSONNavigation.LAST_NAME}"
                        }
                    },
                    FGenSIndentdetailsesList = d.F_GEN_S_INDENTDETAILS.Select(f => new F_GEN_S_INDENTDETAILS
                    {
                        TRNSID = f.TRNSID,
                        TRNSDATE = f.TRNSDATE,
                        GINDID = f.GINDID,
                        INDSLID = f.INDSLID,
                        VALIDITY = f.VALIDITY,
                        ADD_QTY = f.ADD_QTY,
                        BAL_QTY = f.BAL_QTY,
                        LOCATION = f.LOCATION,
                        PRODUCTID = f.PRODUCTID,
                        UNIT = f.UNIT,
                        QTY = f.QTY,
                        REMARKS = f.REMARKS,
                        PRODUCT = new F_GS_PRODUCT_INFORMATION
                        {
                            PRODID = f.PRODUCT.PRODID,
                            PRODNAME = $"{f.PRODUCT.PRODID} - {f.PRODUCT.PRODNAME} {(f.PRODUCT.PARTNO != "" ? " - " + f.PRODUCT.PARTNO : "")}",
                            UNITNavigation = new F_BAS_UNITS
                            {
                                UID = f.PRODUCT.UNITNavigation.UID,
                                UNAME = f.PRODUCT.UNITNavigation.UNAME
                            }
                        }
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<FGenSRequisitionViewModel> GetInitObjByAsync(FGenSRequisitionViewModel fGenSRequisitionViewModel)
        {
            fGenSRequisitionViewModel.FHrEmployeesList = await DenimDbContext.F_HRD_EMPLOYEE
                .Select(d => new F_HRD_EMPLOYEE
                {
                    EMPID = d.EMPID,
                    EMPNO = $"{(d.EMPNO != null ? d.EMPNO + " -" : "")} {d.FIRST_NAME} {d.LAST_NAME}"
                }).OrderBy(d => d.EMPNO).ToListAsync();

            fGenSRequisitionViewModel.FGsProductInformationsList = await DenimDbContext.F_GS_PRODUCT_INFORMATION
                .Select(d => new F_GS_PRODUCT_INFORMATION
                {
                    PRODID = d.PRODID,
                    PRODNAME = $"{d.PRODID} - {d.PRODNAME} {(d.PARTNO != "" ? " - " + d.PARTNO : "")}"
                }).OrderBy(d => d.PRODNAME).ToListAsync();

            return fGenSRequisitionViewModel;
        }

        public async Task<IEnumerable<F_GEN_S_PURCHASE_REQUISITION_MASTER>> GetAllGenSPurchaseRequisitionAsync()
        {
            return await DenimDbContext.F_GEN_S_PURCHASE_REQUISITION_MASTER
                .Include(d=>d.EMP)
                .Include(d=>d.CN_PERSONNavigation)
                .Select(d => new F_GEN_S_PURCHASE_REQUISITION_MASTER
                {
                    INDSLID = d.INDSLID,
                    EncryptedId = _protector.Protect(d.INDSLID.ToString()),
                    INDSLDATE = d.INDSLDATE,
                    EMPID = d.EMPID,
                    CN_PERSON = d.CN_PERSON,
                    EMP = new F_HRD_EMPLOYEE
                    {
                        EMPNO = $"{(d.EMP.EMPNO != null ? d.EMP.EMPNO + " -" : "")} {d.EMP.FIRST_NAME} {d.EMP.LAST_NAME}"
                    },
                    CN_PERSONNavigation = new F_HRD_EMPLOYEE
                    {
                        EMPNO = $"{(d.CN_PERSONNavigation.EMPNO != null ? d.CN_PERSONNavigation.EMPNO + " -" : "")} {d.CN_PERSONNavigation.FIRST_NAME} {d.CN_PERSONNavigation.LAST_NAME}"
                    },
                    REMARKS = d.REMARKS,
                    STATUS = d.STATUS,
                    IsLocked = DenimDbContext.F_GEN_S_INDENTMASTER.Any(f => f.INDSLID.Equals(d.INDSLID)) || d.CREATED_AT.Value.AddDays(2) <= DateTime.Now
                }).ToListAsync();
        }

        public async Task<F_GEN_S_PURCHASE_REQUISITION_MASTER> GetFGenSPurReqMasterById(int id)
        {
            return await DenimDbContext.F_GEN_S_PURCHASE_REQUISITION_MASTER
                .Include(d => d.EMP)
                .Include(d => d.CN_PERSONNavigation)
                .Include(d => d.F_GEN_S_INDENTDETAILS)
                .ThenInclude(e => e.PRODUCT)
                .ThenInclude(d => d.UNITNavigation)
                .FirstOrDefaultAsync(e => e.INDSLID.Equals(id));
        }
    }
}
