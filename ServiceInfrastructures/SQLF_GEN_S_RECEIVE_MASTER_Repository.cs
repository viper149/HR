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
    public class SQLF_GEN_S_RECEIVE_MASTER_Repository : BaseRepository<F_GEN_S_RECEIVE_MASTER>, IF_GEN_S_RECEIVE_MASTER
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;

        public SQLF_GEN_S_RECEIVE_MASTER_Repository(DenimDbContext denimDbContext,
        IHttpContextAccessor httpContextAccessor,
        UserManager<ApplicationUser> userManager,
        IDataProtectionProvider dataProtectionProvider,
        DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<FGenSReceiveViewModel> GetInitObjsByAsync(FGenSReceiveViewModel fGenSReceiveViewModel)
        {
            var ignores = new[]
            {
                "Recone(LCB)"
            };

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            var fHrEmployees = DenimDbContext.F_HRD_EMPLOYEE.Select(d => new F_HRD_EMPLOYEE
            {
                EMPID = d.EMPID,
                FIRST_NAME = $"{(d.EMPNO != null ? d.EMPNO + " -" : "")} {d.FIRST_NAME} {d.LAST_NAME}"
            });

            //fChemStoreReceiveViewModel.RcvEmployees = await fHrEmployees.OrderByDescending(e => e.EMPID.Equals(user.EMPID)).ThenBy(e => e.FIRST_NAME).ToListAsync();
            //fChemStoreReceiveViewModel.CheckEmployees = await fHrEmployees.OrderByDescending(e => e.EMPID.Equals(user.EMPID)).ThenBy(e => e.FIRST_NAME).ToListAsync();

            fGenSReceiveViewModel.CheckEmployees = await DenimDbContext.F_HRD_EMPLOYEE
                .Select(d => new F_HRD_EMPLOYEE
                {
                    EMPID = d.EMPID,
                    FIRST_NAME = $"{(d.EMPNO != null ? d.EMPNO + " -" : "")} {d.FIRST_NAME} {d.LAST_NAME}"
                }).OrderBy(e => e.FIRST_NAME).ToListAsync();

            fGenSReceiveViewModel.RcvEmployees = await DenimDbContext.F_HRD_EMPLOYEE
                .Select(d => new F_HRD_EMPLOYEE
                {
                    EMPID = d.EMPID,
                    FIRST_NAME = $"{(d.EMPNO != null ? d.EMPNO + " -" : "")} {d.FIRST_NAME} {d.LAST_NAME}"
                }).OrderBy(e => e.FIRST_NAME).ToListAsync();

            fGenSReceiveViewModel.CountriesList = await DenimDbContext.COUNTRIES
                .Select(e => new COUNTRIES
                {
                    ID = e.ID,
                    COUNTRY_NAME = e.COUNTRY_NAME
                }).OrderBy(e => e.COUNTRY_NAME).ToListAsync();

            fGenSReceiveViewModel.BasTransportinfos = await DenimDbContext.BAS_TRANSPORTINFO
                .Select(e => new BAS_TRANSPORTINFO
                {
                    TRNSPID = e.TRNSPID,
                    TRNSPNAME = $"{e.TRNSPNAME}, {e.CPERSON}"
                }).OrderBy(e => e.TRNSPNAME).ToListAsync();

            fGenSReceiveViewModel.BasSupplierinfos = await DenimDbContext.BAS_SUPPLIERINFO
                .Select(e => new BAS_SUPPLIERINFO
                {
                    SUPPID = e.SUPPID,
                    SUPPNAME = e.SUPPNAME
                }).OrderBy(e => e.SUPPNAME).ToListAsync();

            fGenSReceiveViewModel.ComImpLcdetailsesList = await DenimDbContext.COM_IMP_LCDETAILS
                .Select(d => new COM_IMP_LCDETAILS()
                {
                    TRNSID = d.TRNSID,
                    PINO = d.PINO,
                }).ToListAsync();

            fGenSReceiveViewModel.FBasReceiveTypesList = await DenimDbContext.F_BAS_RECEIVE_TYPE
                .Where(d => !ignores.Any(f => f.ToLower().Contains(d.RCVTYPE.ToLower())))
                .Select(d => new F_BAS_RECEIVE_TYPE
                {
                    RCVTID = d.RCVTID,
                    RCVTYPE = d.RCVTYPE
                }).OrderByDescending(d => d.RCVTYPE).ToListAsync();

            fGenSReceiveViewModel.ComImpInvoiceInfoList = await DenimDbContext.COM_IMP_INVOICEINFO
                .Include(e => e.LC)
                .Select(e => new COM_IMP_INVOICEINFO
                {
                    INVID = e.INVID,
                    INVNO = $"{e.INVNO}, LC: {e.LC.LCNO}"
                }).OrderBy(e => e.INVNO).ToListAsync();

            fGenSReceiveViewModel.ComImpCnfinfosList = await DenimDbContext.COM_IMP_CNFINFO
                .Select(e => new COM_IMP_CNFINFO
                {
                    CNFID = e.CNFID,
                    CNFNAME = $"{e.CNFNAME}, {e.C_PERSON}"
                }).ToListAsync();

            fGenSReceiveViewModel.FGsProductInformationsList = await DenimDbContext.F_GS_PRODUCT_INFORMATION
                .Where(d => DenimDbContext.F_GEN_S_INDENTDETAILS.Any(f => f.PRODUCTID.Equals(d.PRODID)))
                .Select(d => new F_GS_PRODUCT_INFORMATION
                {
                    PRODID = d.PRODID,
                    PRODNAME = $"{d.PRODID} - {d.PRODNAME} {(d.PARTNO != "" ? " - " + d.PARTNO : "")}"
                })
                .ToListAsync();

            fGenSReceiveViewModel.FGenSIndentdetailsesList = await DenimDbContext.F_GEN_S_INDENTDETAILS
                .Include(d => d.PRODUCT)
                .Select(d => new F_GEN_S_INDENTDETAILS
                {
                    PRODUCT = new F_GS_PRODUCT_INFORMATION
                    {
                        PRODID = d.PRODUCT.PRODID,
                        PRODNAME = $"{d.PRODUCT.PRODID} - {d.PRODUCT.PRODNAME} {(d.PRODUCT.PARTNO != "" ? " - " + d.PRODUCT.PARTNO : "")}"
                    }
                }).Distinct().ToListAsync();

            //fGenSReceiveViewModel.FGenSIndentmastersList = await _denimDbContext.F_GEN_S_INDENTMASTER
            //    .Select(d => new F_GEN_S_INDENTMASTER
            //    {
            //        GINDID = d.GINDID,
            //        GINDNO = d.GINDNO
            //    }).OrderBy(d => d.GINDNO).ToListAsync();

            return fGenSReceiveViewModel;
        }

        public async Task<FGenSReceiveViewModel> FindByIdIncludeAllAsync(int gsRcvId, bool edit = false)
        {
            try
            {
                var x = await DenimDbContext.F_GEN_S_RECEIVE_MASTER
                    .Include(d=>d.RCVBYNavigation)
                    .Include(d=>d.CHECKBYNavigation)
                    .Include(d=>d.LCD.LC)
                    .Include(d=>d.SUPP)
                    .Include(d=>d.ORIGINNavigation)
                    .Include(d=>d.TRANSP)
                    .Include(d=>d.QCPASSNavigation)
                    .Include(d=>d.MRRNavigation)
                    .Include(d=>d.RCVT)
                    .Include(d => d.F_GEN_S_RECEIVE_DETAILS)
                    .ThenInclude(d => d.GIND)
                    .Include(d => d.F_GEN_S_RECEIVE_DETAILS)
                    .ThenInclude(d => d.PRODUCT.UNITNavigation)
                    .Where(d => d.GRCVID.Equals(gsRcvId) &&  (!edit || d.MRR == null || d.QCPASS == null))
                    .Select(d => new FGenSReceiveViewModel
                    {
                        FGenSReceiveMaster = new F_GEN_S_RECEIVE_MASTER
                        {
                            GRCVID = d.GRCVID,
                            EncryptedId = _protector.Protect(d.GRCVID.ToString()),
                            RCVDATE = d.RCVDATE,
                            RCVTID = d.RCVTID,
                            LC_ID = d.LC_ID,
                            SUPPID = d.SUPPID,
                            ORIGIN = d.ORIGIN,
                            CHALLAN_NO = d.CHALLAN_NO,
                            CHALLAN_DATE = d.CHALLAN_DATE,
                            TRANSPID = d.TRANSPID,
                            VEHICAL_NO = d.VEHICAL_NO,
                            GE_ID = d.GE_ID,
                            GEDATE = d.GEDATE,
                            REMARKS = d.REMARKS,
                            QCPASS = d.QCPASS,
                            MRR = d.MRR,
                            RCVBY = d.RCVBY,
                            CHECKBY = d.CHECKBY,
                            QCINFO = d.QCPASS != null ? d.QCPASSNavigation.GSQCANO.ToString() : "QC not Approved Yet",
                            MRRINFO = d.MRR != null ? d.MRRNavigation.GSMRRNO.ToString() : "MRR not Created Yet",
                            RCVT = new F_BAS_RECEIVE_TYPE
                            {
                                RCVTYPE = d.RCVT.RCVTYPE
                            },
                            RCVBYNavigation = new F_HRD_EMPLOYEE
                            {
                                FIRST_NAME = $"{(d.RCVBYNavigation.EMPNO != null ? d.RCVBYNavigation.EMPNO + " -" : "")} {d.RCVBYNavigation.FIRST_NAME} {d.RCVBYNavigation.LAST_NAME}"
                            },
                            CHECKBYNavigation = new F_HRD_EMPLOYEE
                            {
                                FIRST_NAME = $"{(d.CHECKBYNavigation.EMPNO != null ? d.CHECKBYNavigation.EMPNO + " -" : "")} {d.CHECKBYNavigation.FIRST_NAME} {d.CHECKBYNavigation.LAST_NAME}"
                            },
                            LCD = new COM_IMP_LCDETAILS
                            {
                                PINO = d.LCD.PINO,
                                LC = new COM_IMP_LCINFORMATION
                                {
                                    LCNO = d.LCD.LC.LCNO
                                }
                            },
                            SUPP = new BAS_SUPPLIERINFO
                            {
                                SUPPNAME = d.SUPP.SUPPNAME
                            },
                            ORIGINNavigation = new COUNTRIES
                            {
                                COUNTRY_NAME = d.ORIGINNavigation.COUNTRY_NAME
                            },
                            TRANSP = new BAS_TRANSPORTINFO
                            {
                                TRNSPNAME = d.TRANSP.TRNSPNAME
                            }
                        },
                        FGenSReceiveDetailsesList = d.F_GEN_S_RECEIVE_DETAILS
                            .Select(f => new F_GEN_S_RECEIVE_DETAILS
                            {
                                TRNSID = f.TRNSID,
                                TRNSDATE = f.TRNSDATE,
                                GRCVID = f.GRCVID,
                                PRODUCTID = f.PRODUCTID,
                                ADJUSTED_WITH = f.ADJUSTED_WITH,
                                UNIT = f.UNIT,
                                GINDID = f.GINDID,
                                INVQTY = f.INVQTY,
                                RATE = f.RATE,
                                CURRENCY = f.CURRENCY,
                                AMOUNT = f.AMOUNT,
                                BATCHNO = f.BATCHNO,
                                MNGDATE = f.MNGDATE,
                                EXDATE = f.EXDATE,
                                REMARKS = f.REMARKS,
                                FRESH_QTY = f.FRESH_QTY,
                                REJ_QTY = f.REJ_QTY,
                                GIND = new F_GEN_S_INDENTMASTER
                                {
                                    GINDNO = f.GIND.GINDNO
                                },
                                PRODUCT = new F_GS_PRODUCT_INFORMATION
                                {
                                    PRODNAME = $"{f.PRODUCT.PRODID} - {f.PRODUCT.PRODNAME} {(f.PRODUCT.PARTNO != "" ? " - " + f.PRODUCT.PARTNO : "")}",
                                    UNITNavigation = new F_BAS_UNITS
                                    {
                                        UNAME = f.PRODUCT.UNITNavigation.UNAME
                                    }
                                }
                            }).ToList()
                    }).FirstOrDefaultAsync();
                return x;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<F_GEN_S_RECEIVE_MASTER>> GetAllFGenSReceiveAsync()
        {
            return await DenimDbContext.F_GEN_S_RECEIVE_MASTER
                .Include(d => d.CNF)
                .Include(d => d.RCVT)
                .Include(d => d.QCPASSNavigation)
                .Include(d => d.MRRNavigation)
                .Select(d => new F_GEN_S_RECEIVE_MASTER
                {
                    GRCVID = d.GRCVID,
                    EncryptedId = _protector.Protect(d.GRCVID.ToString()),
                    RCVDATE = d.RCVDATE,
                    GINVID = d.GINVID,
                    CHALLAN_NO = d.CHALLAN_NO,
                    CHALLAN_DATE = d.CHALLAN_DATE,
                    VEHICAL_NO = d.VEHICAL_NO,
                    QCPASS = d.QCPASS,
                    MRR = d.MRR,
                    //IsLocked = d.CREATED_AT < DateTime.Now.AddDays(-2),
                    IsLocked = d.QCPASS != null && d.MRR != null,
                    RCVT = new F_BAS_RECEIVE_TYPE
                    {
                        RCVTYPE = d.RCVT.RCVTYPE
                    },
                    CNF = new COM_IMP_CNFINFO
                    {
                        CNFNAME = d.CNF.CNFNAME
                    },
                    QCPASSNavigation = new F_GEN_S_QC_APPROVE
                    {
                        GSQCANO = d.QCPASSNavigation.GSQCANO
                    },
                    MRRNavigation = new F_GEN_S_MRR
                    {
                        GSMRRNO = d.MRRNavigation.GSMRRNO
                    }
                }).ToListAsync();
        }
    }
}
