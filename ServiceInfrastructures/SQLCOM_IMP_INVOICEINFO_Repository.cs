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
using DenimERP.ViewModels.Com.InvoiceImport;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOM_IMP_INVOICEINFO_Repository : BaseRepository<COM_IMP_INVOICEINFO>, ICOM_IMP_INVOICEINFO
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IDataProtector _protector;

        public SQLCOM_IMP_INVOICEINFO_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
            : base(denimDbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<bool> FindByInvNoAsync(string invNo)
        {
            try
            {
                return await DenimDbContext.COM_IMP_INVOICEINFO.AnyAsync(e => e.INVNO.Equals(invNo));
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<COM_IMP_INVOICEINFO> FindByIdIncludeAllAsync(int id)
        {
            try
            {
                var result = await DenimDbContext.COM_IMP_INVOICEINFO.Where(e => e.INVID.Equals(id))
                    .Include(e => e.TRNSP)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ComImpInvoiceInfoCreateViewModel> GetInitObjByAsync(ComImpInvoiceInfoCreateViewModel comImpInvoiceInfoCreateViewModel)
        {
            comImpInvoiceInfoCreateViewModel.ComImpLcinformations = await DenimDbContext.COM_IMP_LCINFORMATION
                .Include(e => e.CAT)
                .Select(e => new COM_IMP_LCINFORMATION
                {
                    LC_ID = e.LC_ID,
                    LCNO = $"{e.LCNO}, LC type: {e.CAT.CATEGORY}",
                    LCDATE = e.LCDATE
                }).OrderBy(e => e.LCDATE).ToListAsync();

            comImpInvoiceInfoCreateViewModel.ComImpDelStatuses = await DenimDbContext.COM_IMP_DEL_STATUS.ToListAsync();
            comImpInvoiceInfoCreateViewModel.ComImpInvoiceinfo.INVDATE = DateTime.Now;
            comImpInvoiceInfoCreateViewModel.BasTransportinfos = await DenimDbContext.BAS_TRANSPORTINFO.ToListAsync();
            //comImpInvoiceInfoCreateViewModel.BasProductinfos = await _denimDbContext.BAS_PRODUCTINFO.ToListAsync();
            comImpInvoiceInfoCreateViewModel.ComImpInvdetails.TRNSDATE = DateTime.Now;
            comImpInvoiceInfoCreateViewModel.ComImpInvdetails.QTY = 0;
            comImpInvoiceInfoCreateViewModel.ComImpInvdetails.RATE = 0;
            comImpInvoiceInfoCreateViewModel.ComImpInvdetails.AMOUNT = 0;

            comImpInvoiceInfoCreateViewModel.BasYarnLotinfos = await DenimDbContext.BAS_YARN_LOTINFO
                .Select(e => new BAS_YARN_LOTINFO
                {
                    LOTID = e.LOTID,
                    LOTNO = e.LOTNO
                }).OrderBy(e => e.LOTNO).ToListAsync();

            comImpInvoiceInfoCreateViewModel.FChemStoreProductinfos = await DenimDbContext.F_CHEM_STORE_PRODUCTINFO
                .Select(e => new F_CHEM_STORE_PRODUCTINFO
                {
                    PRODUCTID = e.PRODUCTID,
                    PRODUCTNAME = e.PRODUCTNAME
                }).OrderBy(e => e.PRODUCTNAME).ToListAsync();

            comImpInvoiceInfoCreateViewModel.FBasUnitses = await DenimDbContext.F_BAS_UNITS.Select(e => new F_BAS_UNITS
            {
                UID = e.UID,
                UNAME = e.UNAME
            }).OrderByDescending(e => e.UNAME.ToLower().Contains("kg")).ThenBy(e => e.UNAME).ToListAsync();

            comImpInvoiceInfoCreateViewModel.ComContainers = await DenimDbContext.COM_CONTAINER
                .OrderBy(e => e.CONTAINERSIZE)
                .ToListAsync();


            comImpInvoiceInfoCreateViewModel.CnFList = await DenimDbContext.COM_IMP_CNFINFO
                .Select(e => new COM_IMP_CNFINFO
                {
                    CNFID = e.CNFID,
                    CNFNAME = e.CNFNAME
                }).OrderBy(e => e.CNFNAME).ToListAsync();

            return comImpInvoiceInfoCreateViewModel;
        }

        public async Task<DataTableObject<COM_IMP_INVOICEINFO>> GetForDataTableByAsync(string sortColumn,
            string sortColumnDirection, string searchValue, string draw, int skip, int pageSize)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var roles = await _signInManager.UserManager.GetRolesAsync(user);
            var x = roles.Any(c => c.Contains("Super Admin") || c.Contains("Admin") || c.Contains("Com Imp"));
            var navigationPropertyStrings = new[] { "" };
            var comImpInvoiceinfos = DenimDbContext.COM_IMP_INVOICEINFO
                .Select(e => new COM_IMP_INVOICEINFO
                {
                    INVID = e.INVID,
                    EncryptedId = _protector.Protect(e.INVID.ToString()),
                    INVNO = e.INVNO,
                    INVDATE = e.INVDATE,
                    LPORT = e.LPORT,
                    DOCHANDSON = e.DOCHANDSON,
                    ETADATE = e.ETADATE,
                    INVPATH = e.INVPATH,
                    BLPATH = e.BLPATH,
                    REMARKS = e.REMARKS,
                    IsLocked = roles.Any(c=>c.Contains("Super Admin") || c.Contains("Admin")||c.Contains("Com Imp"))
                });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                comImpInvoiceinfos = OrderedResult<COM_IMP_INVOICEINFO>.GetOrderedResult(sortColumnDirection,
                    sortColumn, navigationPropertyStrings, comImpInvoiceinfos);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                comImpInvoiceinfos = comImpInvoiceinfos
                    .Where(m => m.INVNO.ToUpper().Contains(searchValue)
                                || m.INVDATE.ToShortDateString().ToUpper().Contains(searchValue)
                                || m.LPORT != null && m.LPORT.Contains(searchValue)
                                || m.DOCHANDSON != null && m.DOCHANDSON.ToString().ToUpper().Contains(searchValue)
                                || m.ETADATE != null && m.ETADATE.ToString().ToUpper().Contains(searchValue)
                                || m.INVPATH != null && m.INVPATH.ToUpper().Contains(searchValue)
                                || m.BLPATH != null && m.BLPATH.ToUpper().Contains(searchValue)
                                || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue));

                comImpInvoiceinfos = OrderedResult<COM_IMP_INVOICEINFO>.GetOrderedResult(sortColumnDirection,
                    sortColumn, navigationPropertyStrings, comImpInvoiceinfos);
            }

            var recordsTotal = await comImpInvoiceinfos.CountAsync();

            return new DataTableObject<COM_IMP_INVOICEINFO>(draw, recordsTotal, recordsTotal,
                await comImpInvoiceinfos.Skip(skip).Take(pageSize).ToListAsync());
        }

        public async Task<ComImpInvoiceInfoDetailsViewModel> FindBInvIdIncludeAllAsync(int invId)
        {
            var comImpInvoiceInfoDetailsViewModel = await DenimDbContext.COM_IMP_INVOICEINFO
                .Include(e => e.TRNSP)
                .Include(e => e.DelStatus)
                .Include(e => e.ComImpInvdetailses)
                .ThenInclude(e => e.F_CHEM_STORE_PRODUCTINFOS)
                .Include(e => e.ComImpInvdetailses)
                .ThenInclude(e => e.BAS_YARN_LOTINFOS)
                .Include(e => e.ComImpInvdetailses)
                .ThenInclude(e => e.BasProductinfo)
                .Where(e => e.INVID.Equals(invId))
                .Select(e => new ComImpInvoiceInfoDetailsViewModel
                {
                    ComImpInvoiceinfo = new COM_IMP_INVOICEINFO
                    {
                        INVID = e.INVID,
                        EncryptedId = _protector.Protect(e.INVID.ToString()),
                        INVNO = e.INVNO,
                        INVDATE = e.INVDATE,
                        INVPATH = e.INVPATH,
                        LC_ID = e.LC_ID,
                        LPORT = e.LPORT,
                        DOCHANDSON = e.DOCHANDSON,
                        ETADATE = e.ETADATE,
                        DEL_STATUS = e.DEL_STATUS,
                        DEL_DATE = e.DEL_DATE,
                        CONTQTY = e.CONTQTY,
                        CONTSIZE = e.CONTSIZE,
                        SBDATE = e.SBDATE,
                        BLNO = e.BLNO,
                        BLDATE = e.BLDATE,
                        BLPATH = e.BLPATH,
                        BENTRYNO = e.BENTRYNO,
                        BENTRYDATE = e.BENTRYDATE,
                        MPNO = e.MPNO,
                        MPDATE = e.MPDATE,
                        MPSUB_DATE = e.MPSUB_DATE,
                        MPBILL = e.MPBILL,
                        RCVSTATUS = e.RCVSTATUS,
                        MRRNO = e.MRRNO,
                        MRRDATE = e.MRRDATE,
                        PAYMENTDATE = e.PAYMENTDATE,
                        CnFBILL = e.CnFBILL,
                        CnFBILLDATE = e.CnFBILLDATE,
                        TRNSPID = e.TRNSPID,
                        TRUCKQTY = e.TRUCKQTY,
                        TRNSPBILL = e.TRNSPBILL,
                        TRNSPBILLDATE = e.TRNSPBILLDATE,
                        TRNS_BILL_SUB_DATE = e.TRNS_BILL_SUB_DATE,
                        SHIPBY = e.SHIPBY,
                        STATUS = e.STATUS,
                        REMARKS = e.REMARKS,
                        USRID = e.USRID,
                        DelStatus = e.DelStatus,
                        COM_CONTAINER = e.COM_CONTAINER,
                        TRNSP = e.TRNSP
                    },
                    ComImpInvdetailses = e.ComImpInvdetailses.Select(f => new COM_IMP_INVDETAILS
                    {
                        TRNSID = f.TRNSID,
                        TRNSDATE = f.TRNSDATE,
                        INVID = f.INVID,
                        INVNO = f.INVNO,
                        PRODID = f.PRODID,
                        CHEMPRODID = f.CHEMPRODID,
                        YARNLOTID = f.YARNLOTID,
                        UNIT = f.UNIT,
                        QTY = f.QTY,
                        RATE = f.RATE,
                        AMOUNT = f.AMOUNT,
                        REMARKS = f.REMARKS,
                        USRID = f.USRID,
                        BasProductinfo = new BAS_PRODUCTINFO
                        {
                            PRODID = (f.BasProductinfo != null ? f.BasProductinfo.PRODID : 0),
                            PRODNAME = (f.BasProductinfo != null ? f.BasProductinfo.PRODNAME : null)
                        },
                        BAS_YARN_LOTINFOS = new BAS_YARN_LOTINFO
                        {
                            LOTID = (f.BAS_YARN_LOTINFOS != null ? f.BAS_YARN_LOTINFOS.LOTID : 0),
                            LOTNO = (f.BAS_YARN_LOTINFOS != null ? f.BAS_YARN_LOTINFOS.LOTNO : null)
                        },
                        F_CHEM_STORE_PRODUCTINFOS = new F_CHEM_STORE_PRODUCTINFO
                        {
                            PRODUCTID = (f.F_CHEM_STORE_PRODUCTINFOS != null ? f.F_CHEM_STORE_PRODUCTINFOS.PRODUCTID : 0),
                            PRODUCTNAME = (f.F_CHEM_STORE_PRODUCTINFOS != null ? f.F_CHEM_STORE_PRODUCTINFOS.PRODUCTNAME : null)
                        },
                        F_BAS_UNITS = new F_BAS_UNITS
                        {
                            UID = (f.F_BAS_UNITS != null ? f.F_BAS_UNITS.UID : 0),
                            UNAME = (f.F_BAS_UNITS != null ? f.F_BAS_UNITS.UNAME : null)
                        }
                    }).ToList()
                }).FirstOrDefaultAsync();

            return comImpInvoiceInfoDetailsViewModel;
        }

        public async Task<ComImpInvoiceInfoEditViewModel> FindByInvIdAsync(int invId)
        {
            try
            {
                var comImpInvoiceInfoEditViewModel = await DenimDbContext.COM_IMP_INVOICEINFO
                    .Include(e => e.TRNSP)
                    .Include(e => e.DelStatus)
                    .Include(e => e.ComImpInvdetailses)
                    .Include(e => e.ComImpInvdetailses)
                    .ThenInclude(e => e.BasProductinfo)
                    .Include(e => e.ComImpInvdetailses)
                    .ThenInclude(e => e.F_CHEM_STORE_PRODUCTINFOS)
                    .Include(e => e.ComImpInvdetailses)
                    .ThenInclude(e => e.BAS_YARN_LOTINFOS)
                    .Include(e => e.ComImpInvdetailses)
                    .ThenInclude(e => e.F_BAS_UNITS)
                    .Where(e => e.INVID.Equals(invId))
                    .Select(e => new ComImpInvoiceInfoEditViewModel
                    {
                        ComImpInvoiceinfo = new COM_IMP_INVOICEINFO
                        {
                            INVID = e.INVID,
                            EncryptedId = _protector.Protect(e.INVID.ToString()),
                            INVNO = e.INVNO,
                            INVDATE = e.INVDATE,
                            INVPATH = e.INVPATH,
                            LC_ID = e.LC_ID,
                            LPORT = e.LPORT,
                            DOCHANDSON = e.DOCHANDSON,
                            ETADATE = e.ETADATE,
                            DEL_STATUS = e.DEL_STATUS,
                            DEL_DATE = e.DEL_DATE,
                            CONTQTY = e.CONTQTY,
                            CONTSIZE = e.CONTSIZE,
                            SBDATE = e.SBDATE,
                            BLNO = e.BLNO,
                            BLDATE = e.BLDATE,
                            BLPATH = e.BLPATH,
                            BENTRYNO = e.BENTRYNO,
                            BENTRYDATE = e.BENTRYDATE,
                            MPNO = e.MPNO,
                            MPDATE = e.MPDATE,
                            MPSUB_DATE = e.MPSUB_DATE,
                            MPBILL = e.MPBILL,
                            RCVSTATUS = e.RCVSTATUS,
                            MRRNO = e.MRRNO,
                            MRRDATE = e.MRRDATE,
                            PAYMENTDATE = e.PAYMENTDATE,
                            CnFBILL = e.CnFBILL,
                            CnFBILLDATE = e.CnFBILLDATE,
                            TRNSPID = e.TRNSPID,
                            TRUCKQTY = e.TRUCKQTY,
                            TRNSPBILL = e.TRNSPBILL,
                            TRNSPBILLDATE = e.TRNSPBILLDATE,
                            TRNS_BILL_SUB_DATE = e.TRNS_BILL_SUB_DATE,
                            SHIPBY = e.SHIPBY,
                            STATUS = e.STATUS,
                            REMARKS = e.REMARKS,
                            USRID = e.USRID,
                            DelStatus = e.DelStatus,
                            COM_CONTAINER = e.COM_CONTAINER,
                            TRNSP = e.TRNSP,
                            BENTRYVALUE = e.BENTRYVALUE,
                            CnF = e.CnF
                        },
                        ComImpInvdetailses = e.ComImpInvdetailses.Select(f => new COM_IMP_INVDETAILS
                        {
                            TRNSID = f.TRNSID,
                            EncryptedId = _protector.Protect(f.TRNSID.ToString()),
                            INVNO = f.INVNO,
                            QTY = f.QTY,
                            RATE = f.RATE,
                            AMOUNT = f.AMOUNT,
                            REMARKS = f.REMARKS,
                            USRID = f.USRID,
                            PRODID = f.PRODID,
                            CHEMPRODID = f.CHEMPRODID,
                            YARNLOTID = f.YARNLOTID,
                            UNIT = f.UNIT,
                            BasProductinfo = new BAS_PRODUCTINFO
                            {
                                PRODID = (f.BasProductinfo != null ? f.BasProductinfo.PRODID : 0),
                                PRODNAME = (f.BasProductinfo != null ? f.BasProductinfo.PRODNAME : null)
                            },
                            BAS_YARN_LOTINFOS = new BAS_YARN_LOTINFO
                            {
                                LOTID = (f.BAS_YARN_LOTINFOS != null ? f.BAS_YARN_LOTINFOS.LOTID : 0),
                                LOTNO = (f.BAS_YARN_LOTINFOS != null ? f.BAS_YARN_LOTINFOS.LOTNO : null)
                            },
                            F_CHEM_STORE_PRODUCTINFOS = new F_CHEM_STORE_PRODUCTINFO
                            {
                                PRODUCTID = (f.F_CHEM_STORE_PRODUCTINFOS != null ? f.F_CHEM_STORE_PRODUCTINFOS.PRODUCTID : 0),
                                PRODUCTNAME = (f.F_CHEM_STORE_PRODUCTINFOS != null ? f.F_CHEM_STORE_PRODUCTINFOS.PRODUCTNAME : null)
                            },
                            F_BAS_UNITS = new F_BAS_UNITS
                            {
                                UID = (f.F_BAS_UNITS != null ? f.F_BAS_UNITS.UID : 0),
                                UNAME = (f.F_BAS_UNITS != null ? f.F_BAS_UNITS.UNAME : null)
                            }

                        }).ToList(),
                    }).FirstOrDefaultAsync();

                //comImpInvoiceInfoEditViewModel.ComImpInvdetails.INVNO = comImpInvoiceInfoEditViewModel.ComImpInvoiceinfo.INVNO;

                //comImpInvoiceInfoEditViewModel.BasProductinfos = await _denimDbContext.BAS_PRODUCTINFO.Select(e => new BAS_PRODUCTINFO
                //{
                //    PRODID = e.PRODID,
                //    PRODNAME = e.PRODNAME
                //}).OrderBy(e => e.PRODNAME).ToListAsync();

                comImpInvoiceInfoEditViewModel.FChemStoreProductinfos = await DenimDbContext.F_CHEM_STORE_PRODUCTINFO.Select(e => new F_CHEM_STORE_PRODUCTINFO
                {
                    PRODUCTID = e.PRODUCTID,
                    PRODUCTNAME = e.PRODUCTNAME
                }).OrderBy(e => e.PRODUCTNAME).ToListAsync();

                comImpInvoiceInfoEditViewModel.BasYarnLotinfos = await DenimDbContext.BAS_YARN_LOTINFO.Select(e => new BAS_YARN_LOTINFO
                {
                    LOTID = e.LOTID,
                    LOTNO = e.LOTNO
                }).OrderBy(e => e.LOTNO).ToListAsync();

                #region Previous Logics

                //if (comImpInvoiceInfoEditViewModel.ComImpInvdetailses.Any(e => e.PRODID is { } or > 0))
                //{
                //    comImpInvoiceInfoEditViewModel.BasProductinfos = await _denimDbContext.BAS_PRODUCTINFO.Select(e => new BAS_PRODUCTINFO
                //    {
                //        PRODID = e.PRODID,
                //        PRODNAME = e.PRODNAME
                //    }).OrderBy(e => e.PRODNAME).ToListAsync();
                //}

                //if (comImpInvoiceInfoEditViewModel.ComImpInvdetailses.Any(e => e.CHEMPRODID is { } or > 0))
                //{
                //    comImpInvoiceInfoEditViewModel.FChemStoreProductinfos = await _denimDbContext.F_CHEM_STORE_PRODUCTINFO.Select(e => new F_CHEM_STORE_PRODUCTINFO
                //    {
                //        PRODUCTID = e.PRODUCTID,
                //        PRODUCTNAME = e.PRODUCTNAME
                //    }).OrderBy(e => e.PRODUCTNAME).ToListAsync();
                //}

                //if (comImpInvoiceInfoEditViewModel.ComImpInvdetailses.Any(e => e.YARNLOTID is { } or > 0))
                //{
                //    comImpInvoiceInfoEditViewModel.BasYarnLotinfos = await _denimDbContext.BAS_YARN_LOTINFO.Select(e => new BAS_YARN_LOTINFO
                //    {
                //        LOTID = e.LOTID,
                //        LOTNO = e.LOTNO
                //    }).OrderBy(e => e.LOTNO).ToListAsync();
                //}

                #endregion

                comImpInvoiceInfoEditViewModel.ComImpLcinformations = await DenimDbContext.COM_IMP_LCINFORMATION
                    .Include(e => e.CAT)
                    .Select(e => new COM_IMP_LCINFORMATION
                    {
                        LC_ID = e.LC_ID,
                        LCNO = $"{e.LCNO}, LC type: {e.CAT.CATEGORY}",
                        LCDATE = e.LCDATE
                    }).OrderBy(e => e.LCDATE).ToListAsync();

                comImpInvoiceInfoEditViewModel.ComImpDelStatuses = await DenimDbContext.COM_IMP_DEL_STATUS.ToListAsync();

                comImpInvoiceInfoEditViewModel.BasTransportinfos = await DenimDbContext.BAS_TRANSPORTINFO.Select(e => new BAS_TRANSPORTINFO
                {
                    TRNSPID = e.TRNSPID,
                    TRNSPNAME = e.TRNSPNAME
                }).ToListAsync();

                comImpInvoiceInfoEditViewModel.FBasUnitses = await DenimDbContext.F_BAS_UNITS.Select(e => new F_BAS_UNITS
                {
                    UID = e.UID,
                    UNAME = e.UNAME
                }).OrderByDescending(e => e.UNAME.ToLower().Contains("kg")).ThenBy(e => e.UNAME).ToListAsync();

                comImpInvoiceInfoEditViewModel.ComContainers = await DenimDbContext.COM_CONTAINER.OrderBy(e => e.CONTAINERSIZE).ToListAsync();

                comImpInvoiceInfoEditViewModel.CnFList = await DenimDbContext.COM_IMP_CNFINFO
                    .Select(e => new COM_IMP_CNFINFO
                    {
                        CNFID = e.CNFID,
                        CNFNAME = e.CNFNAME
                    }).OrderBy(e => e.CNFNAME).ToListAsync();

                return comImpInvoiceInfoEditViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ComImpInvoiceInfoCreateViewModel> GetInitObjForDetailsTableByAsync(ComImpInvoiceInfoCreateViewModel comImpInvoiceInfoCreateViewModel)
        {
            foreach (var item in comImpInvoiceInfoCreateViewModel.ComImpInvdetailses)
            {
                item.BasProductinfo = await DenimDbContext.BAS_PRODUCTINFO.FirstOrDefaultAsync(e => e.PRODID.Equals(item.PRODID));
                item.F_BAS_UNITS = await DenimDbContext.F_BAS_UNITS.FirstOrDefaultAsync(e => e.UID.Equals(item.UNIT));
                item.F_CHEM_STORE_PRODUCTINFOS = await DenimDbContext.F_CHEM_STORE_PRODUCTINFO.FirstOrDefaultAsync(e => e.PRODUCTID.Equals(item.CHEMPRODID));
                item.BAS_YARN_LOTINFOS = await DenimDbContext.BAS_YARN_LOTINFO.FirstOrDefaultAsync(e => e.LOTID.Equals(item.YARNLOTID));
            }

            return comImpInvoiceInfoCreateViewModel;
        }

        public async Task<dynamic> GetProductInfoByAsync(int lc_Id)
        {
            // MAPPED WITH DB ROWS
            var ignores = new Dictionary<int, string>
            {
                {11, "Yarn"},
                {12, "Chemical"},
                {14, "Yarn Sample"}
            };

            var checkPoint = await DenimDbContext.COM_IMP_LCINFORMATION
                .Include(e => e.CAT)
                .AnyAsync(e => e.LC_ID.Equals(lc_Id) && DenimDbContext.BAS_PRODCATEGORY
                    .Any(f => !ignores.ContainsKey(e.CATID) || !ignores.ContainsValue(e.CAT.CATEGORY)));

            if (checkPoint)
            {
                var firstOrDefaultAsync = await DenimDbContext.COM_IMP_LCINFORMATION
                    .Include(e => e.CAT.BAS_PRODUCTINFO)
                    .Where(e => e.LC_ID.Equals(lc_Id) &&
                                DenimDbContext.BAS_PRODCATEGORY.Any(f =>
                                    !ignores.ContainsKey(e.CATID) || !ignores.ContainsValue(e.CAT.CATEGORY)))
                    .Select(e => new
                    {
                        PRODINFO = e.CAT.BAS_PRODUCTINFO
                    }).FirstOrDefaultAsync();

                return firstOrDefaultAsync;
            }
            else
            {
                var comImpLcinformation = await DenimDbContext.COM_IMP_LCINFORMATION
                    .Where(e => e.LC_ID.Equals(lc_Id))
                    .Select(e => new COM_IMP_LCINFORMATION
                    {
                        CATID = e.CATID
                    }).FirstOrDefaultAsync();

                switch (comImpLcinformation.CATID)
                {
                    case 11:
                        return new
                        {
                            YARNLOTINFO = await DenimDbContext.BAS_YARN_LOTINFO.Select(e => new BAS_YARN_LOTINFO
                            {
                                LOTID = e.LOTID,
                                LOTNO = e.LOTNO
                            }).OrderBy(e => e.LOTNO).ToListAsync()
                        };
                    case 12 or 14:
                        return new
                        {
                            CHEMSTOREPRODINFO = await DenimDbContext.F_CHEM_STORE_PRODUCTINFO.Select(e => new F_CHEM_STORE_PRODUCTINFO
                            {
                                PRODUCTID = e.PRODUCTID,
                                PRODUCTNAME = e.PRODUCTNAME
                            }).OrderBy(e => e.PRODUCTNAME).ToListAsync()
                        };
                    default:
                        return null;
                }
            }
        }
    }
}
