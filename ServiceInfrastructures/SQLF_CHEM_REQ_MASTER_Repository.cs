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
    public class SQLF_CHEM_REQ_MASTER_Repository : BaseRepository<F_CHEM_REQ_MASTER>, IF_CHEM_REQ_MASTER
    {
        private readonly IDataProtector _protector;

        public SQLF_CHEM_REQ_MASTER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_CHEM_REQ_MASTER>> GetAllChemicalRequirementAsync()
        {
            return await DenimDbContext.F_CHEM_REQ_MASTER
                     .Include(r => r.DEPT)
                     .Include(e => e.FBasSection)
                     .Include(e => e.FBasSubsection)
                     .Select(e => new F_CHEM_REQ_MASTER
                     {
                         CSRID = e.CSRID,
                         EncryptedId = _protector.Protect(e.CSRID.ToString()),
                         CSRNO = e.CSRNO,
                         CSRDATE = e.CSRDATE,
                         DEPTID = e.DEPTID,
                         SECID = e.SECID,
                         DEPT = new F_BAS_DEPARTMENT
                         {
                             DEPTNAME = e.DEPT.DEPTNAME
                         },
                         FBasSection = new F_BAS_SECTION
                         {
                             SECNAME = e.FBasSection.SECNAME
                         },
                         FBasSubsection = new F_BAS_SUBSECTION
                         {
                             SSECNAME = e.FBasSubsection.SSECNAME
                         },
                         REMARKS = e.REMARKS,
                         IsLocked = DenimDbContext.F_CHEM_ISSUE_MASTER.Any(f => !f.CSRID.Equals(e.CSRID)) || e.CREATED_AT.Value.AddDays(2) <= DateTime.Now
                     }).ToListAsync();
        }

        public async Task<IEnumerable<FChemIssueViewModel>> GetChemReqMaster(int id)
        {
            var result = await DenimDbContext.F_CHEM_REQ_MASTER
                .Include(e => e.DEPT)
                .Include(e => e.FBasSection)
                .Include(e => e.F_CHEM_REQ_DETAILS)
                .ThenInclude(e => e.PRODUCT)
                .Where(e => e.CSRID.Equals(id))
                .Select(e => new FChemIssueViewModel
                {
                    FChemReqMaster = new F_CHEM_REQ_MASTER
                    {
                        CSRID = e.CSRID,
                        CSRNO = e.CSRNO,
                        CSRDATE = e.CSRDATE,
                        DEPTID = e.DEPTID,
                        SECID = e.SECID,
                        DEPT = e.DEPT,
                        REMARKS = e.REMARKS
                    },
                    FChemReqDetailsesList = e.F_CHEM_REQ_DETAILS.Select(f => new F_CHEM_REQ_DETAILS
                    {
                        PRODUCT = new F_CHEM_STORE_PRODUCTINFO
                        {
                            PRODUCTID = f.PRODUCT.PRODUCTID,
                            PRODUCTNAME = f.PRODUCT.PRODUCTNAME
                        }
                    }).ToList(),
                }).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<F_CHEM_REQ_MASTER>> GetRequirementDD()
        {
            var result = await DenimDbContext.F_CHEM_REQ_MASTER
                .Where(c => !DenimDbContext.F_CHEM_ISSUE_MASTER.Any(e => e.CSRID.Equals(c.CSRID)))
                .Select(e => new F_CHEM_REQ_MASTER
                {
                    CSRID = e.CSRID,
                    CSRNO = e.CSRNO
                }).ToListAsync();

            return result;
        }

        public async Task<FChemRequirementViewModel> GetInitObjByAsync(FChemRequirementViewModel fChemRequirementViewModel)
        {
            try
            {
                fChemRequirementViewModel.ReqEmployees = await DenimDbContext.F_HRD_EMPLOYEE
                    .Select(e => new F_HRD_EMPLOYEE
                    {
                        EMPID = e.EMPID,
                        FIRST_NAME = $"{e.EMPNO}, {e.FIRST_NAME} {e.LAST_NAME}"
                    }).OrderBy(e => e.FIRST_NAME).ToListAsync();

                var x = DenimDbContext.F_CHEM_TRANSECTION.GroupBy(x => x.CRCVID, (key, g) => g.OrderByDescending(m => m.CTRID).First()).Where(f => f.PRODUCTID.Equals(400000419)).Sum(f => f.BALANCE);

                fChemRequirementViewModel.FChemStoreProductinfosList = await DenimDbContext.F_CHEM_STORE_PRODUCTINFO
                    .Include(e => e.UNITNAVIGATION)
                    //.Where(d=> _denimDbContext.F_CHEM_STORE_RECEIVE_DETAILS.Where(f=>f.PRODUCTID.Equals(d.PRODUCTID)).Sum(f=>f.FRESH_QTY ?? 0) - _denimDbContext.F_CHEM_ISSUE_DETAILS.Where(f => f.PRODUCTID.Equals(d.PRODUCTID)).Sum(f => f.ISSUE_QTY ?? 0) > 0)

                    //.Where(e =>
                    //_denimDbContext.F_CHEM_TRANSECTION.GroupBy(x => x.CRCVID, (key, g) => g.OrderByDescending(m => m.CTRID).First()).Where(f => e.PRODUCTID.Equals(f.PRODUCTID)).Sum(f => f.BALANCE) > 0
                    // )
                    .Select(e => new F_CHEM_STORE_PRODUCTINFO
                    {
                        PRODUCTID = e.PRODUCTID,
                        PRODUCTNAME = $"{e.OLD_CODE} - {e.PROD_CODE} - {e.PRODUCTNAME}, Balance: {DenimDbContext.F_CHEM_STORE_RECEIVE_DETAILS.Where(f => f.PRODUCTID.Equals(e.PRODUCTID)).Sum(f => f.FRESH_QTY ?? 0) - DenimDbContext.F_CHEM_ISSUE_DETAILS.Where(f => f.PRODUCTID.Equals(e.PRODUCTID)).Sum(f => f.ISSUE_QTY ?? 0)} {e.UNITNAVIGATION.UNAME}"

                        //PRODUCTNAME = $"{e.OLD_CODE} - {e.PROD_CODE} - {e.PRODUCTNAME}, Balance: {_denimDbContext.F_CHEM_TRANSECTION.GroupBy(transection => transection.CRCVID, (key, g) => g.OrderByDescending(m => m.CTRID).First()).Where(f => e.PRODUCTID.Equals(f.PRODUCTID)).Sum(f => f.BALANCE):N} {e.UNITNAVIGATION.UNAME}"
                    }).OrderBy(e => e.PRODUCTNAME).ToListAsync();

                fChemRequirementViewModel.FBasDepartmentsList = await DenimDbContext.F_BAS_DEPARTMENT.Select(e => new F_BAS_DEPARTMENT
                {
                    DEPTID = e.DEPTID,
                    DEPTNAME = e.DEPTNAME
                }).OrderBy(e => e.DEPTNAME).ToListAsync();

                if (fChemRequirementViewModel.FChemReqMaster is { SECID: { } })
                {
                    fChemRequirementViewModel.FBasSectionsList = await DenimDbContext.F_BAS_SECTION.Select(e => new F_BAS_SECTION
                    {
                        SECID = e.SECID,
                        SECNAME = e.SECNAME
                    }).OrderBy(e => e.SECNAME).ToListAsync();
                }

                if (fChemRequirementViewModel.FChemReqMaster is { SSECID: { } })
                {
                    fChemRequirementViewModel.FBasSubsections = await DenimDbContext.F_BAS_SUBSECTION.Select(e => new F_BAS_SUBSECTION
                    {
                        SSECID = e.SSECID,
                        SSECNAME = e.SSECNAME
                    }).OrderBy(e => e.SSECNAME).ToListAsync();
                }

                fChemRequirementViewModel.FRndDyeingTypesList = await DenimDbContext.RND_DYEING_TYPE.Select(e => new RND_DYEING_TYPE
                {
                    DID = e.DID,
                    DTYPE = e.DTYPE
                }).OrderBy(e => e.DTYPE).ToListAsync();

                fChemRequirementViewModel.BasUnits = await DenimDbContext.F_BAS_UNITS.Select(e => new F_BAS_UNITS
                {
                    UID = e.UID,
                    UNAME = e.UNAME
                }).OrderBy(e => e.UNAME).ToListAsync();

                return fChemRequirementViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<F_BAS_SUBSECTION>> GetSubSectionsBySectionIdAsync(int sectionId)
        {
            var fBasSection = await DenimDbContext.F_BAS_SECTION
                    .Include(e => e.F_BAS_SUBSECTION)
                    .FirstOrDefaultAsync(e => e.SECID.Equals(sectionId));

            return fBasSection.F_BAS_SUBSECTION;
        }

        public async Task<FChemRequirementViewModel> GetInitDetailsObjByAsync(FChemRequirementViewModel fChemRequirementViewModel)
        {
            foreach (var item in fChemRequirementViewModel.FChemReqDetailsList)
            {
                item.PRODUCT = await DenimDbContext.F_CHEM_STORE_PRODUCTINFO.FirstOrDefaultAsync(e => e.PRODUCTID.Equals(item.PRODUCTID));
            }

            return fChemRequirementViewModel;
        }

        public async Task<FChemRequirementViewModel> FindByIdIncludeAllAsync(int csrId)
        {
            try
            {
                var result = await DenimDbContext.F_CHEM_REQ_MASTER
                    .Include(e => e.RequisitionEmployee)
                    .Include(e => e.F_CHEM_REQ_DETAILS)
                    .ThenInclude(e => e.PRODUCT)
                    .Where(e => e.CSRID.Equals(csrId))
                    .Select(e => new FChemRequirementViewModel
                    {
                        FChemReqMaster = new F_CHEM_REQ_MASTER
                        {
                            CSRID = e.CSRID,
                            EncryptedId = _protector.Protect(e.CSRID.ToString()),
                            CSRNO = e.CSRNO,
                            CSRDATE = e.CSRDATE,
                            REQUISITIONBY = e.REQUISITIONBY,
                            DEPTID = e.DEPTID,
                            SECID = e.SECID,
                            SSECID = e.SSECID,
                            REMARKS = e.REMARKS,
                            OPT1 = e.OPT1,
                            OPT2 = e.OPT2,
                            DEPT = new F_BAS_DEPARTMENT
                            {
                                DEPTID = e.DEPT != null ? e.DEPT.DEPTID : 0,
                                DEPTNAME = e.DEPT != null ? e.DEPT.DEPTNAME : ""
                            },
                            FBasSection = new F_BAS_SECTION
                            {
                                SECID = e.FBasSection != null ? e.FBasSection.SECID : 0,
                                SECNAME = e.FBasSection != null ? e.FBasSection.SECNAME : ""
                            },
                            FBasSubsection = new F_BAS_SUBSECTION
                            {
                                SSECID = e.FBasSubsection != null ? e.FBasSubsection.SSECID : 0,
                                SSECNAME = e.FBasSubsection != null ? e.FBasSubsection.SSECNAME : ""
                            },
                            RequisitionEmployee = new F_HRD_EMPLOYEE
                            {
                                FIRST_NAME = e.RequisitionEmployee != null ? $"{e.RequisitionEmployee.FIRST_NAME} {e.RequisitionEmployee.LAST_NAME}" : ""
                            }
                        },
                        FChemReqDetailsList = e.F_CHEM_REQ_DETAILS.Select(f => new F_CHEM_REQ_DETAILS
                        {
                            CRQID = f.CRQID,
                            CSRID = f.CSRID,
                            PRODUCTID = f.PRODUCTID,
                            REQ_QTY = f.REQ_QTY,
                            STATUS = f.STATUS,
                            REMARKS = f.REMARKS,
                            OPT1 = f.OPT1,
                            OPT2 = f.OPT2,
                            OPT3 = f.OPT3,
                            PRODUCT = new F_CHEM_STORE_PRODUCTINFO
                            {
                                PRODUCTID = f.PRODUCT != null ? f.PRODUCT.PRODUCTID : 0,
                                PRODUCTNAME = f.PRODUCT != null ? f.PRODUCT.PRODUCTNAME : ""
                            }
                        }).ToList()
                    }).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
