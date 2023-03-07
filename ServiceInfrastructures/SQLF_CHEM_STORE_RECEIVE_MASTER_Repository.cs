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
    public class SQLF_CHEM_STORE_RECEIVE_MASTER_Repository : BaseRepository<F_CHEM_STORE_RECEIVE_MASTER>, IF_CHEM_STORE_RECEIVE_MASTER
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;

        public SQLF_CHEM_STORE_RECEIVE_MASTER_Repository(DenimDbContext denimDbContext,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }


        public async Task<DataTableObject<F_CHEM_STORE_RECEIVE_MASTER>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip,
            int pageSize)
        {
            var navigationPropertyStrings = new[] { "RCVT", "CNF", "ComImpInvoiceinfo" };

            var fChemStoreReceiveMasters = DenimDbContext.F_CHEM_STORE_RECEIVE_MASTER
                .Include(e => e.ComImpInvoiceinfo)
                .Include(e => e.CNF)
                .Include(e => e.RCVT)
                .Select(e => new F_CHEM_STORE_RECEIVE_MASTER
                {
                    CHEMRCVID = e.CHEMRCVID,
                    EncryptedId = _protector.Protect(e.CHEMRCVID.ToString()),
                    RCVDATE = e.RCVDATE,
                    RCVT = new F_BAS_RECEIVE_TYPE
                    {
                        RCVTYPE = e.RCVT.RCVTYPE
                    },
                    ComImpInvoiceinfo = new COM_IMP_INVOICEINFO
                    {
                        INVNO = e.ComImpInvoiceinfo.INVNO
                    },
                    CHALLAN_NO = e.CHALLAN_NO,
                    CHALLAN_DATE = e.CHALLAN_DATE,
                    CNF = new COM_IMP_CNFINFO
                    {
                        CNFNAME = e.CNF.CNFNAME
                    },
                    VEHICAL_NO = e.VEHICAL_NO,
                    REMARKS = e.REMARKS,
                    //IsLocked = e.CREATED_AT < DateTime.Now.AddDays(-2),
                    IsLocked = false
                });


            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                fChemStoreReceiveMasters = OrderedResult<F_CHEM_STORE_RECEIVE_MASTER>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, fChemStoreReceiveMasters);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                fChemStoreReceiveMasters = fChemStoreReceiveMasters
                    .Where(m => m.CHEMRCVID.ToString().ToUpper().Contains(searchValue)
                    || m.RCVDATE.ToString().ToUpper().Contains(searchValue)
                    || m.RCVT.RCVTYPE != null && m.RCVT.RCVTYPE.ToUpper().Contains(searchValue)
                    || m.ComImpInvoiceinfo.INVNO != null && m.ComImpInvoiceinfo.INVNO.ToUpper().Contains(searchValue)
                    || m.CHALLAN_NO != null && m.CHALLAN_NO.Contains(searchValue)
                    || m.CHALLAN_DATE != null && m.CHALLAN_DATE.ToString().Contains(searchValue)
                    || m.CNF.CNFNAME != null && m.CNF.CNFNAME.Contains(searchValue)
                    || m.VEHICAL_NO.ToUpper().Contains(searchValue)
                    || m.REMARKS.ToUpper().Contains(searchValue));

                fChemStoreReceiveMasters = OrderedResult<F_CHEM_STORE_RECEIVE_MASTER>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, fChemStoreReceiveMasters);
            }

            var recordsTotal = await fChemStoreReceiveMasters.CountAsync();
            return new DataTableObject<F_CHEM_STORE_RECEIVE_MASTER>(draw, recordsTotal, recordsTotal, await fChemStoreReceiveMasters.Skip(skip).Take(pageSize).ToListAsync());
        }

        public async Task<IEnumerable<F_CHEM_STORE_RECEIVE_MASTER>> GetAllChemicalReceiveAsync()
        {
            return await DenimDbContext.F_CHEM_STORE_RECEIVE_MASTER
                    .Include(e => e.ComImpInvoiceinfo)
                    .Include(e => e.CNF)
                    .Include(e => e.RCVT)
                    .Select(e => new F_CHEM_STORE_RECEIVE_MASTER
                    {
                        CHEMRCVID = e.CHEMRCVID,
                        EncryptedId = _protector.Protect(e.CHEMRCVID.ToString()),
                        RCVDATE = e.RCVDATE,
                        RCVT = new F_BAS_RECEIVE_TYPE
                        {
                            RCVTYPE = e.RCVT.RCVTYPE
                        },
                        ComImpInvoiceinfo = new COM_IMP_INVOICEINFO
                        {
                            INVNO = e.ComImpInvoiceinfo.INVNO
                        },
                        CHALLAN_NO = e.CHALLAN_NO,
                        CHALLAN_DATE = e.CHALLAN_DATE,
                        CNF = new COM_IMP_CNFINFO
                        {
                            CNFNAME = e.CNF.CNFNAME
                        },
                        VEHICAL_NO = e.VEHICAL_NO,
                        REMARKS = e.REMARKS,
                        //IsLocked = e.CREATED_AT < DateTime.Now.AddDays(-2)
                        IsLocked = false
                    }).ToListAsync();
        }

        public async Task<FChemStoreReceiveViewModel> GetInitObjsByAsync(FChemStoreReceiveViewModel fChemStoreReceiveViewModel)
        {
            var ignores = new[]
            {
                "inspection",
                "Recone(LCB)"
            };

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            var fHrEmployees = DenimDbContext.F_HRD_EMPLOYEE.Select(e => new F_HRD_EMPLOYEE
            {
                EMPID = e.EMPID,
                FIRST_NAME = $"{e.EMPNO}, {e.FIRST_NAME} {e.LAST_NAME}"
            });

            fChemStoreReceiveViewModel.RcvEmployees = await fHrEmployees.OrderByDescending(e => e.EMPID.Equals(user.EMPID)).ThenBy(e => e.FIRST_NAME).ToListAsync();
            fChemStoreReceiveViewModel.CheckEmployees = await fHrEmployees.OrderByDescending(e => e.EMPID.Equals(user.EMPID)).ThenBy(e => e.FIRST_NAME).ToListAsync();

            fChemStoreReceiveViewModel.Countrieses = await DenimDbContext.COUNTRIES.Select(e => new COUNTRIES
            {
                ID = e.ID,
                COUNTRY_NAME = e.COUNTRY_NAME
            }).OrderBy(e => e.COUNTRY_NAME).ToListAsync();

            fChemStoreReceiveViewModel.BasTransportinfos = await DenimDbContext.BAS_TRANSPORTINFO.Select(e => new BAS_TRANSPORTINFO
            {
                TRNSPID = e.TRNSPID,
                TRNSPNAME = $"{e.TRNSPNAME}, {e.CPERSON}"
            }).OrderBy(e => e.TRNSPNAME).ToListAsync();

            fChemStoreReceiveViewModel.BasSupplierinfos = await DenimDbContext.BAS_SUPPLIERINFO.Select(e => new BAS_SUPPLIERINFO
            {
                SUPPID = e.SUPPID,
                SUPPNAME = e.SUPPNAME
            }).OrderBy(e => e.SUPPNAME).ToListAsync();

            fChemStoreReceiveViewModel.ComImpLcinformations = await DenimDbContext.COM_IMP_LCINFORMATION
                .Include(e => e.SUPP)
                .Select(e => new COM_IMP_LCINFORMATION
                {
                    LC_ID = e.LC_ID,
                    LCNO = $"{e.LCNO}, Supplier: {e.SUPP.SUPPNAME}"
                }).OrderBy(e => e.LCNO).ToListAsync();

            fChemStoreReceiveViewModel.FBasReceiveTypesList = await DenimDbContext.F_BAS_RECEIVE_TYPE
                .Where(e => !ignores.Any(f => f.ToLower().Contains(e.RCVTYPE.ToLower())))
                .Select(e => new F_BAS_RECEIVE_TYPE
                {
                    RCVTID = e.RCVTID,
                    RCVTYPE = e.RCVTYPE
                }).OrderByDescending(e => e.RCVTYPE).ToListAsync();

            fChemStoreReceiveViewModel.ComImpInvoiceInfoList = await DenimDbContext.COM_IMP_INVOICEINFO
                .Include(e => e.LC)
                .Select(e => new COM_IMP_INVOICEINFO
                {
                    INVID = e.INVID,
                    INVNO = $"{e.INVNO}, LC: {e.LC.LCNO}"
                }).OrderBy(e => e.INVNO).ToListAsync();

            fChemStoreReceiveViewModel.ComImpCnfinfosList = await DenimDbContext.COM_IMP_CNFINFO
                .Select(e => new COM_IMP_CNFINFO
                {
                    CNFID = e.CNFID,
                    CNFNAME = $"{e.CNFNAME}, {e.C_PERSON}"
                }).ToListAsync();

            fChemStoreReceiveViewModel.FChemStoreProductinfosList = await DenimDbContext.F_CHEM_STORE_PRODUCTINFO
                .Select(e => new F_CHEM_STORE_PRODUCTINFO
                {
                    PRODUCTID = e.PRODUCTID,
                    PRODUCTNAME = e.PRODUCTNAME
                }).OrderBy(e => e.PRODUCTNAME).ToListAsync();

            fChemStoreReceiveViewModel.FChemStoreIndentmastersList = await DenimDbContext.F_CHEM_STORE_INDENTMASTER
                .Include(e => e.INDSL.FBasSubsection)
                .Select(e => new F_CHEM_STORE_INDENTMASTER
                {
                    CINDID = e.CINDID,
                    CINDNO = $"{e.CINDNO}, Sub-Section: {e.INDSL.FBasSubsection.SSECNAME}"
                }).OrderBy(e => e.CINDNO).ToListAsync();

            return fChemStoreReceiveViewModel;
        }

        public async Task<FChemStoreReceiveViewModel> FindByIdIncludeAllAsync(int chemRcvId)
        {
            return await DenimDbContext.F_CHEM_STORE_RECEIVE_MASTER
                .Where(e => e.CHEMRCVID.Equals(chemRcvId))
                .Include(e => e.F_CHEM_STORE_RECEIVE_DETAILS)
                .ThenInclude(e => e.FChemStoreProductinfo)
                .Include(e => e.F_CHEM_STORE_RECEIVE_DETAILS)
                .ThenInclude(e => e.FBasUnits)
                .Select(e => new FChemStoreReceiveViewModel
                {
                    FChemStoreReceiveMaster = new F_CHEM_STORE_RECEIVE_MASTER
                    {
                        CHEMRCVID = e.CHEMRCVID,
                        EncryptedId = _protector.Protect(e.CHEMRCVID.ToString()),
                        RCVDATE = e.RCVDATE,
                        RCVTID = e.RCVTID,
                        RCVBY = e.RCVBY,
                        CHECKBY = e.CHECKBY,
                        INVID = e.INVID,
                        LC_ID = e.LC_ID,
                        SUPPID = e.SUPPID,
                        ORIGIN = e.ORIGIN,
                        CHALLAN_NO = e.CHALLAN_NO,
                        CHALLAN_DATE = e.CHALLAN_DATE,
                        CNF_CHALLAN_NO = e.CNF_CHALLAN_NO,
                        CNF_CHALLAN_DATE = e.CNF_CHALLAN_DATE,
                        CNFID = e.CNFID,
                        TRANSPID = e.TRANSPID,
                        VEHICAL_NO = e.VEHICAL_NO,
                        GE_ID = e.GE_ID,
                        GEDATE = e.GEDATE,
                        REMARKS = e.REMARKS,
                        OPT1 = e.OPT1,
                        OPT2 = e.OPT2,
                        OPT3 = e.OPT3
                    },
                    FChemStoreReceiveDetailsList = e.F_CHEM_STORE_RECEIVE_DETAILS.Select(f => new F_CHEM_STORE_RECEIVE_DETAILS
                    {
                        TRNSID = f.TRNSID,
                        TRNSDATE = f.TRNSDATE,
                        CHEMRCVID = f.CHEMRCVID,
                        PRODUCTID = f.PRODUCTID,
                        ADJUSTED_WITH = f.ADJUSTED_WITH,
                        QC_APPROVE = f.QC_APPROVE,
                        MRR_CREATE = f.MRR_CREATE,
                        UNIT = f.UNIT,
                        CINDID = f.CINDID,
                        CINDDATE = f.CINDDATE,
                        INVQTY = f.INVQTY,
                        RATE = f.RATE,
                        CURRENCY = f.CURRENCY,
                        AMOUNT = f.AMOUNT,
                        BATCHNO = f.BATCHNO,
                        MNGDATE = f.MNGDATE,
                        EXDATE = f.EXDATE,
                        REMARKS = f.REMARKS,
                        ISQC = f.ISQC,
                        MRRNO = f.MRRNO,
                        FRESH_QTY = f.FRESH_QTY,
                        REJ_QTY = f.REJ_QTY,
                        MRRDATE = f.MRRDATE,
                        OPT1 = f.OPT1,
                        OPT2 = f.OPT2,
                        FChemStoreProductinfo = f.FChemStoreProductinfo,
                        FBasUnits = f.FBasUnits
                    }).ToList()
                }).FirstOrDefaultAsync();
        }
    }
}
