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
    public class SQLF_GEN_S_REQ_MASTER_Repository : BaseRepository<F_GEN_S_REQ_MASTER>, IF_GEN_S_REQ_MASTER
    {
        private readonly IDataProtector _protector;

        public SQLF_GEN_S_REQ_MASTER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<FGenSIssueViewModel>> GetGenSReqMaster(int id)
        {
            return await DenimDbContext.F_GEN_S_REQ_MASTER
                    .Include(d => d.EMP.DEPT)
                    .Include(d => d.EMP.SEC)
                    .Include(d => d.EMP.SUBSEC)
                    .Include(d => d.F_GEN_S_REQ_DETAILS)
                    .ThenInclude(d => d.PRODUCT)
                    .Where(d => d.GSRID.Equals(id))
                    .Select(d => new FGenSIssueViewModel
                    {
                        FGenSReqMaster = new F_GEN_S_REQ_MASTER
                        {
                            GSRID = d.GSRID,
                            GSRNO = d.GSRNO,
                            GSRDATE = d.GSRDATE,
                            REQUISITIONBY = d.REQUISITIONBY,
                            REMARKS = d.REMARKS,
                            EMP = new F_HRD_EMPLOYEE
                            {
                                EMPID = d.EMP.EMPID,
                                EMPNO = $"{d.EMP.EMPNO}, {d.EMP.FIRST_NAME} {d.EMP.LAST_NAME}",
                                DEPT = new F_BAS_HRD_DEPARTMENT
                                {
                                    DEPTID = d.EMP.DEPT.DEPTID,
                                    DEPTNAME = d.EMP.DEPTID != null ? d.EMP.DEPT.DEPTNAME : "N/A"
                                },
                                SEC = new F_BAS_HRD_SECTION
                                {
                                    SECID = d.EMP.SEC.SECID,
                                    SEC_NAME = d.EMP.SECID != null ? d.EMP.SEC.SEC_NAME : "N/A"
                                },
                                SUBSEC = new F_BAS_HRD_SUB_SECTION
                                {
                                    SUBSECID = d.EMP.SUBSEC.SUBSECID,
                                    SUBSEC_NAME = d.EMP.SUBSEC != null ? d.EMP.SUBSEC.SUBSEC_NAME : "N/A"
                                }
                            }
                        },
                        FGenSReqDetailsesList = d.F_GEN_S_REQ_DETAILS.Select(f => new F_GEN_S_REQ_DETAILS
                        {
                            PRODUCT = new F_GS_PRODUCT_INFORMATION
                            {
                                PRODID = f.PRODUCT.PRODID,
                                PRODNAME = $"{f.PRODUCT.PRODID} - {f.PRODUCT.PRODNAME} {(f.PRODUCT.PARTNO != "" ? " - " + f.PRODUCT.PARTNO : "")}"
                            }
                        }).ToList()
                    }).ToListAsync();
        }

        public async Task<IEnumerable<F_GEN_S_REQ_MASTER>> GetRequirementDD()
        {
            return await DenimDbContext.F_GEN_S_REQ_MASTER
                .Where(d => !DenimDbContext.F_GEN_S_ISSUE_MASTER.Any(f => f.GSRID.Equals(d.GSRID)))
                .Select(d => new F_GEN_S_REQ_MASTER
                {
                    GSRID = d.GSRID,
                    GSRNO = d.GSRNO
                }).ToListAsync();
        }

        public async Task<FGenSRequirementViewModel> GetInitObjByAsync(FGenSRequirementViewModel fGenSRequirementViewModel)
        {
            fGenSRequirementViewModel.ReqEmployees = await DenimDbContext.F_HRD_EMPLOYEE
                .Select(d => new F_HRD_EMPLOYEE
                {
                    EMPID = d.EMPID,
                    FIRST_NAME = $"{(d.EMPNO != null ? d.EMPNO + " -" : "")} {d.FIRST_NAME} {d.LAST_NAME}"
                }).OrderBy(d => d.FIRST_NAME).ToListAsync();

            fGenSRequirementViewModel.FGsProductInformationsList = await DenimDbContext.F_GS_PRODUCT_INFORMATION
                .Include(d => d.UNITNavigation)
                .Where(d=> (DenimDbContext.F_GEN_S_RECEIVE_DETAILS.Where(f=> f.PRODUCTID.Equals(d.PRODID)).Sum(f=>f.FRESH_QTY) ?? 0) - (DenimDbContext.F_GEN_S_ISSUE_DETAILS.Where(f => f.PRODUCTID.Equals(d.PRODID)).Sum(f => f.ISSUE_QTY) ?? 0) > 0)
                //.Where(d => _denimDbContext.F_GEN_S_TRANSECTION.Where(f => d.PRODID.Equals(f.PRODUCTID)).Sum(f => f.BALANCE) > 0)
                .Select(d => new F_GS_PRODUCT_INFORMATION
                {
                    PRODID = d.PRODID,
                    PRODNAME = $"{d.PRODID} - {d.PRODNAME} {(d.PARTNO != "" ? " - " + d.PARTNO : "")}"
                }).OrderBy(e => e.PRODNAME).ToListAsync();

            return fGenSRequirementViewModel;
        }

        
        public async Task<FGenSRequirementViewModel> FindByIdIncludeAllAsync(int gsrId, bool edit)
        {

            return await DenimDbContext.F_GEN_S_REQ_MASTER
                .Include(d => d.EMP)
                .Include(d => d.F_GEN_S_REQ_DETAILS)
                .ThenInclude(d => d.PRODUCT)
                .Where(d => d.GSRID.Equals(gsrId) && (!edit || !(DenimDbContext.F_GEN_S_ISSUE_MASTER.Any(f => f.GSRID.Equals(d.GSRID)) || d.CREATED_AT.Value.AddDays(2) <= DateTime.Now)))
                .Select(d => new FGenSRequirementViewModel
                {
                    FGenSReqMaster = new F_GEN_S_REQ_MASTER
                    {
                        GSRID = d.GSRID,
                        EncryptedId = _protector.Protect(d.GSRID.ToString()),
                        GSRNO = d.GSRNO,
                        GSRDATE = d.GSRDATE,
                        REQUISITIONBY = d.REQUISITIONBY,
                        REMARKS = d.REMARKS,
                        EMP = new F_HRD_EMPLOYEE
                        {
                            EMPID = d.EMP.EMPID,
                            FIRST_NAME = $"{(d.EMP.EMPNO != null ? d.EMP.EMPNO + " -" : "")} {d.EMP.FIRST_NAME} {d.EMP.LAST_NAME}"
                        }
                    },
                    FGenSReqDetailsesList = d.F_GEN_S_REQ_DETAILS
                        .Select(f => new F_GEN_S_REQ_DETAILS
                        {
                            GRQID = f.GRQID,
                            GSRID = f.GSRID,
                            PRODUCTID = f.PRODUCTID,
                            REQ_QTY = f.REQ_QTY,
                            STATUS = f.STATUS,
                            REMARKS = f.REMARKS,
                            PRODUCT = new F_GS_PRODUCT_INFORMATION()
                            {
                                PRODID = f.PRODUCT.PRODID,
                                PRODNAME = $"{f.PRODUCT.PRODID} - {f.PRODUCT.PRODNAME} {(f.PRODUCT.PARTNO != "" ? " - " + f.PRODUCT.PARTNO : "")}"
                            }
                        }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<F_GEN_S_REQ_MASTER>> GetAllFGenSRequirementAsync()
        {
            return await DenimDbContext.F_GEN_S_REQ_MASTER
                .Include(d => d.EMP)
                .Select(d => new F_GEN_S_REQ_MASTER
                {
                    GSRID = d.GSRID,
                    EncryptedId = _protector.Protect(d.GSRID.ToString()),
                    GSRNO = d.GSRNO,
                    GSRDATE = d.GSRDATE,
                    EMP = new F_HRD_EMPLOYEE
                    {
                        FIRST_NAME = $"{(d.EMP.EMPNO != null ? d.EMP.EMPNO+" -" : "")} {d.EMP.FIRST_NAME} {d.EMP.LAST_NAME}"
                    },
                    REMARKS = d.REMARKS,
                    IsLocked = DenimDbContext.F_GEN_S_ISSUE_MASTER.Any(f => f.GSRID.Equals(d.GSRID)) || d.CREATED_AT.Value.AddDays(2) <= DateTime.Now
                }).ToListAsync();
        }

        public async Task<FGenSRequirementViewModel> GetInitDetailsObjByAsync(FGenSRequirementViewModel fGenSRequirementViewModel)
        {
            foreach (var item in fGenSRequirementViewModel.FGenSReqDetailsesList)
            {
                item.PRODUCT = await DenimDbContext.F_GS_PRODUCT_INFORMATION
                    .Include(d => d.UNITNavigation)
                    .Select(d => new F_GS_PRODUCT_INFORMATION
                    {
                        PRODID = d.PRODID,
                        PRODNAME = $"{d.PRODID} - {d.PRODNAME} {(d.PARTNO != "" ? " - " + d.PARTNO : "")}",
                        UNITNavigation = new F_BAS_UNITS
                        {
                            UID = d.UNITNavigation.UID,
                            UNAME = d.UNITNavigation.UNAME
                        }
                    }).FirstOrDefaultAsync(e => e.PRODID.Equals(item.PRODUCTID));
            }

            return fGenSRequirementViewModel;
        }
    }
}
