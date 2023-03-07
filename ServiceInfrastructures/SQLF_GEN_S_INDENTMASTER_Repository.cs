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
    public class SQLF_GEN_S_INDENTMASTER_Repository : BaseRepository<F_GEN_S_INDENTMASTER>, IF_GEN_S_INDENTMASTER
    {
        private readonly IDataProtector _protector;

        public SQLF_GEN_S_INDENTMASTER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<FGenSRequisitionViewModel> GetInitObjByAsync(FGenSRequisitionViewModel fGenSRequisitionViewModel)
        {
            fGenSRequisitionViewModel.FGenSIndentTypes = await DenimDbContext.F_GEN_S_INDENT_TYPE
                .OrderBy(e => e.INDENTTYPE).ToListAsync();
            fGenSRequisitionViewModel.FGenSPurchaseRequisitionMastersList = await DenimDbContext.F_GEN_S_PURCHASE_REQUISITION_MASTER
                .Include(d => d.EMP)
                .Include(d => d.CN_PERSONNavigation)
                .Include(d => d.F_GEN_S_INDENTDETAILS)
                .ThenInclude(e => e.PRODUCT)
                .ThenInclude(d => d.UNITNavigation)
                .Select(d => new F_GEN_S_PURCHASE_REQUISITION_MASTER
                {
                    INDSLID = d.INDSLID,
                    INDSLDATE = d.INDSLDATE,
                    DEPTID = d.DEPTID,
                    SECID = d.SECID,
                    EMPID = d.EMPID,
                    CN_PERSON = d.CN_PERSON,
                    STATUS = d.STATUS,
                    REMARKS = d.REMARKS,
                    EMP = new F_HRD_EMPLOYEE
                    {
                        EMPID = d.EMP.EMPID,
                        FIRST_NAME = $"{(d.EMP.EMPNO != null ? d.EMP.EMPNO + " -" : "")} {d.EMP.FIRST_NAME} {d.EMP.LAST_NAME}"
                    },
                    CN_PERSONNavigation = new F_HRD_EMPLOYEE
                    {
                        EMPID = d.CN_PERSONNavigation.EMPID,
                        FIRST_NAME = $"{(d.CN_PERSONNavigation.EMPNO != null ? d.CN_PERSONNavigation.EMPNO + " -" : "")} {d.CN_PERSONNavigation.FIRST_NAME} {d.CN_PERSONNavigation.LAST_NAME}"
                    },
                    F_GEN_S_INDENTDETAILS = d.F_GEN_S_INDENTDETAILS
                })
                .Where(e => e.STATUS.Equals(false))
                .ToListAsync();

            ////TEMP For Testing

            //fGenSRequisitionViewModel.FGsProductInformationsList = await _denimDbContext.F_GS_PRODUCT_INFORMATION
            //    .Include(d => d.UNITNavigation)
            //    .Select(d => new F_GS_PRODUCT_INFORMATION
            //    {
            //        PRODID = d.PRODID,
            //        PRODNAME = d.PRODNAME,
            //        UNITNavigation = new F_BAS_UNITS
            //        {
            //            UID = d.UNITNavigation.UID,
            //            UNAME = d.UNITNavigation.UNAME
            //        }
            //    })
            //    .ToListAsync();

            return fGenSRequisitionViewModel;
        }

        public async Task<FGenSRequisitionViewModel> FindByIdIncludeAllAsync(int gindId)
        {
            try
            {
                var rs = await DenimDbContext.F_GEN_S_INDENTMASTER
                    .Include(d => d.INDSL.F_GEN_S_INDENTDETAILS)
                    .ThenInclude(d => d.PRODUCT)
                    .Include(e => e.F_GEN_S_INDENTDETAILS)
                    .ThenInclude(d => d.PRODUCT)
                    .Where(d => d.GINDID.Equals(gindId))
                    .Select(d => new FGenSRequisitionViewModel
                    {
                        FGenSIndentmaster = new F_GEN_S_INDENTMASTER
                        {
                            GINDID = d.GINDID,
                            EncryptedId = _protector.Protect(d.GINDID.ToString()),
                            GINDDATE = d.GINDDATE,
                            GINDNO = d.GINDNO,
                            INDSLID = d.INDSLID,
                            INDTYPE = d.INDTYPE,
                            REMARKS = d.REMARKS,
                            STATUS = d.STATUS,
                            INDTYPENavigation = new F_GEN_S_INDENT_TYPE
                            {
                                INDENTTYPE = d.INDTYPENavigation.INDENTTYPE
                            },
                            INDSL = new F_GEN_S_PURCHASE_REQUISITION_MASTER
                            {
                                INDSLID = d.INDSL.INDSLID,
                                F_GEN_S_INDENTDETAILS = d.INDSL.F_GEN_S_INDENTDETAILS.Select(f => new F_GEN_S_INDENTDETAILS
                                {
                                    PRODUCT = new F_GS_PRODUCT_INFORMATION
                                    {
                                        //PRODID = f.PRODUCT.PRODID,
                                        PRODNAME = $"{f.PRODUCT.PRODID} - {f.PRODUCT.PRODNAME} {(f.PRODUCT.PARTNO != "" ? " - " + f.PRODUCT.PARTNO : "")}"
                                    }
                                }).ToList()
                            }
                        },

                        FGenSIndentdetailsesList = d.F_GEN_S_INDENTDETAILS.Select(f => new F_GEN_S_INDENTDETAILS
                        {
                            TRNSID = f.TRNSID,
                            TRNSDATE = f.TRNSDATE,
                            GINDID = f.GINDID,
                            INDSLID = f.INDSLID,
                            PRODUCTID = f.PRODUCTID,
                            UNIT = f.UNIT,
                            QTY = f.QTY,
                            VALIDITY = f.VALIDITY,
                            FULL_QTY = f.FULL_QTY,
                            ADD_QTY = f.ADD_QTY,
                            BAL_QTY = f.BAL_QTY,
                            LOCATION = f.LOCATION,
                            REMARKS = f.REMARKS,
                            Expire = f.VALIDITY.HasValue ? f.VALIDITY.GetValueOrDefault().Subtract(DateTime.Now).Days >= 0 ? $"Remaining : {f.VALIDITY.GetValueOrDefault().Subtract(DateTime.Now).Days} days." : "Date Expired!!" : "",
                            PRODUCT = new F_GS_PRODUCT_INFORMATION
                            {
                                PRODID = f.PRODUCT.PRODID,
                                PRODNAME = $"{f.PRODUCT.PRODID} - {f.PRODUCT.PRODNAME} {(f.PRODUCT.PARTNO != "" ? " - " + f.PRODUCT.PARTNO : "")}"
                            }
                        }).ToList()
                    }).FirstOrDefaultAsync();

                return rs;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<F_GEN_S_INDENTMASTER>> GetAllGenSIndentAsync()
        {
            return await DenimDbContext.F_GEN_S_INDENTMASTER
                .Include(d => d.INDTYPENavigation)
                .Select(d => new F_GEN_S_INDENTMASTER
                {
                    GINDID = d.GINDID,
                    EncryptedId = _protector.Protect(d.GINDID.ToString()),
                    GINDDATE = d.GINDDATE,
                    GINDNO = d.GINDNO,
                    INDSLID = d.INDSLID,
                    INDTYPENavigation = new F_GEN_S_INDENT_TYPE
                    {
                        INDENTTYPE = d.INDTYPENavigation.INDENTTYPE
                    },
                    REMARKS = d.REMARKS,
                    STATUS = d.STATUS,
                    IsLocked = d.CREATED_AT < DateTime.Now.AddDays(-2)
                }).ToListAsync();
        }

        public async Task<int> GetLastGenSIndentNo()
        {
            var fGenSIndentmasters = DenimDbContext.F_GEN_S_INDENTMASTER.AsQueryable();
            return fGenSIndentmasters.Any() ? await fGenSIndentmasters.MaxAsync(d => d.GINDID) + 1 : 200000001;
        }

        public async Task<F_GEN_S_INDENTMASTER> GetIndentByIndId(int id)
        {
            return await DenimDbContext.F_GEN_S_INDENTMASTER
                .Where(d => d.GINDID.Equals(id))
                .Select(d => new F_GEN_S_INDENTMASTER
                {
                    GINDID = d.GINDID,
                    GINDDATE = d.GINDDATE
                }).FirstOrDefaultAsync();
        }

        public async Task<FGenSRequisitionViewModel> GetInitEditObjByAsync(FGenSRequisitionViewModel fGenSRequisitionViewModel)
        {
            fGenSRequisitionViewModel.FGenSIndentTypes = await DenimDbContext.F_GEN_S_INDENT_TYPE
                .OrderBy(e => e.INDENTTYPE).ToListAsync();
            fGenSRequisitionViewModel.FGenSPurchaseRequisitionMastersList = await DenimDbContext.F_GEN_S_PURCHASE_REQUISITION_MASTER
                .Include(d => d.EMP)
                .Include(d => d.CN_PERSONNavigation)
                .Include(d => d.F_GEN_S_INDENTDETAILS)
                .ThenInclude(e => e.PRODUCT)
                .ThenInclude(d => d.UNITNavigation)
                .Select(d => new F_GEN_S_PURCHASE_REQUISITION_MASTER
                {
                    INDSLID = d.INDSLID,
                    INDSLDATE = d.INDSLDATE,
                    DEPTID = d.DEPTID,
                    SECID = d.SECID,
                    EMPID = d.EMPID,
                    CN_PERSON = d.CN_PERSON,
                    STATUS = d.STATUS,
                    REMARKS = d.REMARKS,
                    EMP = new F_HRD_EMPLOYEE
                    {
                        EMPID = d.EMP.EMPID,
                        FIRST_NAME = $"{(d.EMP.EMPNO != null ? d.EMP.EMPNO + " -" : "")} {d.EMP.FIRST_NAME} {d.EMP.LAST_NAME}"
                    },
                    CN_PERSONNavigation = new F_HRD_EMPLOYEE
                    {
                        EMPID = d.CN_PERSONNavigation.EMPID,
                        FIRST_NAME = $"{(d.CN_PERSONNavigation.EMPNO != null ? d.CN_PERSONNavigation.EMPNO + " -" : "")} {d.CN_PERSONNavigation.FIRST_NAME} {d.CN_PERSONNavigation.LAST_NAME}"
                    },
                    F_GEN_S_INDENTDETAILS = d.F_GEN_S_INDENTDETAILS
                }).ToListAsync();

            return fGenSRequisitionViewModel;
        }
    }
}
