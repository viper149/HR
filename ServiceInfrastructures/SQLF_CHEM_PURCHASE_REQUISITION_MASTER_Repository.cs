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
    public class SQLF_CHEM_PURCHASE_REQUISITION_MASTER_Repository : BaseRepository<F_CHEM_PURCHASE_REQUISITION_MASTER>, IF_CHEM_PURCHASE_REQUISITION_MASTER
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;

        public SQLF_CHEM_PURCHASE_REQUISITION_MASTER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager) : base(denimDbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_CHEM_PURCHASE_REQUISITION_MASTER>> GetAllChemicalPurchaseRequisitionAsync()
        {
            return await DenimDbContext.F_CHEM_PURCHASE_REQUISITION_MASTER
                .Include(e => e.FBasSubsection)
                .Include(e => e.FBasSection)
                .Include(e => e.Employee)
                .Include(e => e.ConcernEmployee)
                .Select(e => new F_CHEM_PURCHASE_REQUISITION_MASTER
                {
                    INDSLID = e.INDSLID,
                    EncryptedId = _protector.Protect(e.INDSLID.ToString()),
                    INDSLDATE = e.INDSLDATE,
                    FBasSubsection = e.FBasSubsection,
                    FBasDepartment = new F_BAS_DEPARTMENT
                    {
                        DEPTNAME = e.FBasDepartment.DEPTNAME
                    },
                    FBasSection = new F_BAS_SECTION
                    {
                        SECNAME = e.FBasSection.SECNAME
                    },
                    Employee = new F_HRD_EMPLOYEE
                    {
                        FIRST_NAME = e.Employee.FIRST_NAME,
                        LAST_NAME = e.Employee.LAST_NAME
                    },
                    ConcernEmployee = new F_HRD_EMPLOYEE
                    {
                        FIRST_NAME = e.ConcernEmployee.FIRST_NAME,
                        LAST_NAME = e.ConcernEmployee.LAST_NAME
                    },
                    REMARKS = e.REMARKS,
                    STATUS = e.STATUS,
                    IsLocked = DenimDbContext.F_CHEM_STORE_INDENTMASTER.Any(f => !f.INDSLID.Equals(e.INDSLID)) || e.CREATED_AT.Value.AddDays(2) <= DateTime.Now
                }).ToListAsync();
        }

        public async Task<F_CHEM_PURCHASE_REQUISITION_MASTER> GetChemPurReqMasterById(int id)
        {
            try
            {
                var fChemPurchaseRequisitionMaster = await DenimDbContext.F_CHEM_PURCHASE_REQUISITION_MASTER
                    .Include(e => e.Employee)
                    .Include(e => e.ConcernEmployee)
                    .Include(e => e.FBasDepartment)
                    .Include(e => e.FBasSection)
                    .Include(e => e.F_CHEM_STORE_INDENTDETAILS)
                    .ThenInclude(e => e.PRODUCT)
                    .FirstOrDefaultAsync(e => e.INDSLID.Equals(id));
                return fChemPurchaseRequisitionMaster;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FChemicalRequisitionViewModel> GetInitObjByAsync(FChemicalRequisitionViewModel requisitionViewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

                var fHrEmployee = await DenimDbContext.F_HRD_EMPLOYEE
                    .Include(d=>d.DEPT)
                    .Include(d=>d.SEC)
                    .Include(d=>d.SUBSEC)
                    .Where(d => d.EMPID.Equals(user.EMPID))
                    .Select(d => new F_HRD_EMPLOYEE
                    {
                        EMPID = d.EMPID,
                        DEPT = new F_BAS_HRD_DEPARTMENT
                        {
                            DEPTID = d.DEPT.DEPTID,
                            DEPTNAME = d.DEPTID != null ? d.DEPT.DEPTNAME : "N/A"
                        },
                        SEC = new F_BAS_HRD_SECTION
                        {
                            SECID = d.SEC.SECID,
                            SEC_NAME = d.SECID != null ? d.SEC.SEC_NAME : "N/A"
                        },
                        SUBSEC = new F_BAS_HRD_SUB_SECTION
                        {
                            SUBSECID = d.SUBSEC.SUBSECID,
                            SUBSEC_NAME = d.SUBSEC != null ? d.SUBSEC.SUBSEC_NAME : "N/A"
                        }
                    }).FirstOrDefaultAsync();


                requisitionViewModel.FBasSubsections = await DenimDbContext.F_BAS_SUBSECTION
                .Select(d => new F_BAS_SUBSECTION
                {
                    SSECID = d.SSECID,
                    SSECNAME = d.SSECNAME
                }).OrderByDescending(d => d.SSECNAME).ToListAsync();

                //requisitionViewModel.FBasSubsections = await _denimDbContext.F_BAS_SUBSECTION.Select(e => new F_BAS_SUBSECTION
                //{
                //    SSECID = e.SSECID,
                //    SSECNAME = e.SSECNAME
                //}).OrderByDescending(e => e.SSECNAME).ToListAsync();

                requisitionViewModel.FBasSectionList = await DenimDbContext.F_BAS_SECTION
               .Select(d => new F_BAS_SECTION
               {
                   SECID = d.SECID,
                   SECNAME = d.SECNAME
               }).OrderByDescending(d => d.SECNAME).ToListAsync();

                //requisitionViewModel.FBasSectionList = await _denimDbContext.F_BAS_SECTION.Select(e => new F_BAS_SECTION
                //{
                //    SECID = e.SECID,
                //    SECNAME = e.SECNAME
                //}).OrderByDescending(e => fHrEmployee.F_HR_EMP_OFFICIALINFO.Select(f => f.SEC.SECID).Contains(e.SECID)).ThenBy(e => e.SECNAME).ToListAsync();

                requisitionViewModel.DepartmentList = await DenimDbContext.F_BAS_DEPARTMENT
               .Select(d => new F_BAS_DEPARTMENT
               {
                   DEPTID = d.DEPTID,
                   DEPTNAME = d.DEPTNAME
               }).OrderByDescending(d => d.DEPTNAME).ToListAsync();

                //requisitionViewModel.DepartmentList = await _denimDbContext.F_BAS_DEPARTMENT.Select(e => new F_BAS_DEPARTMENT
                //{
                //    DEPTID = e.DEPTID,
                //    DEPTNAME = e.DEPTNAME
                //}).OrderByDescending(e => fHrEmployee.F_HR_EMP_OFFICIALINFO.Select(f => f.DEPT.DEPTID).Contains(e.DEPTID)).ThenBy(e => e.DEPTNAME).ToListAsync();

                requisitionViewModel.FHrEmployeesList = await DenimDbContext.F_HRD_EMPLOYEE
              .Select(d => new F_HRD_EMPLOYEE
              {
                  EMPID = d.EMPID,
                  EMPNO = $"{(d.EMPNO != null ? d.EMPNO + " -" : "")} {d.FIRST_NAME} {d.LAST_NAME}"
              }).OrderBy(d => d.EMPNO).ToListAsync();

                //requisitionViewModel.FHrEmployeesList = await _denimDbContext.F_HRD_EMPLOYEE.Select(e => new F_HRD_EMPLOYEE
                //{
                //    EMPID = e.EMPID,
                //    EMPNO = $"{e.EMPNO}, {e.FIRST_NAME} {e.LAST_NAME}"
                //}).OrderBy(e => e.EMPNO).ToListAsync();

                requisitionViewModel.FChemStoreProductinfoList = await DenimDbContext.F_CHEM_STORE_PRODUCTINFO
              .Select(d => new F_CHEM_STORE_PRODUCTINFO
              {
                  PRODUCTID = d.PRODUCTID,
                  PRODUCTNAME = d.PRODUCTNAME
              }).OrderByDescending(d => d.PRODUCTNAME).ToListAsync();

                //requisitionViewModel.FChemStoreProductinfoList = await _denimDbContext.F_CHEM_STORE_PRODUCTINFO.Select(e => new F_CHEM_STORE_PRODUCTINFO
                //{
                //    PRODUCTID = e.PRODUCTID,
                //    PRODUCTNAME = e.PRODUCTNAME
                //}).OrderBy(e => e.PRODUCTNAME).ToListAsync();

                requisitionViewModel.BasUnits = await DenimDbContext.F_BAS_UNITS
            .Select(d => new F_BAS_UNITS
            {
                UID = d.UID,
                UNAME = d.UNAME
            }).OrderByDescending(e => e.UNAME.ToLower().Contains("kg")).ThenBy(e => e.UNAME).ToListAsync();

                //requisitionViewModel.BasUnits = await _denimDbContext.F_BAS_UNITS.Select(e => new F_BAS_UNITS
                //{
                //    UID = e.UID,
                //    UNAME = e.UNAME
                //}).OrderByDescending(e => e.UNAME.ToLower().Contains("kg")).ThenBy(e => e.UNAME).ToListAsync();

                return requisitionViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FChemicalRequisitionViewModel> FindByIdIncludeAllAsync(int indslId, bool edit = false)
        {
            if (edit)
            {
                var isLocked = await DenimDbContext.F_CHEM_STORE_INDENTMASTER.AnyAsync(f => f.INDSLID.Equals(indslId));

                if (isLocked)
                    return null;

                var rs = await DenimDbContext.F_CHEM_PURCHASE_REQUISITION_MASTER
                    .Include(e => e.FBasDepartment)
                    .Include(e => e.FBasSection)
                    .Include(e => e.FBasSubsection)
                    .Include(e => e.Employee)
                    .Include(e => e.ConcernEmployee)
                    .Include(e => e.F_CHEM_STORE_INDENTDETAILS)
                    .ThenInclude(e => e.FBasUnits)
                    .Include(e => e.F_CHEM_STORE_INDENTDETAILS)
                    .ThenInclude(e => e.PRODUCT)
                    .Where(e => e.INDSLID.Equals(indslId) && !isLocked && e.CREATED_AT > DateTime.Now.AddDays(-2))
                    .Select(e => new FChemicalRequisitionViewModel
                    {
                        FChemPurchaseRequisitionMaster = new F_CHEM_PURCHASE_REQUISITION_MASTER
                        {
                            INDSLID = e.INDSLID,
                            EncryptedId = _protector.Protect(e.INDSLID.ToString()),
                            INDSLDATE = e.INDSLDATE,
                            DEPTID = e.DEPTID,
                            SECID = e.SECID,
                            SSECID = e.SSECID,
                            EMPID = e.EMPID,
                            CN_PERSON = e.CN_PERSON,
                            REMARKS = e.REMARKS,
                            OPT1 = e.OPT1,
                            OPT2 = e.OPT2,
                            OPT3 = e.OPT3,
                            OPT4 = e.OPT4,
                            FBasDepartment = e.FBasDepartment,
                            FBasSection = e.FBasSection,
                            FBasSubsection = e.FBasSubsection,
                            Employee = e.Employee,
                            ConcernEmployee = e.ConcernEmployee,
                            IsLocked = isLocked
                        },
                        FChemStoreIndentdetailsList = e.F_CHEM_STORE_INDENTDETAILS.Select(f => new F_CHEM_STORE_INDENTDETAILS
                        {
                            TRNSID = f.TRNSID,
                            TRNSDATE = f.TRNSDATE,
                            CINDID = f.CINDID,
                            INDSLID = f.INDSLID,
                            VALIDITY = f.VALIDITY,
                            ADD_QTY = f.ADD_QTY,
                            BAL_QTY = f.BAL_QTY,
                            LOCATION = f.LOCATION,
                            PRODUCTID = f.PRODUCTID,
                            UNIT = f.UNIT,
                            QTY = f.QTY,
                            REMARKS = f.REMARKS,
                            OPT1 = f.OPT1,
                            OPT2 = f.OPT2,
                            OPT3 = f.OPT3,
                            PRODUCT = new F_CHEM_STORE_PRODUCTINFO
                            {
                                PRODUCTID = f.PRODUCT.PRODUCTID,
                                PRODUCTNAME = f.PRODUCT.PRODUCTNAME
                            },
                            FBasUnits = new F_BAS_UNITS
                            {
                                UID = f.FBasUnits.UID,
                                UNAME = f.FBasUnits.UNAME
                            }
                        }).ToList()
                    }).FirstOrDefaultAsync();

                return rs;
            }
            else
            {
                var isLocked = await DenimDbContext.F_CHEM_STORE_INDENTMASTER.AnyAsync(f => f.INDSLID.Equals(indslId));

                return await DenimDbContext.F_CHEM_PURCHASE_REQUISITION_MASTER
                    .Include(e => e.FBasDepartment)
                    .Include(e => e.FBasSection)
                    .Include(e => e.FBasSubsection)
                    .Include(e => e.Employee)
                    .Include(e => e.ConcernEmployee)
                    .Include(e => e.F_CHEM_STORE_INDENTDETAILS)
                    .ThenInclude(e => e.FBasUnits)
                    .Include(e => e.F_CHEM_STORE_INDENTDETAILS)
                    .ThenInclude(e => e.PRODUCT)
                    .Where(e => e.INDSLID.Equals(indslId))
                    .Select(e => new FChemicalRequisitionViewModel
                    {
                        FChemPurchaseRequisitionMaster = new F_CHEM_PURCHASE_REQUISITION_MASTER
                        {
                            INDSLID = e.INDSLID,
                            EncryptedId = _protector.Protect(e.INDSLID.ToString()),
                            INDSLDATE = e.INDSLDATE,
                            DEPTID = e.DEPTID,
                            SECID = e.SECID,
                            SSECID = e.SSECID,
                            EMPID = e.EMPID,
                            CN_PERSON = e.CN_PERSON,
                            REMARKS = e.REMARKS,
                            OPT1 = e.OPT1,
                            OPT2 = e.OPT2,
                            OPT3 = e.OPT3,
                            OPT4 = e.OPT4,
                            FBasDepartment = e.FBasDepartment,
                            FBasSection = e.FBasSection,
                            FBasSubsection = e.FBasSubsection,
                            Employee = e.Employee,
                            ConcernEmployee = e.ConcernEmployee,
                            IsLocked = isLocked || e.CREATED_AT > DateTime.Now.AddDays(-2)
                        },
                        FChemStoreIndentdetailsList = e.F_CHEM_STORE_INDENTDETAILS.Select(f => new F_CHEM_STORE_INDENTDETAILS
                        {
                            TRNSID = f.TRNSID,
                            TRNSDATE = f.TRNSDATE,
                            CINDID = f.CINDID,
                            INDSLID = f.INDSLID,
                            VALIDITY = f.VALIDITY,
                            ADD_QTY = f.ADD_QTY,
                            BAL_QTY = f.BAL_QTY,
                            LOCATION = f.LOCATION,
                            PRODUCTID = f.PRODUCTID,
                            UNIT = f.UNIT,
                            QTY = f.QTY,
                            REMARKS = f.REMARKS,
                            OPT1 = f.OPT1,
                            OPT2 = f.OPT2,
                            OPT3 = f.OPT3,
                            PRODUCT = new F_CHEM_STORE_PRODUCTINFO
                            {
                                PRODUCTID = f.PRODUCT.PRODUCTID,
                                PRODUCTNAME = f.PRODUCT.PRODUCTNAME
                            },
                            FBasUnits = new F_BAS_UNITS
                            {
                                UID = f.FBasUnits.UID,
                                UNAME = f.FBasUnits.UNAME
                            }
                        }).ToList()
                    }).FirstOrDefaultAsync();
            }
        }
    }
}