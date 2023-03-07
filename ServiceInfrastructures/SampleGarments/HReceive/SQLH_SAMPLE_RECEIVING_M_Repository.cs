using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.SampleGarments.HReceive;
using DenimERP.ViewModels;
using DenimERP.ViewModels.SampleGarments.HReceive;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.SampleGarments.HReceive
{
    public class SQLH_SAMPLE_RECEIVING_M_Repository : BaseRepository<H_SAMPLE_RECEIVING_M>, IH_SAMPLE_RECEIVING_M
    {
        private readonly IDataProtector _protector;

        public SQLH_SAMPLE_RECEIVING_M_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<CreateHSampleReceivingMViewModel> GetInitObjects(CreateHSampleReceivingMViewModel createHSampleReceivingMViewModel)
        {
            createHSampleReceivingMViewModel.FSampleDespatchMasters = await DenimDbContext.F_SAMPLE_DESPATCH_MASTER

                .Select(e => new F_SAMPLE_DESPATCH_MASTER
                {
                    DPID = e.DPID,
                    GPNO = e.GPNO,
                    GPDATE = e.GPDATE
                })
                .Where(e => !DenimDbContext.H_SAMPLE_RECEIVING_M.Any(f => int.Parse(f.GPNO).Equals(e.GPNO)))
                .OrderBy(e => e.GPDATE).ToListAsync();

            return createHSampleReceivingMViewModel;
        }

        public async Task<CreateHSampleReceivingMViewModel> GetFactoryGatePassReceiveDetails(int dpId)
        {
            var createHSampleReceivingMViewModel = await DenimDbContext.F_SAMPLE_DESPATCH_MASTER
                .Include(e => e.GPTYPE)
                .Include(e => e.DR)
                .Include(e => e.V)
                .Include(e => e.F_SAMPLE_DESPATCH_DETAILS)
                .ThenInclude(e => e.TRNS)
                .ThenInclude(e => e.SITEM)
                .Include(e => e.F_SAMPLE_DESPATCH_DETAILS)
                .ThenInclude(e => e.BYER)
                .Include(e => e.F_SAMPLE_DESPATCH_DETAILS)
                .ThenInclude(e => e.U)
                .Select(e => new CreateHSampleReceivingMViewModel
                {
                    FSampleDespatchMaster = e
                })
                .FirstOrDefaultAsync(e => e.FSampleDespatchMaster.DPID.Equals(dpId));

            return createHSampleReceivingMViewModel;
        }

        public async Task<DataTableObject<H_SAMPLE_RECEIVING_M>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip,
            int pageSize)
        {
            var navigationPropertyStrings = new[] { "DP", "DR", "V" };
            var hSampleReceivingMs = DenimDbContext.H_SAMPLE_RECEIVING_M
                //.Include(e => e.DP)
                .Include(e => e.DR)
                .Include(e => e.V)
                .Select(e => new H_SAMPLE_RECEIVING_M
                {
                    RCVID = e.RCVID,
                    RCVDATE = e.RCVDATE,
                    EncryptedId = _protector.Protect(e.RCVID.ToString()),
                    DPID = e.DPID,
                    GPNO = e.GPNO,
                    GPDATE = e.GPDATE,
                    DRID = e.DRID,
                    VID = e.VID,
                    REMARKS = e.REMARKS,
                    DR = e.DR,
                    V = e.V
                })
                .AsQueryable();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                hSampleReceivingMs = OrderedResult<H_SAMPLE_RECEIVING_M>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, hSampleReceivingMs);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                hSampleReceivingMs = hSampleReceivingMs
                    .Where(m => m.RCVDATE.ToString().ToUpper(CultureInfo.InvariantCulture).Contains(searchValue)
                                || m.GPDATE != null && m.GPDATE.ToString().ToUpper(CultureInfo.InvariantCulture).Contains(searchValue)
                                || m.GPNO != null && m.GPDATE.ToString().ToUpper(CultureInfo.InvariantCulture).Contains(searchValue)
                                || m.DR.DRIVER_NAME != null && m.DR.DRIVER_NAME.ToUpper().Contains(searchValue)
                                || m.V.VNUMBER != null && m.V.VNUMBER.ToUpper().Contains(searchValue)
                                || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue));

                hSampleReceivingMs = OrderedResult<H_SAMPLE_RECEIVING_M>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, hSampleReceivingMs);
            }

            var recordsTotal = await hSampleReceivingMs.CountAsync();

            return new DataTableObject<H_SAMPLE_RECEIVING_M>(draw, recordsTotal, recordsTotal, await hSampleReceivingMs.Skip(skip).Take(pageSize).ToListAsync());
        }

        public async Task<CreateHSampleReceivingMViewModel> FindByHsrIdIncludeAllAsync(int hsrId)
        {
            var result = await DenimDbContext.H_SAMPLE_RECEIVING_M
                .Include(e => e.DR)
                .Include(e => e.V)
                .Include(e => e.H_SAMPLE_RECEIVING_D)
                .ThenInclude(e => e.TRNS)
                .ThenInclude(e => e.SITEM)
                .Include(e => e.H_SAMPLE_RECEIVING_D)
                .ThenInclude(e => e.BUYER)
                .Include(e => e.H_SAMPLE_RECEIVING_D)
                .ThenInclude(e => e.U)
                .Select(e => new CreateHSampleReceivingMViewModel
                {
                    HSampleReceivingM = e
                }).FirstOrDefaultAsync(e => e.HSampleReceivingM.RCVID.Equals(hsrId));

            result.HSampleReceivingM.EncryptedId = _protector.Protect(result.HSampleReceivingM.RCVID.ToString());

            return result;
        }
    }
}
