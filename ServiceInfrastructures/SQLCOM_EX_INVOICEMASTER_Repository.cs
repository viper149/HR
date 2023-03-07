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
using DenimERP.ViewModels.Com.Export;
using DenimERP.ViewModels.Com.InvoiceExport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOM_EX_INVOICEMASTER_Repository : BaseRepository<COM_EX_INVOICEMASTER>, ICOM_EX_INVOICEMASTER
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtector _protector;

        public SQLCOM_EX_INVOICEMASTER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IAuthorizationService authorizationService,
            IHttpContextAccessor httpContextAccessor) : base(denimDbContext)
        {
            _authorizationService = authorizationService;
            _httpContextAccessor = httpContextAccessor;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<ComExInvoiceMasterCreateViewModel> GetInitObjByAsync(ComExInvoiceMasterCreateViewModel comExInvoiceMasterCreateViewModel)
        {
            comExInvoiceMasterCreateViewModel.ComExLcinfos = await DenimDbContext.COM_EX_LCINFO.Select(c => new COM_EX_LCINFO
            {
                LCID = c.LCID,
                LCNO = $"{c.LCNO}; ({c.FILENO})",
                FILENO = c.FILENO
            }).OrderByDescending(e => e.FILENO).ToListAsync();

            comExInvoiceMasterCreateViewModel.BasBuyerinfos = await DenimDbContext.BAS_BUYERINFO.Select(e => new BAS_BUYERINFO
            {
                BUYERID = e.BUYERID,
                BUYER_NAME = e.BUYER_NAME
            }).OrderBy(e => e.BUYER_NAME).ToListAsync();

            switch (comExInvoiceMasterCreateViewModel.ComExInvoicemaster)
            {
                case { LCID: > 0 }:
                    {
                        //var result = await _denimDbContext.COM_EX_LCINFO
                        //    .Include(e => e.COM_EX_LCDETAILS)
                        //    .ThenInclude(e => e.PI.COM_EX_PI_DETAILS)
                        //    .ThenInclude(e => e.STYLE.FABCODENavigation)
                        //    .Where(e => e.LCID.Equals(comExInvoiceMasterCreateViewModel.ComExInvoicemaster.LCID))
                        //    .Select(e => e.COM_EX_LCDETAILS.Select(f => f.PI.COM_EX_PI_DETAILS.Select(g => new COM_EX_FABSTYLE
                        //    {
                        //        STYLEID = g.STYLE.STYLEID,
                        //        STYLENAME = g.STYLE.STYLENAME,
                        //        FABCODENavigation = new RND_FABRICINFO
                        //        {
                        //            STYLE_NAME = $"{g.PINO}; {g.STYLE.STYLENAME}; {g.STYLE.FABCODENavigation.STYLE_NAME}"
                        //        }
                        //    }).ToList()).ToList()).FirstOrDefaultAsync();

                        //foreach (var item1 in result.SelectMany(item => item))
                        //{
                        //    comExInvoiceMasterCreateViewModel.ComExFabstyles.Add(item1);
                        //}

                        var listAsync = await DenimDbContext.COM_EX_LCINFO
                            .Include(e => e.COM_EX_LCDETAILS)
                            .ThenInclude(e => e.PI.COM_EX_PI_DETAILS)
                            .ThenInclude(e => e.STYLE.FABCODENavigation)
                            .AsNoTracking()
                            .Where(e => e.LCID.Equals(comExInvoiceMasterCreateViewModel.ComExInvoicemaster.LCID))
                            .Select(e => e.COM_EX_LCDETAILS.Select(f => f.PI.COM_EX_PI_DETAILS.Select(g =>
                                new COM_EX_PI_DETAILS
                                {
                                    TRNSID = g.TRNSID,
                                    STYLE = new COM_EX_FABSTYLE                 
                                    {
                                        FABCODENavigation = new RND_FABRICINFO
                                        {
                                            STYLE_NAME = $"{g.PIMASTER.PINO} - {g.STYLE.STYLENAME} ({g.STYLE.FABCODENavigation.STYLE_NAME})"
                                        }
                                    }
                                }).ToList()).ToList()).FirstOrDefaultAsync();

                        foreach (var item1 in listAsync.SelectMany(item => item))
                        {
                            comExInvoiceMasterCreateViewModel.ComExPiDetailses.Add(item1);
                        }

                        break;
                    }
            }

            return comExInvoiceMasterCreateViewModel;
        }

        public async Task<ComExInvoiceMasterGetBuyerAndPDocNoViewModel> GetBuyerAndPDocNumber(int lcId)
        {
            try
            {
                var pDocNo = await DenimDbContext.COM_EX_INVOICEMASTER
                    .Where(e => e.LCID.Equals(lcId))
                    .CountAsync();

                var comExLcinfo = await DenimDbContext.COM_EX_LCINFO
                    .Include(e => e.BUYER)
                    .Where(e => e.LCID.Equals(lcId))
                    .FirstOrDefaultAsync();

                if (comExLcinfo.BUYERID != null)
                    return new ComExInvoiceMasterGetBuyerAndPDocNoViewModel()
                    {
                        BuyerId = (int)comExLcinfo.BUYERID,
                        BuyerName = comExLcinfo.BUYER.BUYER_NAME,
                        PDocNo = pDocNo == 0 ? 1 : (pDocNo + 1),
                        Value = comExLcinfo.VALUE,
                        fileno= comExLcinfo.FILENO,
                    };
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public async Task<bool> FindByInvNoAsync(string invNo)
        {
            var result = await DenimDbContext.COM_EX_INVOICEMASTER.Where(e => e.INVNO.Equals(invNo)).ToListAsync();
            return result.Any();
        }

        public async Task<COM_EX_INVOICEMASTER> FindByIdIncludeAllAsync(int invId)
        {
            return await DenimDbContext.COM_EX_INVOICEMASTER
                .Include(e => e.ComExInvdetailses)
                .ThenInclude(e => e.ComExFabstyle.FABCODENavigation)
                .Include(e => e.BUYER)
                .Include(e => e.LC.COM_TENOR)
                .Include(e => e.LC.COM_EX_LCDETAILS)
                .ThenInclude(e => e.PI.COM_EX_PI_DETAILS)
                .ThenInclude(e => e.STYLE.FABCODENavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.INVID.Equals(invId) && !e.STATUS.ToLower().Equals("realized"));
        }

        public async Task<COM_EX_INVOICEMASTER> FindByIdIncludeAllNotRealizedAsync(int invId)
        {
            
            return await DenimDbContext.COM_EX_INVOICEMASTER
                    .Include(e => e.BUYER)
                    .Include(c => c.LC.COM_EX_LCDETAILS)
                    .Include(c => c.LC.BANK)
                    .Include(c => c.LC.BANK_)
                    .Include(c => c.ComExInvdetailses)
                    .ThenInclude(c => c.ComExFabstyle)
                    .Where(e => e.INVID.Equals(invId)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<COM_EX_INVOICEMASTER>> GetComExInvoiceMasterBy(string searchBy)
        {
            try
            {
                var comExInvoicemasters = await DenimDbContext.COM_EX_INVOICEMASTER
                    .Include(e => e.BUYER)
                    .Where(e => e.INVNO.Contains(searchBy) || e.INVREF.Contains(searchBy) || e.BANK_REF.Contains(searchBy))
                    .ToListAsync();
                return comExInvoicemasters;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<DataTableObject<ExtendComExInvoiceMaster>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip,
            int pageSize)
        {
            try
            {
                var navigationPropertyStrings = new[] { "LC", "BUYER" };
                var authorizationResult = await _authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, "CustomsPolicy");

                var comExInvoicemasters = DenimDbContext.COM_EX_INVOICEMASTER
                    .Include(e => e.LC)
                    .Include(e => e.BUYER)
                    .Select(e => new ExtendComExInvoiceMaster
                    {
                        INVID = e.INVID,
                        EncryptedId = _protector.Protect(e.INVID.ToString()),
                        INVNO = e.INVNO,
                        INVDATE = e.INVDATE,
                        INV_QTY = e.INV_QTY,
                        INV_AMOUNT = e.INV_AMOUNT,
                        STATUS = e.STATUS,
                        BUYER = new BAS_BUYERINFO
                        {
                            BUYER_NAME = $"{e.BUYER.BUYER_NAME}"
                        },
                        LC = new COM_EX_LCINFO
                        {
                            LCNO = $"{e.LC.LCNO}"
                        },
                        ReadOnly = authorizationResult.Succeeded
                    });
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    comExInvoicemasters = OrderedResult<ExtendComExInvoiceMaster>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, comExInvoicemasters);
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    comExInvoicemasters = comExInvoicemasters
                        .Where(m => !string.IsNullOrEmpty(m.INVNO) && m.INVNO.ToUpper().Contains(searchValue)
                                    || m.INVDATE != null && m.INVDATE.ToShortDateString().ToUpper().Contains(searchValue)
                                    || m.LC != null && !string.IsNullOrEmpty(m.LC.LCNO) && m.LC.LCNO.ToUpper().Contains(searchValue)
                                    || m.INV_QTY != null && m.INV_QTY.ToString().ToUpper().Contains(searchValue)
                                    || m.INV_AMOUNT != null && m.INV_AMOUNT.ToString().ToUpper().Contains(searchValue)
                                    || m.BUYER != null && !string.IsNullOrEmpty(m.BUYER.BUYER_NAME) && m.BUYER.BUYER_NAME.ToUpper().Contains(searchValue)
                                    || !string.IsNullOrEmpty(m.STATUS) && m.STATUS.ToUpper().Contains(searchValue));

                    comExInvoicemasters = OrderedResult<ExtendComExInvoiceMaster>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, comExInvoicemasters);
                }

                var recordsTotal = await comExInvoicemasters.CountAsync();

                return new DataTableObject<ExtendComExInvoiceMaster>(draw, recordsTotal, recordsTotal, await comExInvoicemasters.Skip(skip).Take(pageSize).ToListAsync());

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<ComExInvoiceMasterInvoiceListViewModel>> GetInvoices(COM_EX_LCINFO comExLcinfo)
        {
            var comExInvoiceMasterInvoiceListViewModels = await DenimDbContext.COM_EX_INVOICEMASTER
                .Include(e => e.ComExInvdetailses)
                .Where(e => e.LCID.Equals(comExLcinfo.LCID))
                .Select(m => new ComExInvoiceMasterInvoiceListViewModel
                {
                    INVNO = m.INVNO,
                    DOCNO = m.PDOCNO,
                    ODRCVDATE = m.ODRCVDATE,
                    NEGODATE = m.NEGODATE,
                    QTY = m.ComExInvdetailses.Sum(e => e.QTY),
                    AMOUNT = m.ComExInvdetailses.Sum(e => e.AMOUNT)
                }).ToListAsync();

            return comExInvoiceMasterInvoiceListViewModels;
        }

        public async Task<IEnumerable<ComExInvoiceMasterPiListViewModel>> GetPI(COM_EX_LCINFO comExLcinfo)
        {
            try
            {
                var result = await DenimDbContext.COM_EX_LCDETAILS
                    .GroupJoin(DenimDbContext.COM_EX_PIMASTER,
                        ld => ld.PIID,
                        pm => pm.PIID,
                        (ld, pm) => new { ld, pm })
                    .SelectMany(p => p.pm.DefaultIfEmpty(),
                        (ld, pm) => new { ld.ld, pm })
                    .GroupJoin(DenimDbContext.COM_EX_PI_DETAILS,
                        lld => lld.pm.PINO,
                        pd => pd.PINO,
                        (lld, pd) => new { lld, pd })
                    .SelectMany(
                        lld => lld.pd.DefaultIfEmpty(),
                        (lld, pd) => new { lld.lld, pd })
                    .GroupJoin(DenimDbContext.COM_EX_FABSTYLE,
                        ppd => ppd.pd.STYLEID,
                        fs => fs.STYLEID,
                        (ppd, fs) => new { ppd, fs })
                    .Where(e => e.ppd.lld.ld.LCID.Equals(comExLcinfo.LCID))
                    .SelectMany(
                        ppd => ppd.fs.DefaultIfEmpty(),
                        (ppd, fs) => new ComExInvoiceMasterPiListViewModel
                        {
                            PINO = ppd.ppd.lld.pm.PINO,
                            STYLENAME = fs.STYLENAME,
                            UNITPRICE = ppd.ppd.pd.UNITPRICE,
                            QTY = (decimal?) ppd.ppd.pd.QTY,
                            TOTAL = ppd.ppd.pd.TOTAL
                        })
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<ComExInvoiceMasterDetailsViewModel> FindByInvIdWithOutRealizedAsync(int invId)
        {
            var comExInvoiceMasterDetailsViewModel = await DenimDbContext.COM_EX_INVOICEMASTER
                .Include(e => e.BUYER)
                .GroupJoin(DenimDbContext.COM_EX_INVDETAILS
                        .Include(e => e.ComExFabstyle),
                    f1 => f1.INVID,
                    f2 => f2.INVID,
                    (f1, f2) => new ComExInvoiceMasterDetailsViewModel
                    {
                        comExInvoicemaster = f1,
                        ComExInvdetailses = f2.ToList()
                    })
                .AsNoTracking()
                .Where(e => e.comExInvoicemaster.INVID.Equals(invId))
                .FirstOrDefaultAsync();

            comExInvoiceMasterDetailsViewModel.comExInvoicemaster.EncryptedId =
                _protector.Protect(comExInvoiceMasterDetailsViewModel.comExInvoicemaster.INVID.ToString());

            return comExInvoiceMasterDetailsViewModel;
        }

        public async Task<dynamic> GetFabricStylesByAsync(int lcId)
        {
            return await DenimDbContext.COM_EX_LCINFO
                    .Include(e => e.COM_EX_LCDETAILS)
                    .ThenInclude(e => e.PI.COM_EX_PI_DETAILS)
                    .ThenInclude(e => e.STYLE.FABCODENavigation)
                    .Where(e => e.LCID.Equals(lcId)).ToListAsync();
        }

        public async Task<ComExInvoiceMasterCreateViewModel> FindByinvIdIncludeAllAsync(int invId)
        {
            var comExInvoicemaster = await DenimDbContext.COM_EX_INVOICEMASTER
                .Include(e => e.LC.COM_TENOR)
                .Include(e => e.LC.COM_EX_LCDETAILS)
                .ThenInclude(e => e.PI.COM_EX_PI_DETAILS)
                .ThenInclude(e => e.STYLE.FABCODENavigation)
                .Include(e => e.ComExInvdetailses)
                .ThenInclude(e => e.PiDetails.STYLE.FABCODENavigation)
                .Include(e => e.ComExInvdetailses)
                .ThenInclude(e => e.ComExFabstyle.FABCODENavigation)
                .Where(e => e.INVID.Equals(invId))
                .AsNoTracking()
                .FirstOrDefaultAsync();

            ComExInvoiceMasterCreateViewModel comExInvoiceMasterCreateViewModel =
                new ComExInvoiceMasterCreateViewModel
                {
                    ComExInvoicemaster = comExInvoicemaster,
                    ComExInvdetailses = comExInvoicemaster.ComExInvdetailses.ToList()
                };

            return comExInvoiceMasterCreateViewModel;
        }

        public async Task<bool> HasBalanceByAsync(int lcId, double? amount)
        {
            //return await _denimDbContext.COM_EX_LCINFO.Where(e =>
            //        e.LCNO.Equals(_denimDbContext.COM_EX_LCINFO.FirstOrDefault(f => f.LCID.Equals(lcId)).LCNO))
            //    .SumAsync(e => e.VALUE) >= amount;

            //await _denimDbContext.COM_EX_INVOICEMASTER
            //    .Include(e => e.LC)
            //    .Include(e => e.ComExInvdetailses)
            //    .Where(e => e.LC.LCID.Equals(comExInvoiceMasterCreateViewModel.ComExInvoicemaster.LC.LCID))
            //    .SumAsync(e => e.ComExInvdetailses.Sum(f => f.AMOUNT))

            return await DenimDbContext.COM_EX_LCINFO.AnyAsync(e =>
                e.LCID.Equals(lcId) &&
                double.Parse($"{e.VALUE - DenimDbContext.COM_EX_INVOICEMASTER.Include(f => f.ComExInvdetailses).Where(f => f.LCID.Equals(lcId)).Sum(f => f.ComExInvdetailses.Sum(g => g.AMOUNT)):F}") >= amount);
        }

        public async Task<string> GetLastInvoiceNoAsync()
        {
            try
            {
                var invNo = "";
                var result = await DenimDbContext.COM_EX_INVOICEMASTER.OrderByDescending(c => c.INVNO).Select(c => c.INVNO).FirstOrDefaultAsync();
                var year = DateTime.Now.Year - 2000;

                if (result != null)
                {
                    var resultArray = result.Split("/");
                    if (int.Parse(resultArray[1]) < year)
                    {
                        invNo = "PDL/" + year + "/" + "1".PadLeft(4, '0');
                    }
                    else
                    {
                        invNo = "PDL/" + year + "/" + (int.Parse(resultArray[2]) + 1).ToString().PadLeft(4, '0');
                    }
                }
                else
                {
                    invNo = "PDL/" + year + "/" + "1".PadLeft(4, '0');
                }

                return invNo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<COM_EX_INVOICEMASTER> FindByIdForDeleteAsync(int invId)
        {
            return await DenimDbContext.COM_EX_INVOICEMASTER
                .Include(e => e.ComExInvdetailses)
                .Include(e => e.ACC_EXPORT_REALIZATION)
                .FirstOrDefaultAsync(e => e.INVID.Equals(invId));
        }

        public async Task<IEnumerable<ExtendComExInvoiceMaster>> GetAllAsync()
        {
            try
            {
                var navigationPropertyStrings = new[] { "LC", "BUYER" };
                var authorizationResult = await _authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, "CustomsPolicy");

                var comExInvoicemasters = DenimDbContext.COM_EX_INVOICEMASTER
                    .Include(e => e.LC.COM_EX_LCDETAILS)
                    .ThenInclude(d=>d.PI.BRAND)
                    .Include(e => e.BUYER)
                    .Select(e => new ExtendComExInvoiceMaster
                    {
                        INVID = e.INVID,
                        EncryptedId = _protector.Protect(e.INVID.ToString()),
                        INVNO = e.INVNO,
                        INVDATE = e.INVDATE,
                        INV_QTY = e.INV_QTY,
                        INV_AMOUNT = e.INV_AMOUNT,
                        STATUS = e.STATUS,
                        BANK_REF = e.BANK_REF,
                        IS_FINAL = e.IS_FINAL,
                        BUYER = new BAS_BUYERINFO
                        {
                            BUYER_NAME = $"{e.BUYER.BUYER_NAME}"
                        },
                        LC = new COM_EX_LCINFO
                        {
                            LCNO = $"{e.LC.LCNO}",
                            FILENO = $"{e.LC.FILENO}",
                            COM_EX_LCDETAILS = e.LC.COM_EX_LCDETAILS.Select(f=> new COM_EX_LCDETAILS
                            {
                                PI = new COM_EX_PIMASTER
                                {
                                    BRAND = new BAS_BRANDINFO
                                    {
                                        BRANDNAME = f.PI.BRAND.BRANDNAME
                                    }
                                }
                            }).ToList()
                        },
                        ReadOnly = authorizationResult.Succeeded
                    });

                return comExInvoicemasters.ToList();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<COM_EX_INVOICEMASTER> FindByIdIncludeAsync(int invId)
        {
            return await DenimDbContext.COM_EX_INVOICEMASTER
                .Include(e => e.ComExInvdetailses)
                .ThenInclude(e => e.ComExFabstyle.FABCODENavigation)
                .Include(e => e.BUYER)
                .Include(e => e.LC.COM_TENOR)
                .Include(e => e.LC.COM_EX_LCDETAILS)
                .ThenInclude(e => e.PI.COM_EX_PI_DETAILS)
                .ThenInclude(e => e.STYLE.FABCODENavigation)
                .FirstOrDefaultAsync(e => e.INVID.Equals(invId) && !e.STATUS.ToLower().Equals("realized"));
        }

        public async Task<CreateComExInvoiceMasterViewModel> FindByInvIdIncludeAllAsync(int invId)
        {
            var createComExInvoiceMasterViewModel = await DenimDbContext.COM_EX_INVOICEMASTER
                .Include(e => e.LC)
                .Include(e => e.ComExInvdetailses)
                .ThenInclude(e => e.PiDetails.STYLE.FABCODENavigation)
                .Where(e => e.INVID.Equals(invId))
                .Select(e => new CreateComExInvoiceMasterViewModel
                {
                    ComExInvoicemaster = new COM_EX_INVOICEMASTER
                    {
                        INVID = e.INVID,
                        EncryptedId = _protector.Protect(e.INVID.ToString()),
                        INVREF = e.INVREF,
                        INVNO = e.INVNO,
                        INVDATE = e.INVDATE,
                        INVDURATION = e.INVDURATION,
                        LCID = e.LCID,
                        BUYERID = e.BUYERID,
                        PDOCNO = e.PDOCNO,
                        DOC_NOTES = e.DOC_NOTES,
                        NEGODATE = e.NEGODATE,
                        TRUCKNO = e.TRUCKNO,
                        STATUS = e.STATUS,
                        ISACTIVE = e.ISACTIVE,
                        DELDATE = e.DELDATE,
                        DOC_SUB_DATE = e.DOC_SUB_DATE,
                        DOC_RCV_DATE = e.DOC_RCV_DATE,
                        BNK_SUB_DATE = e.BNK_SUB_DATE,
                        BANK_REF = e.BANK_REF,
                        BILL_DATE = e.BILL_DATE,
                        DISCREPANCY = e.DISCREPANCY,
                        BNK_ACC_DATE = e.BNK_ACC_DATE,
                        MATUDATE = e.MATUDATE,
                        BNK_ACC_POSTING = e.BNK_ACC_POSTING,
                        EXDATE = e.EXDATE,
                        ODAMOUNT = e.ODAMOUNT,
                        ODRCVDATE = e.ODRCVDATE,
                        PRCAMOUNT = e.PRCAMOUNT,
                        PRCDATE = e.PRCDATE,
                        PRCAMNTTK = e.PRCAMNTTK,
                        USRID = e.USRID,
                        BANKREFPATH = e.BANKREFPATH,
                        BANKACCEPTPATH = e.BANKACCEPTPATH,
                        PAYMENTPATH = e.PAYMENTPATH,
                        LC = new COM_EX_LCINFO
                        {
                            LCNO = $"{e.LC.LCNO}",
                            FILENO = $"{e.LC.FILENO}"
                        }
                    },
                    ComExInvdetailses = e.ComExInvdetailses.Select(f => new COM_EX_INVDETAILS
                    {
                        TRNSID = f.TRNSID,
                        INVNO = f.INVNO,
                        INVID = f.INVID,
                        STYLEID = f.STYLEID,
                        ROLL = f.ROLL,
                        QTY = f.QTY,
                        RATE = f.RATE,
                        AMOUNT = f.AMOUNT,
                        REMARKS = f.REMARKS,
                        PiDetails = new COM_EX_PI_DETAILS
                        {
                            TRNSID = f.PiDetails != null ? f.TRNSID : 0,
                            STYLE = new COM_EX_FABSTYLE
                            {
                                FABCODENavigation = new RND_FABRICINFO
                                {
                                    STYLE_NAME = $"{f.PiDetails.STYLE.FABCODENavigation.STYLE_NAME}"
                                }
                            }
                        }
                    }).ToList()
                }).AsNoTracking().FirstOrDefaultAsync();
            return createComExInvoiceMasterViewModel;
        }
        public async Task<IEnumerable<COM_EX_INVOICEMASTER>> GetInvoiceList()
        {
            return await DenimDbContext.COM_EX_INVOICEMASTER
                //.Where(d => !d.ComExGspInfos.Any(f => f.INVID.Equals(d.INVID)))
                .Select(d => new COM_EX_INVOICEMASTER
                {
                    INVID = d.INVID,
                    INVNO = d.INVNO
                }).ToListAsync();

        }
    }
}