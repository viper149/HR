using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Com;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOM_IMP_LCINFORMATION_Repository : BaseRepository<COM_IMP_LCINFORMATION>, ICOM_IMP_LCINFORMATION
    {
        private readonly IDataProtector _protector;

        public SQLCOM_IMP_LCINFORMATION_Repository(DenimDbContext denimDbContext,
                IDataProtectionProvider dataProtectionProvider,
                DataProtectionPurposeStrings dataProtectionPurposeStrings)
            : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<COM_IMP_LCINFORMATION> FindByIdInlcudeOtherObjAsync(int id)
        {
            try
            {
                var result = await DenimDbContext.COM_IMP_LCINFORMATION
                    .Include(e => e.CAT)
                    .Include(e => e.SUPP)
                    .Include(e => e.BANK)
                    .Include(e => e.BANK_)
                    .Include(e => e.ComExLcinfo)
                    .Include(e => e.INS)
                    .Include(c => c.COM_TENOR)
                    .Include(c => c.COM_IMP_LCDETAILS)
                    .ThenInclude(e => e.BAS_PRODUCTINFO)
                    .Include(c => c.COM_IMP_LCDETAILS)
                    .ThenInclude(e => e.F_BAS_UNITS)
                    .FirstOrDefaultAsync(e => e.LC_ID.Equals(id));
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> FindByLcNoAsync(string lcNo)
        {
            try
            {
                var result = await DenimDbContext.COM_IMP_LCINFORMATION.Where(e => e.LCNO.ToLower().Equals(lcNo.ToLower())).ToListAsync();
                return result.Any();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<int> TotalNumberOfComImpLcInformationList()
        {
            try
            {

                var totalItem = await DenimDbContext.COM_IMP_LCINFORMATION.CountAsync();
                return totalItem;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> TotalPercentageOfComImpLcInformationList(DateTime dateTime, int days = 7)
        {
            return await DenimDbContext.COM_IMP_LCINFORMATION.Where(e => e.LCDATE > dateTime.AddDays(-days)).CountAsync();
        }

        public async Task<ComImpLcInformationForCreateViewModel> GetComImpLcInformationForCreateViewModelValue(
            ComImpLcInformationForCreateViewModel comImpLcInformationForCreateViewModel)
        {
            try
            {
                comImpLcInformationForCreateViewModel.FBasUnitses = await DenimDbContext.F_BAS_UNITS.Select(e => new F_BAS_UNITS
                {
                    UID = e.UID,
                    UNAME = e.UNAME
                }).OrderByDescending(e => e.UNAME.ToLower().Contains("kg")).ThenBy(e => e.UNAME).ToListAsync();

                comImpLcInformationForCreateViewModel.bAS_PRODCATEGORies = await DenimDbContext.BAS_PRODCATEGORY.Select(e => new BAS_PRODCATEGORY
                {
                    CATID = e.CATID,
                    CATEGORY = e.CATEGORY
                }).OrderBy(e => e.CATEGORY).ToListAsync();

                comImpLcInformationForCreateViewModel.BasProductinfos = await DenimDbContext.BAS_PRODUCTINFO.Select(e => new BAS_PRODUCTINFO
                {
                    PRODID = e.PRODID,
                    PRODNAME = e.PRODNAME
                }).OrderBy(e => e.PRODNAME).ToListAsync();

                comImpLcInformationForCreateViewModel.bAS_SUPPLIERINFOs = await DenimDbContext.BAS_SUPPLIERINFO.Select(e => new BAS_SUPPLIERINFO
                {
                    SUPPID = e.SUPPID,
                    SUPPNAME = e.SUPPNAME
                }).OrderBy(e => e.SUPPNAME).ToListAsync();

                


                comImpLcInformationForCreateViewModel.bAS_BEN_BANK_MASTERs = await DenimDbContext.BAS_BEN_BANK_MASTER.ToListAsync();
                comImpLcInformationForCreateViewModel.bAS_BUYER_BANK_MASTERs = await DenimDbContext.BAS_BUYER_BANK_MASTER.ToListAsync();
                comImpLcInformationForCreateViewModel.bAS_INSURANCEINFOs = await DenimDbContext.BAS_INSURANCEINFO.OrderBy(c => c.INSNAME).ToListAsync();
                comImpLcInformationForCreateViewModel.ComTenors = await DenimDbContext.COM_TENOR.ToListAsync();
                comImpLcInformationForCreateViewModel.ComImpLctypes = await DenimDbContext.COM_IMP_LCTYPE.ToListAsync();

                comImpLcInformationForCreateViewModel.cOM_IMP_LCINFORMATION.LCDATE = DateTime.Now;
                comImpLcInformationForCreateViewModel.cOM_IMP_LCDETAILS.TRNSDATE = DateTime.Now;
                comImpLcInformationForCreateViewModel.cOM_IMP_LCDETAILS.PINO = 0.ToString();
                comImpLcInformationForCreateViewModel.cOM_IMP_LCDETAILS.QTY = 0;
                comImpLcInformationForCreateViewModel.cOM_IMP_LCDETAILS.RATE = 0;

                comImpLcInformationForCreateViewModel.ComExLcinfos = await DenimDbContext.COM_EX_LCINFO.Where(e => e.SHIP_DATE > comImpLcInformationForCreateViewModel.cOM_IMP_LCINFORMATION.LCDATE).Select(e => new COM_EX_LCINFO
                {
                    LCID = e.LCID,
                    LCNO = $"{e.LCNO} ({e.FILENO})"
                }).OrderBy(e => e.LCNO).ToListAsync();

                comImpLcInformationForCreateViewModel.cOM_IMP_LCDETAILS.TOTAL = ((double?)comImpLcInformationForCreateViewModel.cOM_IMP_LCDETAILS.QTY * comImpLcInformationForCreateViewModel.cOM_IMP_LCDETAILS.RATE);

                return comImpLcInformationForCreateViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int?> GetFileNumberByCategory(int categoryId)
        {
            try
            {
                var listAsync = await DenimDbContext.COM_IMP_LCINFORMATION.Where(e => e.CATID.Equals(categoryId))
                    .CountAsync();
                return listAsync + 1;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<COM_IMP_LCINFORMATION>> FindByExportLCIdAsync(int exportLcId)
        {
            try
            {
                var comImpLcinformations = await DenimDbContext.COM_IMP_LCINFORMATION.Where(e => e.LCID.Equals(exportLcId) && e.LTID.Equals(2)).ToListAsync();
                return comImpLcinformations;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<IEnumerable<COM_IMP_LCINFORMATION>> GetAllAsync()
        {
            var comImpLcInformation = await DenimDbContext.COM_IMP_LCINFORMATION
                .Include(e => e.CAT)
                .Include(e => e.SUPP)
                .Include(c => c.COM_TENOR)
                .Include(e => e.COM_IMP_LCTYPE)
                .Include(e => e.ComExLcinfo)
                .Select(e => new COM_IMP_LCINFORMATION
                {
                    LC_ID = e.LC_ID,
                    LCNO = e.LCNO,
                    EncryptedId = _protector.Protect(e.LC_ID.ToString()),
                    COM_IMP_LCTYPE = e.COM_IMP_LCTYPE,
                    CURRENCY = e.CURRENCY,
                    LCDATE = e.LCDATE,
                    SHIPDATE = e.SHIPDATE,
                    EXPDATE = e.EXPDATE,
                    LIENVAL = e.LIENVAL,
                    SUPP = new BAS_SUPPLIERINFO
                    {
                        SUPPNAME = $"{e.SUPP.SUPPNAME}"
                    },
                    CAT = e.CAT,
                    REMARKS = e.REMARKS,
                    LCPATH = e.LCPATH,
                    ComExLcinfo = new COM_EX_LCINFO
                    {
                        LCNO = e.ComExLcinfo.LCNO,
                        LCDATE = e.ComExLcinfo.LCDATE,
                        VALUE = e.ComExLcinfo.VALUE
                    }
                }).ToListAsync();

            return comImpLcInformation;
        }

        public async Task<ComImpLcInformationForCreateViewModel> FindByLcIdInlcudeAllAsync(int lcId)
        {
            var comImpLcInformationForCreateViewModel = await DenimDbContext.COM_IMP_LCINFORMATION
                    .Include(e => e.BANK)
                    .Include(e => e.BANK_)
                    .Include(e => e.CAT)
                    .Include(e => e.INS)
                    .Include(e => e.SUPP)
                    .Include(e => e.COM_TENOR)
                    .Include(e => e.COM_IMP_LCTYPE)
                    .Include(e => e.COM_IMP_LCDETAILS)
                    .ThenInclude(e => e.F_BAS_UNITS)
                    .Include(e => e.COM_IMP_LCDETAILS)
                    .ThenInclude(e => e.BAS_PRODUCTINFO)
                    .Where(e => e.LC_ID.Equals(lcId))
                    .Select(e => new ComImpLcInformationForCreateViewModel
                    {
                        cOM_IMP_LCINFORMATION = new COM_IMP_LCINFORMATION
                        {
                            LC_ID = e.LC_ID,
                            EncryptedId = _protector.Protect(e.LC_ID.ToString()),
                            LCNO = e.LCNO,
                            LCDATE = e.LCDATE,
                            LCPATH = e.LCPATH,
                            LTID = e.LTID,
                            CURRENCY = e.CURRENCY,
                            ISFTT = e.ISFTT,
                            CATID = e.CATID,
                            SUPPID = e.SUPPID,
                            BANKID = e.BANKID,
                            SHIPDATE = e.SHIPDATE,
                            EXPDATE = e.EXPDATE,
                            TID = e.TID,
                            LCID = e.LCID,
                            TOLLERANCE = e.TOLLERANCE,
                            ORIGIN = e.ORIGIN,
                            DESPORT = e.DESPORT,
                            INSID = e.INSID,
                            INSPATH = e.INSPATH,
                            CNOTENO = e.CNOTENO,
                            BANK_ID = e.BANK_ID,
                            LIENVAL = e.LIENVAL,
                            BALANCE = e.BALANCE,
                            REMARKS = e.REMARKS,
                            USRID = e.USRID,
                            FILENO = e.FILENO,
                            BANK = e.BANK,
                            BANK_ = e.BANK_,
                            CAT = e.CAT,
                            INS = e.INS,
                            SUPP = e.SUPP,
                            COM_TENOR = e.COM_TENOR,
                            COM_IMP_LCTYPE = e.COM_IMP_LCTYPE,
                            ComExLcinfo = e.ComExLcinfo
                        },
                        cOM_IMP_LCDETAILs = e.COM_IMP_LCDETAILS.Select(f => new COM_IMP_LCDETAILS
                        {
                            TRNSID = f.TRNSID,
                            TRNSDATE = f.TRNSDATE,
                            LCNO = f.LCNO,
                            LC_ID = f.LC_ID,
                            PINO = f.PINO,
                            HSCODE = f.HSCODE,
                            PIDATE = f.PIDATE,
                            PIPATH = f.PIPATH,
                            PRODID = f.PRODID,
                            UNIT = f.UNIT,
                            QTY = f.QTY,
                            RATE = f.RATE,
                            TOTAL = f.TOTAL,
                            USRID = f.USRID,
                            F_BAS_UNITS = f.F_BAS_UNITS,
                            BAS_PRODUCTINFO = f.BAS_PRODUCTINFO
                        }).ToList()

                    }).FirstOrDefaultAsync();

            return comImpLcInformationForCreateViewModel;
        }

        public async Task<bool> FindFileNoByAsync(ComImpLcInformationForCreateViewModel ComImpLcInformationForCreateViewModel)
        {
            return await DenimDbContext.COM_IMP_LCINFORMATION.AnyAsync(e => e.FILENO.ToLower().Contains(ComImpLcInformationForCreateViewModel.cOM_IMP_LCINFORMATION.FILENO.ToLower()));
        }
    }
}
