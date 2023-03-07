using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.SampleGarments.HDispatch;
using DenimERP.ViewModels;
using DenimERP.ViewModels.SampleGarments.HDispatch;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.SampleGarments.HDispatch
{
    public class SQLH_SAMPLE_DESPATCH_M_Repository : BaseRepository<H_SAMPLE_DESPATCH_M>, IH_SAMPLE_DESPATCH_M
    {
        private readonly IDataProtector _protector;

        public SQLH_SAMPLE_DESPATCH_M_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<CreateHSampleDespatchMViewModel> GetInitObjects(CreateHSampleDespatchMViewModel createHSampleDespatchMViewModel)
        {
            createHSampleDespatchMViewModel.FBasUnitses = await DenimDbContext.F_BAS_UNITS.Select(e => new F_BAS_UNITS
            {
                UID = e.UID,
                UNAME = e.UNAME
            }).OrderBy(e => e.UNAME).ToListAsync();

            createHSampleDespatchMViewModel.BasBuyerinfos = await DenimDbContext.BAS_BUYERINFO.Select(e => new BAS_BUYERINFO
            {
                BUYERID = e.BUYERID,
                BUYER_NAME = e.BUYER_NAME
            }).OrderBy(e => e.BUYER_NAME).ToListAsync();

            createHSampleDespatchMViewModel.BasBrandinfos = await DenimDbContext.BAS_BRANDINFO.Select(e => new BAS_BRANDINFO
            {
                BRANDID = e.BRANDID,
                BRANDNAME = e.BRANDNAME
            }).OrderBy(e => e.BRANDNAME).ToListAsync();

            createHSampleDespatchMViewModel.BasTeaminfos = await DenimDbContext.BAS_TEAMINFO.Select(e => new BAS_TEAMINFO
            {
                TEAMID = e.TEAMID,
                TEAM_NAME = e.TEAM_NAME
            }).OrderBy(e => e.TEAM_NAME).ToListAsync();

            createHSampleDespatchMViewModel.ExtendHSampleReceivingDViewModels = await DenimDbContext.H_SAMPLE_RECEIVING_D
                .Include(e => e.RCV)
                .Include(e => e.BUYER)
                .Include(e => e.TRNS)
                .ThenInclude(e => e.SITEM)
                .Include(e => e.TRNS)
                .ThenInclude(e => e.FABCODENavigation)
                .ThenInclude(e => e.WV)
                //.Where(e => (e.QTY - _denimDbContext.H_SAMPLE_DESPATCH_D.Where(g => e.RCVDID.Equals(g.RCVDID)).Sum(f => f.QTY)) > 0)
                .Select(e => new ExtendHSampleReceivingDViewModel(e.RCVDID, $"Serial: {e.RCVDID}, GP No: {e.RCV.GPNO}, GP Date: {e.RCV.GPDATE.Value:dd-MM-yyyy}, Item Name: {e.TRNS.SITEM.NAME}, Buyer: {e.BUYER.BUYER_NAME}, Fabric Code: {e.TRNS.FABCODENavigation.STYLE_NAME}"))
                .OrderByDescending(e => e.CUST_NAME)
                .ToListAsync();

            return createHSampleDespatchMViewModel;
        }

        public async Task<int> GetGatePassNumber()
        {
            return await DenimDbContext.H_SAMPLE_DESPATCH_M.Select(e => e.GPNO).LastOrDefaultAsync() + 1;
        }

        public async Task<bool> IsAddableToHSampleDispatchAsync(CreateHSampleDespatchMViewModel createHSampleDespatchMViewModel)
        {
            var anyAsync = await DenimDbContext.H_SAMPLE_RECEIVING_D
                .Where(e => e.RCVDID.Equals(createHSampleDespatchMViewModel.HSampleDespatchD.RCVDID))
                .AnyAsync(e
                    => createHSampleDespatchMViewModel.HSampleDespatchD.QTY > 0
                       && e.QTY >= createHSampleDespatchMViewModel.HSampleDespatchD.QTY

                       && e.QTY >= createHSampleDespatchMViewModel.HSampleDespatchD.QTY +
                       DenimDbContext.H_SAMPLE_DESPATCH_D.Where(f => f.RCVDID.Equals(createHSampleDespatchMViewModel.HSampleDespatchD.RCVDID)).Sum(f => f.QTY)

                       && e.QTY >= createHSampleDespatchMViewModel.HSampleDespatchD.QTY +
                       createHSampleDespatchMViewModel.HSampleDespatchDs.Where(g => g.RCVDID.Equals(createHSampleDespatchMViewModel.HSampleDespatchD.RCVDID)).Sum(g => g.QTY)

                       && e.QTY >= createHSampleDespatchMViewModel.HSampleDespatchD.QTY +
                       DenimDbContext.H_SAMPLE_DESPATCH_D.Where(f => f.RCVDID.Equals(createHSampleDespatchMViewModel.HSampleDespatchD.RCVDID)).Sum(f => f.QTY) +
                       createHSampleDespatchMViewModel.HSampleDespatchDs.Where(g => g.RCVDID.Equals(createHSampleDespatchMViewModel.HSampleDespatchD.RCVDID)).Sum(g => g.QTY)
                );

            return anyAsync;
        }

        public async Task<DataTableObject<H_SAMPLE_DESPATCH_M>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip,
            int pageSize)
        {
            var navigationPropertyStrings = new[] { "HSP", "BRAND", "HST.TEAM" };
            var hSampleDespatchMs = DenimDbContext.H_SAMPLE_DESPATCH_M
                .Include(e => e.HSP)
                .Include(e => e.BRAND)
                .Include(e => e.PURPOSENavigation)
                .Include(e => e.HST)
                .Select(e => new H_SAMPLE_DESPATCH_M
                {
                    SDID = e.SDID,
                    EncryptedId = _protector.Protect(e.SDID.ToString()),
                    SDDATE = e.SDDATE,
                    GPNO = e.GPNO,
                    GPDATE = e.GPDATE,
                    HSP = new BAS_BUYERINFO
                    {
                        BUYER_NAME = e.HSP.BUYER_NAME
                    },
                    BRAND = new BAS_BRANDINFO
                    {
                        BRANDNAME = e.BRAND.BRANDNAME
                    },
                    PURPOSE = e.PURPOSE,
                    STATUS = e.STATUS,
                    RTNDATE = e.RTNDATE,
                    HST = e.HST,
                    THROUGH = e.THROUGH,
                    COST_STATUS = e.COST_STATUS,
                    REMARKS = e.REMARKS,
                    PURPOSENavigation = new F_BAS_UNITS
                    {
                        UID = e.PURPOSENavigation.UID,
                        UNAME = e.PURPOSENavigation.UNAME
                    }
                });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                hSampleDespatchMs = OrderedResult<H_SAMPLE_DESPATCH_M>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, hSampleDespatchMs);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                hSampleDespatchMs = hSampleDespatchMs
                    .Where(m => m.SDDATE.ToString().ToUpper(CultureInfo.InvariantCulture).ToUpper().Contains(searchValue)
                                || m.GPNO > 0 && m.GPNO.ToString().ToUpper().Contains(searchValue)
                                || m.GPDATE != null && m.GPDATE.ToString(CultureInfo.InvariantCulture).ToUpper().Contains(searchValue)
                                || m.HSP.BUYER_NAME != null && m.HSP.BUYER_NAME.ToUpper().Contains(searchValue)
                                || m.BRAND.BRANDNAME != null && m.BRAND.BRANDNAME.ToUpper().Contains(searchValue)
                                || m.PURPOSENavigation.UNAME != null && m.PURPOSENavigation.UNAME.ToUpper().Contains(searchValue)
                                || m.STATUS != null && m.STATUS.ToUpper().Contains(searchValue)
                                || m.RTNDATE != null && m.RTNDATE.ToString().ToUpper(CultureInfo.InvariantCulture).Contains(searchValue)
                                || m.HST.TEAM_NAME != null && m.HST.TEAM_NAME.ToString().ToUpper().Contains(searchValue)
                                || m.THROUGH != null && m.THROUGH.ToString().ToUpper().Contains(searchValue)
                                || m.COST_STATUS != null && m.COST_STATUS.ToString().ToUpper().Contains(searchValue)
                                || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue));

                hSampleDespatchMs = OrderedResult<H_SAMPLE_DESPATCH_M>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, hSampleDespatchMs);
            }

            var recordsTotal = await hSampleDespatchMs.CountAsync();

            return new DataTableObject<H_SAMPLE_DESPATCH_M>(draw, recordsTotal, recordsTotal, await hSampleDespatchMs.Skip(skip).Take(pageSize).ToListAsync());
        }

        public async Task<CreateHSampleDespatchMViewModel> FindByHsdIdIncludeAllAsync(int hsdId)
        {
            try
            {
                var result = await DenimDbContext.H_SAMPLE_DESPATCH_M
                    .Include(e => e.HSP)
                    .Include(e => e.BRAND)
                    .Include(e => e.PURPOSENavigation)
                    .Include(e => e.HST)
                    .Include(e => e.H_SAMPLE_DESPATCH_D)
                    .ThenInclude(e => e.RCVD)
                    .ThenInclude(e => e.TRNS)
                    .ThenInclude(e => e.SITEM)
                    .Include(e => e.H_SAMPLE_DESPATCH_D)
                    .ThenInclude(e => e.RCVD)
                    .ThenInclude(e => e.RCV)
                    .Select(e => new CreateHSampleDespatchMViewModel
                    {
                        HSampleDespatchM = new H_SAMPLE_DESPATCH_M
                        {
                            SDID = e.SDID,
                            EncryptedId = _protector.Protect(e.SDID.ToString()),
                            SDDATE = e.SDDATE,
                            GPNO = e.GPNO,
                            GPDATE = e.GPDATE,
                            HSPID = e.HSPID,
                            BRANDID = e.BRANDID,
                            BRAND = e.BRAND,
                            PURPOSE = e.PURPOSE,
                            STATUS = e.STATUS,
                            RTNDATE = e.RTNDATE,
                            HSTID = e.HSTID,
                            HSP = e.HSP,
                            HST = e.HST,
                            THROUGH = e.THROUGH,
                            COST_STATUS = e.COST_STATUS,
                            REMARKS = e.REMARKS,
                            PURPOSENavigation = e.PURPOSENavigation
                        },
                        HSampleDespatchDs = e.H_SAMPLE_DESPATCH_D.ToList()
                    }).FirstOrDefaultAsync(e => e.HSampleDespatchM.SDID.Equals(hsdId));

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