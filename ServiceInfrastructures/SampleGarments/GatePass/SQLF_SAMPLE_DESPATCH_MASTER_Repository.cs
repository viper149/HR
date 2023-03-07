using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.SampleGarments.GatePass;
using DenimERP.ViewModels;
using DenimERP.ViewModels.SampleGarments.GatePass;
using DenimERP.ViewModels.SampleGarments.Receive;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.SampleGarments.GatePass
{
    public class SQLF_SAMPLE_DESPATCH_MASTER_Repository : BaseRepository<F_SAMPLE_DESPATCH_MASTER>, IF_SAMPLE_DESPATCH_MASTER
    {
        private readonly IDataProtector _protector;

        public SQLF_SAMPLE_DESPATCH_MASTER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<CreateFSampleDesPatchMasterViewModel> GetInitObjects(CreateFSampleDesPatchMasterViewModel createFSampleDesPatchMasterViewModel)
        {
            createFSampleDesPatchMasterViewModel.FSampleDespatchMasterTypes = await DenimDbContext.F_SAMPLE_DESPATCH_MASTER_TYPE.Select(e => new F_SAMPLE_DESPATCH_MASTER_TYPE { TYPEID = e.TYPEID, TYPENAME = e.TYPENAME }).OrderBy(e => e.TYPENAME).ToListAsync();
            createFSampleDesPatchMasterViewModel.FBasDriverinfos = await DenimDbContext.F_BAS_DRIVERINFO.Select(e => new F_BAS_DRIVERINFO { DRID = e.DRID, DRIVER_NAME = e.DRIVER_NAME }).OrderBy(e => e.DRIVER_NAME).ToListAsync();
            createFSampleDesPatchMasterViewModel.FBasVehicleInfos = await DenimDbContext.F_BAS_VEHICLE_INFO.Select(e => new F_BAS_VEHICLE_INFO { VID = e.VID, VNUMBER = e.VNUMBER }).OrderBy(e => e.VNUMBER).ToListAsync();

            createFSampleDesPatchMasterViewModel.ExtendFSampleGarmentRcvDs = await DenimDbContext.F_SAMPLE_GARMENT_RCV_D
                .Include(e => e.SITEM)
                .Include(e => e.FABCODENavigation.WV)
                .Include(e => e.FABCODENavigation.COLORCODENavigation)
                .Include(e => e.COLOR)
                .Where(e =>
                    DenimDbContext.F_SAMPLE_DESPATCH_DETAILS.Any(f => e.TRNSID.Equals(f.TRNSID)) ?
                        DenimDbContext.F_SAMPLE_DESPATCH_DETAILS.Any(f => e.TRNSID.Equals(f.TRNSID) && e.QTY - f.DEL_QTY > 0) :
                        DenimDbContext.F_SAMPLE_GARMENT_RCV_D.Any(f => f.QTY > 0))
                .Select(e => new ExtendFSampleGarmentRcvD
                {
                    TRNSID = e.TRNSID,
                    ExsItem = $"[ID - {e.TRNSID}] Item name: {e.SITEM.NAME}, " +
                              $"Style: {e.FABCODENavigation.STYLE_NAME}, " +
                              $"Color: {e.FABCODENavigation.COLORCODENavigation.COLOR}, " +
                              $"Remaining: {(DenimDbContext.F_SAMPLE_DESPATCH_DETAILS.Any(f => e.TRNSID.Equals(f.TRNSID) && (e.QTY - DenimDbContext.F_SAMPLE_DESPATCH_DETAILS.Where(f1 => e.TRNSID.Equals(f1.TRNSID)).Sum(g1 => g1.DEL_QTY)) > 0) ? (e.QTY - DenimDbContext.F_SAMPLE_DESPATCH_DETAILS.Where(f => e.TRNSID.Equals(f.TRNSID)).Sum(g => g.DEL_QTY)) : e.QTY)}"
                })
                .OrderBy(e => e.TRNSID)
                .ToListAsync();

            var filterStrings = new[] { "mtr", "pcs", "yds", "pkt" };

            createFSampleDesPatchMasterViewModel.FBasUnitses = await DenimDbContext.F_BAS_UNITS
                .Where(e => filterStrings.Any(f => f.Contains(e.UNAME.ToLower())))
                .Select(e => new F_BAS_UNITS
                {
                    UID = e.UID,
                    UNAME = e.UNAME
                }).OrderBy(e => e.UNAME).ToListAsync();

            createFSampleDesPatchMasterViewModel.BasBuyerinfos = await DenimDbContext.BAS_BUYERINFO.Select(e => new BAS_BUYERINFO { BUYERID = e.BUYERID, BUYER_NAME = e.BUYER_NAME }).OrderBy(e => e.BUYER_NAME).ToListAsync();
            createFSampleDesPatchMasterViewModel.GatepassTypes = await DenimDbContext.GATEPASS_TYPE.OrderBy(e => e.GPTYPENAME).ToListAsync();

            return createFSampleDesPatchMasterViewModel;
        }

        public async Task<CreateFSampleDesPatchMasterViewModel> GetInitObjectsOfSelectedItems(CreateFSampleDesPatchMasterViewModel createFSampleDesPatchMasterViewModel)
        {
            foreach (var item in createFSampleDesPatchMasterViewModel.FSampleDespatchDetailses)
            {
                item.TRNS = await DenimDbContext.F_SAMPLE_GARMENT_RCV_D
                    .Include(e => e.SITEM)
                    .FirstOrDefaultAsync(e => e.TRNSID.Equals(item.TRNSID));
                item.BYER = await DenimDbContext.BAS_BUYERINFO.FirstOrDefaultAsync(e => e.BUYERID.Equals(item.BYERID));
                item.U = await DenimDbContext.F_BAS_UNITS.FirstOrDefaultAsync(e => e.UID.Equals(item.UID));
            }

            return createFSampleDesPatchMasterViewModel;
        }

        public async Task<DataTableObject<ExtendFSampleDespatchMasterViewModel>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip,
            int pageSize)
        {
            var navigationPropertyStrings = new[] { "DR", "V", "GPTYPE", "FSampleDespatchMasterType" };
            var fSampleDespatchMasters = DenimDbContext.F_SAMPLE_DESPATCH_MASTER
                .Include(e => e.DR)
                .Include(e => e.V)
                .Include(e => e.GPTYPE)
                .Include(e => e.FSampleDespatchMasterType)
                .Select(e => new ExtendFSampleDespatchMasterViewModel
                {
                    DPID = e.DPID,
                    EncryptedId = _protector.Protect(e.DPID.ToString()),
                    GPNO = e.GPNO,
                    GPDATE = e.GPDATE,
                    GPTYPE = e.GPTYPE,
                    DR = e.DR,
                    V = e.V,
                    REMARKS = e.REMARKS,
                    FSampleDespatchMasterType = new F_SAMPLE_DESPATCH_MASTER_TYPE
                    {
                        TYPENAME = e.FSampleDespatchMasterType.TYPENAME
                    },
                    IsLocked = e.CREATED_AT < DateTime.Now.AddDays(-2)
                });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                fSampleDespatchMasters = OrderedResult<ExtendFSampleDespatchMasterViewModel>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, fSampleDespatchMasters);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                fSampleDespatchMasters = fSampleDespatchMasters
                    .Where(m => m.GPNO.ToString().ToUpper().Contains(searchValue)
                                || m.GPDATE != null && m.GPDATE.ToString().ToUpper(CultureInfo.InvariantCulture).Contains(searchValue)
                                || m.GPTYPE.GPTYPENAME != null && m.GPTYPE.GPTYPENAME.ToUpper().Contains(searchValue)
                                || m.DR.DRIVER_NAME != null && m.DR.DRIVER_NAME.ToUpper().Contains(searchValue)
                                || m.V.VNUMBER != null && m.V.VNUMBER.ToUpper().Contains(searchValue)
                                || m.FSampleDespatchMasterType.TYPENAME != null && m.FSampleDespatchMasterType.TYPENAME.ToUpper().Contains(searchValue)
                                || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue));

                fSampleDespatchMasters = OrderedResult<ExtendFSampleDespatchMasterViewModel>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, fSampleDespatchMasters);
            }

            var recordsTotal = await fSampleDespatchMasters.CountAsync();

            return new DataTableObject<ExtendFSampleDespatchMasterViewModel>(draw, recordsTotal, recordsTotal, await fSampleDespatchMasters.Skip(skip).Take(pageSize).ToListAsync());
        }

        public async Task<CreateFSampleDesPatchMasterViewModel> FindByDispatchIdIncludeAllAsync(int dispatchId)
        {
            var createFSampleDesPatchMasterViewModel = await DenimDbContext.F_SAMPLE_DESPATCH_MASTER
                    .Include(e => e.DR)
                    .Include(e => e.V)
                    .Include(e => e.GPTYPE)
                    .Include(e => e.FSampleDespatchMasterType)
                    .GroupJoin(DenimDbContext.F_SAMPLE_DESPATCH_DETAILS
                            .Include(e => e.BYER)
                            .Include(e => e.DP)
                            .Include(e => e.U)
                            .Include(e => e.TRNS)
                            .ThenInclude(e => e.SITEM),
                        f1 => f1.DPID,
                        f2 => f2.DPID,
                        (f1, f2) => new CreateFSampleDesPatchMasterViewModel
                        {
                            FSampleDespatchMaster = new F_SAMPLE_DESPATCH_MASTER
                            {
                                DPID = f1.DPID,
                                EncryptedId = _protector.Protect(f1.DPID.ToString()),
                                GPNO = f1.GPNO,
                                GPDATE = f1.GPDATE,
                                GPTYPEID = f1.GPTYPEID,
                                GPTYPE = f1.GPTYPE,
                                DRID = f1.DRID,
                                VID = f1.VID,
                                DR = f1.DR,
                                V = f1.V,
                                CREATED_AT = f1.CREATED_AT,
                                REMARKS = f1.REMARKS,
                                TYPEID = f1.TYPEID,
                                FSampleDespatchMasterType = f1.FSampleDespatchMasterType
                            },
                            FSampleDespatchDetailses = f2.ToList()
                        }).FirstOrDefaultAsync(e => e.FSampleDespatchMaster.DPID.Equals(dispatchId));

            createFSampleDesPatchMasterViewModel.IsLocked = createFSampleDesPatchMasterViewModel.FSampleDespatchMaster.CREATED_AT < DateTime.Now.AddDays(-2);

            return createFSampleDesPatchMasterViewModel;
        }

        public async Task<int> GetGatePassNumber()
        {
            try
            {
                var fSampleDespatchMaster = await DenimDbContext.F_SAMPLE_DESPATCH_MASTER.OrderBy(e => e.GPNO).LastOrDefaultAsync();
                return fSampleDespatchMaster?.GPNO + 1 ?? 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public async Task<ContainsFsampleGarmentRcvd> GetNumberOfTotalItems(int trnsId)
        {
            try
            {
                var firstOrDefaultAsync = await DenimDbContext.F_SAMPLE_GARMENT_RCV_D
                    .Include(e => e.FABCODENavigation.WV)
                    .Include(e => e.COLOR)
                    .Where(e => DenimDbContext.F_SAMPLE_DESPATCH_DETAILS.Any(f => e.TRNSID.Equals(f.TRNSID))
                            ? DenimDbContext.F_SAMPLE_DESPATCH_DETAILS.Any(f => e.TRNSID.Equals(f.TRNSID) && e.QTY - f.DEL_QTY > 0)
                            : DenimDbContext.F_SAMPLE_GARMENT_RCV_D.Any(f => f.QTY > 0))
                    .Select(e => new
                    {
                        TRNSID = e.TRNSID,
                        FABCODE = e.FABCODENavigation.STYLE_NAME,
                        Remainings = (DenimDbContext.F_SAMPLE_DESPATCH_DETAILS.Any(f => e.TRNSID.Equals(f.TRNSID) && (e.QTY - DenimDbContext.F_SAMPLE_DESPATCH_DETAILS.Where(f1 => e.TRNSID.Equals(f1.TRNSID)).Sum(g1 => g1.DEL_QTY)) > 0)
                            ? (e.QTY - DenimDbContext.F_SAMPLE_DESPATCH_DETAILS.Where(f => e.TRNSID.Equals(f.TRNSID)).Sum(g => g.DEL_QTY))
                            : e.QTY)
                    })
                    .OrderBy(e => e.Remainings)
                    .FirstOrDefaultAsync(e => e.TRNSID.Equals(trnsId));

                return new ContainsFsampleGarmentRcvd(firstOrDefaultAsync.Remainings, firstOrDefaultAsync.FABCODE);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<F_SAMPLE_GARMENT_RCV_D> FindByBarcodeAsync(string barcode)
        {
            return await DenimDbContext.F_SAMPLE_GARMENT_RCV_D
                .Include(e => e.SITEM)
                .FirstOrDefaultAsync(e => e.BARCODE.Equals(barcode));
        }
    }
}
