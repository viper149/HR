using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.SampleGarments.Fabric;
using DenimERP.ViewModels;
using DenimERP.ViewModels.SampleFabric;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.SampleGarments.Fabric
{
    public class SQLH_SAMPLE_FABRIC_RECEIVING_M_Repository : BaseRepository<H_SAMPLE_FABRIC_RECEIVING_M>, IH_SAMPLE_FABRIC_RECEIVING_M
    {
        private readonly IDataProtector _protector;

        public SQLH_SAMPLE_FABRIC_RECEIVING_M_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<CreateHSampleFabricReceivingM> GetInitObjsByAsync(CreateHSampleFabricReceivingM createHSampleFabricReceivingM)
        {
            createHSampleFabricReceivingM.FSampleFabricDispatchMasters = await DenimDbContext.F_SAMPLE_FABRIC_DISPATCH_MASTER
                .OrderByDescending(e => e.GPNO)
                .Select(e => new F_SAMPLE_FABRIC_DISPATCH_MASTER
                {
                    DPID = e.DPID,
                    GPNO = e.GPNO
                })
                .ToListAsync();

            return createHSampleFabricReceivingM;
        }

        public async Task<DataTableObject<H_SAMPLE_FABRIC_RECEIVING_M>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip,
            int pageSize)
        {
            var navigationPropertyStrings = new[] { "FSampleFabricDispatchMaster" };
            var hSampleFabricReceivingMs = DenimDbContext.H_SAMPLE_FABRIC_RECEIVING_M
                .Include(e => e.FSampleFabricDispatchMaster)
                .Select(e => new H_SAMPLE_FABRIC_RECEIVING_M
                {
                    RCVID = e.RCVID,
                    RCVDATE = e.RCVDATE,
                    EncryptedId = _protector.Protect(e.RCVID.ToString()),
                    DPID = e.DPID,
                    REMARKS = e.REMARKS,
                    FSampleFabricDispatchMaster = new F_SAMPLE_FABRIC_DISPATCH_MASTER
                    {
                        GPNO = e.FSampleFabricDispatchMaster.GPNO
                    }
                }).AsQueryable();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                hSampleFabricReceivingMs = OrderedResult<H_SAMPLE_FABRIC_RECEIVING_M>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, hSampleFabricReceivingMs);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                hSampleFabricReceivingMs = hSampleFabricReceivingMs
                    .Where(m => m.RCVDATE.ToString().ToUpper().Contains(searchValue)
                                || m.RCVID.ToString().ToUpper().Contains(searchValue)
                                || m.FSampleFabricDispatchMaster.GPNO != null && m.FSampleFabricDispatchMaster.GPNO.ToString().ToUpper().Contains(searchValue)
                                || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue));

                hSampleFabricReceivingMs = OrderedResult<H_SAMPLE_FABRIC_RECEIVING_M>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, hSampleFabricReceivingMs);
            }

            var recordsTotal = await hSampleFabricReceivingMs.CountAsync();

            return new DataTableObject<H_SAMPLE_FABRIC_RECEIVING_M>(draw, recordsTotal, recordsTotal, await hSampleFabricReceivingMs.Skip(skip).Take(pageSize).ToListAsync());
        }

        public async Task<CreateHSampleFabricReceivingM> GetHSampleFabricReceiveDetailsByAsync(CreateHSampleFabricReceivingM createHSampleFabricReceivingM)
        {
            if (createHSampleFabricReceivingM.HSampleFabricReceivingM.DPID is > 0 &&
                DenimDbContext.F_SAMPLE_FABRIC_DISPATCH_MASTER
                    .Include(e => e.F_SAMPLE_FABRIC_DISPATCH_DETAILS)
                    .Where(e => e.DPID.Equals(createHSampleFabricReceivingM.HSampleFabricReceivingM.DPID))
                    .Any(e => e.F_SAMPLE_FABRIC_DISPATCH_DETAILS.All(f =>
                        createHSampleFabricReceivingM.HSampleFabricReceivingDs.All(g => !g.DPDID.Equals(f.DPDID)))))
            {

                createHSampleFabricReceivingM.HSampleFabricReceivingDs.RemoveRange(0, createHSampleFabricReceivingM.HSampleFabricReceivingDs.Count);
                createHSampleFabricReceivingM.HSampleFabricReceivingDs.AddRange(await DenimDbContext.F_SAMPLE_FABRIC_DISPATCH_MASTER
                    .Where(e => e.DPID.Equals(createHSampleFabricReceivingM.HSampleFabricReceivingM.DPID))
                    .SelectMany(e => e.F_SAMPLE_FABRIC_DISPATCH_DETAILS.Select(f => new H_SAMPLE_FABRIC_RECEIVING_D
                    {
                        DPDID = f.DPDID,
                        QTY = f.DEL_QTY,
                        REMARKS = f.REMARKS
                    })).ToListAsync());
            }

            return createHSampleFabricReceivingM;
        }

        public async Task<CreateHSampleFabricReceivingM> GetInitObjsForDetailsTableByAsync(CreateHSampleFabricReceivingM createHSampleFabricReceivingM)
        {
            foreach (var item in createHSampleFabricReceivingM.HSampleFabricReceivingDs)
            {
                item.DPD = await DenimDbContext.F_SAMPLE_FABRIC_DISPATCH_DETAILS
                    .Include(e => e.FSampleFabricRcvD.SITEM)
                    .FirstOrDefaultAsync(e => e.DPDID.Equals(item.DPDID));
            }

            return createHSampleFabricReceivingM;
        }

        public async Task<CreateHSampleFabricReceivingM> FindByIdIncludeAllAsync(int rcvId)
        {
            return await DenimDbContext.H_SAMPLE_FABRIC_RECEIVING_M
                .Include(e => e.H_SAMPLE_FABRIC_RECEIVING_D)
                .Where(e => e.RCVID.Equals(rcvId))
                .Select(e => new CreateHSampleFabricReceivingM
                {
                    HSampleFabricReceivingM = new H_SAMPLE_FABRIC_RECEIVING_M
                    {
                        RCVID = e.RCVID,
                        EncryptedId = _protector.Protect(e.RCVID.ToString()),
                        DPID = e.DPID,
                        RCVDATE = e.RCVDATE,
                        REMARKS = e.REMARKS
                    },
                    HSampleFabricReceivingDs = e.H_SAMPLE_FABRIC_RECEIVING_D.Select(f => new H_SAMPLE_FABRIC_RECEIVING_D
                    {
                        RCVDID = f.RCVDID,
                        RCVID = f.RCVID,
                        DPDID = f.DPDID,
                        QTY = f.QTY,
                        REMARKS = f.REMARKS
                    }).ToList()
                }).FirstOrDefaultAsync();
        }
    }
}
